import nearAPI from "near-api-js";
import fs from 'fs'
import os from 'os'
import path from 'path'
import crypto from 'crypto';

import nftData, { lvl_nft } from "./nftData.js";
console.log('DEFAULT GAS', nearAPI.DEFAULT_FUNCTION_CALL_GAS.toString())
const connectToNear = async () => {
	const { keyStores, connect } = nearAPI;

	// creates a keyStore that searches for keys in .near-credentials
	// requires credentials stored locally by using a NEAR-CLI command: `near login` 
	const homedir = os.homedir();
	const CREDENTIALS_DIR = ".near-credentials";
	const credentialsPath = path.join(homedir, CREDENTIALS_DIR);
	const myKeyStore = new keyStores.UnencryptedFileSystemKeyStore(credentialsPath);

	const connectionConfig = {
		networkId: "testnet",
		keyStore: myKeyStore,
		nodeUrl: "https://rpc.testnet.near.org",
		walletUrl: "https://wallet.testnet.near.org",
		helperUrl: "https://helper.testnet.near.org",
		explorerUrl: "https://explorer.testnet.near.org",
	};
	const nearConnection = await connect(connectionConfig);
	return nearConnection
}

let subaccount_id = 'nftv4.relik.testnet'
let account_id = 'relik.testnet'
let token_account = 'goldtoken.relik.testnet'

const deployNftContract = async (account) => {
	const accountKeys = await account.getAccessKeys();
	console.log('accountKeys', accountKeys)
	// return;

	const response = await account.createAndDeployContract(subaccount_id, accountKeys[0].public_key, fs.readFileSync('./build/nft.wasm'), '10455719995000000000000000');
	console.log("DEPLOY CONTRACT RESPONSE", response);

	// const resp = await account.deployContract(fs.readFileSync('./build/nft.wasm'))


	console.log('Loading the contract!')

	const nftContract = new nearAPI.Contract(account, subaccount_id, {
		viewMethods: ['nft_total_supply', 'nft_tokens', 'nft_tokens_for_owner', 'nft_supply_for_owner', 'nft_metadata'],
		changeMethods: ['init', 'nft_mint', 'nft_token', 'nft_transfer_call', 'nft_resolve_transfer', 'nft_approve', 'nft_transfer_payout', 'nft_revoke', 'nft_revoke_call']
	})

	console.log('Contract Loaded!', nftContract)
	console.log('initialized??', await nftContract.init({
		args: {
			owner_id: subaccount_id,
		},
		amount: '10000000000000'
	}))
}

const mint_nft = async (account) => {
	const nftContract = new nearAPI.Contract(account, subaccount_id, {
		viewMethods: ['nft_total_supply', 'nft_tokens', 'nft_tokens_for_owner', 'nft_supply_for_owner', 'nft_metadata'],
		changeMethods: ['init', 'nft_mint', 'nft_token', 'nft_transfer_call', 'nft_resolve_transfer', 'nft_approve', 'nft_transfer_payout', 'nft_revoke', 'nft_revoke_call']
	})

	let media_url = 'https://opengameart.org/sites/default/files/styles/medium/public/w_shortsword.png'

	console.log('Minting NFTs....')

	let keys = Object.keys(nftData)

	for (let i = 0; i < 100; i++) {
		let rand = keys[Math.floor(Math.random() * 10) + 1]

		while (rand === undefined) rand = keys[Math.floor(Math.random() * 10) + 1]

		console.log('Index', i)
		console.log('Rand', rand)

		await nftContract.nft_mint({
			args: {
				token_id: i,
				metadata: nftData[rand].metadata,
				receiver_id: account_id
			},
			amount: '20390000000000000000000',

		})

		console.log('Minted Successfully ✅')
	}

	await nftContract.nft_mint({
		args: {
			token_id: 9000,
			metadata: lvl_nft.metadata,
			receiver_id: account_id
		},
		amount: '20390000000000000000000',
	})
}

const send_nft = async (account) => {
	const nftContract = new nearAPI.Contract(account, subaccount_id, {
		viewMethods: ['nft_total_supply', 'nft_tokens', 'nft_tokens_for_owner', 'nft_supply_for_owner', 'nft_metadata'],
		changeMethods: ['init', 'nft_mint', 'nft_token', 'nft_transfer_call', 'nft_resolve_transfer', 'nft_approve', 'nft_transfer_payout', 'nft_revoke', 'nft_revoke_call', 'nft_transfer']
	})

	for (let i = 0; i < 10; i++) {
		let random = Math.floor(Math.random() * 120) + 1


		await nftContract.nft_transfer({
			args: {
				receiver_id: 'pocket.testnet',
				token_id: random,
			},
			amount: '1'
		})
	}

	console.log('Sent! ✅')

}

const nftForOwner = async (account) => {
	const nftContract = new nearAPI.Contract(account, subaccount_id, {
		viewMethods: ['nft_total_supply', 'nft_tokens', 'nft_tokens_for_owner', 'nft_supply_for_owner', 'nft_metadata'],
		changeMethods: ['init', 'nft_mint', 'nft_token', 'nft_transfer_call', 'nft_resolve_transfer', 'nft_approve', 'nft_transfer_payout', 'nft_revoke', 'nft_revoke_call', 'nft_transfer']
	})

	console.log('NFTs FOr Owner', await nftContract.nft_tokens_for_owner({ args: { account_id: 'cardi.testnet' } }))
}



const deployFtContract = async (account) => {
	const accountKeys = await account.getAccessKeys()


	console.log('Access Keys', accountKeys)

	const response = await account.createAndDeployContract(token_account, accountKeys[0].public_key, fs.readFileSync('./build/ft.wasm'), '10455719995000000000000000')
	console.log("Contract Deploy Response", response)

	const ftContract = new nearAPI.Contract(account, token_account, {
		viewMethods: ['ft_total_supply', 'ft_balance_of'],
		changeMethods: ['init', 'storage_deposit', 'ft_transfer', 'ft_transfer_call'],
	})

	console.log('Initialized', await ftContract.init({
		args: {
			owner_id: account_id,
			metadata: {
				spec: 'v1.0',
				name: 'Relik Gold Token',
				symbol: 'GOLD',
				icon: 'https://opengameart.org/sites/default/files/styles/medium/public/goldCoin1.png',
				decimals: 0,
			},
			total_supply: "1000000000"
		},

	}))
}

const transferGoldTokens = async (account) => {
	const ftContract = new nearAPI.Contract(account, token_account, {
		viewMethods: ['ft_total_supply', 'ft_balance_of'],
		changeMethods: ['init', 'storage_deposit', 'ft_transfer', 'ft_transfer_call'],
	})

	console.log('Total Supply', await ftContract.ft_total_supply())
	console.log('Registering account', await ftContract.storage_deposit({
		args: {
			account_id: 'pocket.testnet',
		},
		amount: '10000000000000000'
	}))
	console.log('Balance of Owner', await ftContract.ft_balance_of({
		account_id: 'pocket.testnet'
	}))

	console.log('Transferring Tokens', await ftContract.ft_transfer({
		args: {
			receiver_id: 'pocket.testnet',
			amount: '100',
			memo: 'test transfer'
		},
		amount: '10000000000000'
	}))

	console.log('Pocket balance', await ftContract.ft_balance_of({
		account_id: 'pocket.testnet'
	}))
}

const increaseExp = async (account) => {
	const nftContract = new nearAPI.Contract(account, subaccount_id, {
		viewMethods: ['nft_total_supply', 'nft_tokens', 'nft_tokens_for_owner', 'nft_supply_for_owner', 'nft_metadata'],
		changeMethods: ['init', 'increase_exp', 'nft_mint', 'nft_token', 'nft_transfer_call', 'nft_resolve_transfer', 'nft_approve', 'nft_transfer_payout', 'nft_revoke', 'nft_revoke_call', 'nft_transfer']
	})

	// await nftContract.increase_exp({
	// 	args: {
	// 		token_id: 35
	// 	},
	// 	gas: '5000000000000'
	// })

	await nftContract.increase_exp({ token_id: 35 }, '300000000000000')

}

// const initializeContract = async ()

const runscript = async () => {
	const { Contract } = nearAPI;


	const nearConnection = await connectToNear()
	const account = await nearConnection.account(account_id);
	const contractAccount = await nearConnection.account(subaccount_id)
	console.log('Testnet Account Details', await account.getAccountDetails())

	// await deployNftContract(account)
	// await mint_nft(account)
	// await send_nft(account)
	// await nftForOwner(account)
	await increaseExp(account)


	// GOLD Token
	// await deployFtContract(account)
	// await transferGoldTokens(account)
}


// mint_nft()
runscript()





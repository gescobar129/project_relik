import * as functions from "firebase-functions";
import { GetAccountResponse, NFT } from "./types";
import { filter } from 'lodash';

const nearAPI = require("near-api-js");


const nftaccount_id = 'poopypants666.poopypants.testnet'
const account_id = 'poopypants.testnet'
const ftaccount_id = 'goldtokenv2.poopypants.testnet'

const connectToNear = async () => {
  const { keyStores, connect } = nearAPI;

  // creates a keyStore that searches for keys in .near-credentials
  // requires credentials stored locally by using a NEAR-CLI command: `near login` 
  const homedir = require("os").homedir();
  const CREDENTIALS_DIR = ".near-credentials";
  const credentialsPath = require("path").join(homedir, CREDENTIALS_DIR);
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


const loadContracts = async () => {
  const { Contract } = nearAPI;
  const nearConnection = await connectToNear()
  const account = await nearConnection.account(account_id);

  const nftContract = new Contract(
    account,
    nftaccount_id,
    {
      viewMethods: ['nft_total_supply', 'nft_tokens', 'nft_tokens_for_owner', 'nft_supply_for_owner', 'nft_metadata'],
      changeMethods: ['init', 'nft_mint', 'nft_token', 'nft_transfer_call', 'nft_resolve_transfer', 'nft_approve', 'nft_transfer_payout', 'nft_revoke', 'nft_revoke_call', 'nft_transfer']
    }
  );

  const ftContract = new Contract(
    account,
    ftaccount_id,
    {
      viewMethods: ['ft_total_supply', 'ft_balance_of'],
      changeMethods: ['init', 'storage_deposit', 'ft_transfer', 'ft_transfer_call'],
    }
  )

  console.log('nftContract', nftContract)
  console.log('ftContract', ftContract)

  return {
    nftContract,
    ftContract
  }
}


// // Start writing Firebase Functions
// // https://firebase.google.com/docs/functions/typescript
//
// export const helloWorld = functions.https.onRequest((request, response) => {
//   functions.logger.info("Hello logs!", {structuredData: true});
//   response.send("Hello from Firebase!");
// });

export const getGameAccount = functions.https.onRequest(async (request, response) => {
  const nearConnection = await connectToNear()
  // Request simply uses an account_id query param
  const account_id = request.query.account_id as string;
  const contracts = await loadContracts()

  if (!account_id) {
    throw new functions.https.HttpsError('invalid-argument', 'account_id is undefined. Please send a valid account_id with your request')
  }

  console.log('response', response)

  const userData: GetAccountResponse = {
    accountId: account_id,
    nearBalance: 0,
    goldBalance: 0,
    ownedNfts: {
      loot: [],
      characters: [],
    }
  }

  // get NEAR balance
  const account = await nearConnection.account(account_id)
  userData.nearBalance = await account.getAccountBalance();

  // get NFTs for account
  const nfts = await contracts.nftContract.nft_tokens_for_owner({ account_id })
  userData.ownedNfts.characters = filter(nfts, (nft: NFT) => {
    const extraData = JSON.parse(nft.metadata.extra)
    return extraData.type === 'character'
  });

  userData.ownedNfts.loot = filter(nfts, (nft: NFT) => {
    const extraData = JSON.parse(nft.metadata.extra)
    return extraData.type !== 'character'
  })

  // get Gold Token Balance for account
  userData.goldBalance = await contracts.ftContract.ft_balance_of({ account_id })


  response.status(200).send(userData)
})
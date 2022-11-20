import * as functions from "firebase-functions";
import { GetAccountResponse, NFT } from "./types";
import { filter, find } from 'lodash';

// @ts-ignore
const nearAPI = require("near-api-js");


const nftaccount_id = 'nftv2.relik.testnet'
const account_id = 'relik.testnet'
const ftaccount_id = 'goldtoken.relik.testnet'

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
      changeMethods: ['init', 'increase_exp', 'nft_mint', 'nft_token', 'nft_transfer_call', 'nft_resolve_transfer', 'nft_approve', 'nft_transfer_payout', 'nft_revoke', 'nft_revoke_call', 'nft_transfer']
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

/**
 * Gets the user's account, along with the NEAR and GOLD balances. Also returns the 
 * NFTs/Loot owned by the user
 * 
 * url: https://us-central1-dao-v-player.cloudfunctions.net/getGameAccount?account_id=${account_id}
 * 
 * @param {functions.https.Request} request - The Request object.
 */

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
  try {
    const nfts = await contracts.nftContract.nft_tokens_for_owner({ account_id })
    userData.ownedNfts.characters = filter(nfts, (nft: NFT) => {
      const extraData = JSON.parse(nft.metadata.extra)
      return extraData.type === 'character'
    });
    userData.ownedNfts.loot = filter(nfts, (nft: NFT) => {
      const extraData = JSON.parse(nft.metadata.extra)
      return extraData.type !== 'character'
    })

  } catch (err) {
    console.log('Not registered for NFTs!')
  }


  try {
    // get Gold Token Balance for account
    userData.goldBalance = await contracts.ftContract.ft_balance_of({ account_id })

  } catch (err) {
    console.log('Error! Account not registered for GoldToken!')
    console.log('Registering....')

    await contracts.ftContract.storage_deposit({
      args: {
        account_id
      },
      amount: '10000000000000000'
    })
  }


  response.status(200).send(userData)
})

/**
 * Called when an enemy is killed by the player. Increases player NFT experience points
 * 
 * url: https://us-central1-dao-v-player.cloudfunctions.net/onKillEnemy?token_id=${token_id}
 * token_id: The token_id of the NFT we want to add experience points to
 * 
 * @param {functions.https.Request} request - The Request object.
 * 
 */

export const onKillEnemy = functions.https.onRequest(async (request, response) => {
  const token_id = request.query.token_id as string;
  const contracts = await loadContracts()

  if (contracts.nftContract) {
    await contracts.nftContract.increase_exp({
      args: {
        token_id
      }
    })

    const nfts = await contracts.nftContract.nft_tokens_for_owner({ account_id })
    const currentNft = find(nfts, { token_id });

    if (currentNft) {
      response.status(200).send(currentNft)
      return
    }

    response.status(500).send({
      error: 'NFT not found!'
    })
    return
  }

  response.status(500).send({
    error: 'Was unable to load NFT contract'
  })
})


/**
 * Gets the user's account, along with the NEAR and GOLD balances. Also returns the 
 * NFTs/Loot owned by the user
 * 
 * url: https://us-central1-dao-v-player.cloudfunctions.net/onPickUpLoot?token_id=${token_id}&gold_amount=${gold_amount}&account_id=${account_id}
 * 
 * account_id: Account/Wallet Id of the player
 * token_id: Id of loot that was picked up by the player. This loot/NFT will then be transferred to the users wallet
 * gold_amount: Amount of Gold tokens to award to the player
 * 
 * @param {functions.https.Request} request - The Request object.
 */


export const onPickUpLoot = functions.https.onRequest(async (request, response) => {
  const token_id = request.query.token_id as string | undefined;
  const gold_amount = request.query.gold_amount as string;
  const account_id = request.query.account_id as string;

  const contracts = await loadContracts()

  const responseData = {
    totalBalance: '0',
    nfts: []
  }

  if (contracts.ftContract && contracts.nftContract) {
    // Register account, if not already registered
    await contracts.ftContract.storage_deposit({
      args: {
        account_id
      },
      amount: '10000000000000000'
    })

    // Transfer Token Amount to User
    await contracts.ftContract.ft_transfer({
      args: {
        receiver_id: account_id,
        amount: gold_amount,
        memo: 'Gold Loot Rewards'
      },
      amount: '10000000000000'
    })

    // Get Token Balance for User
    const totalUserBalance = await contracts.ftContract.ft_balance_of({
      account_id
    })

    responseData.totalBalance = totalUserBalance


    // Send NFT (if any) 
    if (token_id) {
      await contracts.nftContract.nft_transfer({
        args: {
          receiver_id: account_id,
          token_id: token_id,
        },
        amount: '1'
      })
      const nfts = await contracts.nftContract.nft_tokens_for_owner({ account_id })
      responseData.nfts = nfts
    }
  }


  response.status(200).send(responseData)
})



// Called on loading the Unity Game engine. This will allow the game
// to know what NFTs are available for pick up, and which have been claimed by users
// additionally, it returns the LVL NFT token to the engine, so that the engine
// can populate the appropriate parameters
export const loadGameState = () => {


}


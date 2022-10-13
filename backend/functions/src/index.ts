import * as functions from "firebase-functions";
const nearAPI = require("near-api-js");
const fs = require('fs')

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

const deployContract = async () => {
  const nearConnection = await connectToNear()
  const account = await nearConnection.account("cardi.testnet");
  const response = await account.deployContract(fs.readFileSync('/Users/gaidaescobar/Development/platformer-game/contract/build/hello_near.wasm'));
  console.log("DEPLOY CONTRACT RESPONSE",response);
}

const loadContract = async () => {
  const { Contract } = nearAPI;

  await deployContract()

  const nearConnection = await connectToNear()
  const account = await nearConnection.account("cardi.testnet");

  const contract = new Contract(
    account,
    "cardi.testnet",
    {
      // name of contract you're connecting to
      viewMethods: ["get_greeting"], // view methods do not change state but usually return a value
      changeMethods: ["set_greeting"], // change methods modify state
      sender: account,
    }
  );
  
  console.log('CONTRACT******', contract)
  return contract
}

const callContract = async () => {
  const contract = await loadContract()
  // const greetingChange = await contract.set_greeting({
  //   message: "DBC!"
  // })
  const currentGreeting = await contract.get_greeting()
  // const response = await contract.get_greeting();
  // console.log('Greeting CHANGED to:',greetingChange);
  console.log('Current GREETING:', currentGreeting)
  return currentGreeting
}





// // Start writing Firebase Functions
// // https://firebase.google.com/docs/functions/typescript
//
// export const helloWorld = functions.https.onRequest((request, response) => {
  //   functions.logger.info("Hello logs!", {structuredData: true});
  //   response.send("Hello from Firebase!");
  // });

  // loadContract()
  // deployContract()
  callContract()
  // connectToNear()
export const startGame = functions.https.onRequest(async(request, response) => {
  const startGameData = await callContract()
  console.log('response', response)
  response.send(startGameData)
})
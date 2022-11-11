// React
import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';

import {
  createBrowserRouter,
  RouterProvider,
  Route,
} from "react-router-dom";

// NEAR
import { HelloNEAR } from './near-interface';
import { Wallet } from './near-wallet';

import 'bootstrap/dist/css/bootstrap.min.css';
import NavBar from './NavBar';
import NftPage from './NftPage';

// The Cloud Functions for Firebase SDK to create Cloud Functions and set up triggers.
// const functions = require('firebase-functions');

// The Firebase Admin SDK to access Firestore.
// const admin = require('firebase-admin');
// admin.initializeApp();

// When creating the wallet you can optionally ask to create an access key
// Having the key enables to call non-payable methods without interrupting the user to sign
const wallet = new Wallet({ createAccessKeyFor: process.env.CONTRACT_NAME })

// Abstract the logic of interacting with the contract to simplify your flow
const helloNEAR = new HelloNEAR({ contractId: process.env.CONTRACT_NAME, walletToUse: wallet });

// Setup on page load
window.onload = async () => {
  const isSignedIn = await wallet.startUp()

  const router = createBrowserRouter([
    {
      path: "/",
      element: (
        <App isSignedIn={isSignedIn} helloNEAR={helloNEAR} wallet={wallet} />
      ),
    },
    {
      path: "nft",
      element: <NftPage />
    },
  ]);
 
  ReactDOM.render(
    <div>
      <NavBar isSignedIn={isSignedIn} wallet={wallet}/>
      <RouterProvider router={router} />
    </div>,
    document.getElementById('root')
    );
  }

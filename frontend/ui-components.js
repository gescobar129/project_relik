import React from 'react';

export function SignInButton({onClick}) {
  return (
    <button onClick={onClick} style={{fontFamily: 'Kanit', backgroundColor: '#015E80', borderRadius: 12}}>Sign in with NEAR Wallet</button>
  );
}

export function SignOutButton({accountId, onClick}) {
  return (
    <button style={{ float: 'right', borderRadius: 12, backgroundColor: '#03293E', color: '#9E8475', fontSize:17, fontWeight: 'bold'}} onClick={onClick}>
      Sign out {accountId}
    </button>
  );
}

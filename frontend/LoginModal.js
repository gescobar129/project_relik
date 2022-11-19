import React, {useEffect} from 'react'

const LoginModal = ({wallet, isSignedIn}) => {

  const showAlert = () => {
    if (isSignedIn) {
      setTimeout(() => {
        alert(`Near wallet - ${wallet.accountId} successfully connected! 
        You may return to the game now.`)
        console.log("Delayed for 1 second.");
      }, "1000")
    } 
  }

  useEffect(() => {
    wallet.signIn()
    if (isSignedIn) {
      fetch(`http://localhost:2050/?walletid=${wallet.accountId}`)
      .then(() => {
        showAlert()
      })
      .catch((error) => {
        console.log('error', error)
      })
    }
  }, [])

  return (
    <></>
  )
}

export default LoginModal
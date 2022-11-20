import React from 'react'

import { Container, Navbar, Nav, NavItem } from 'react-bootstrap';
import { SignInPrompt, SignOutButton } from './ui-components';

// 9E8475
const NavBar = ({isSignedIn, wallet}) => {
  return (
    <Navbar collapseOnSelect expand="md" style={{backgroundColor: '#03293E', height: 45, fontSize: 17}}>
      <Container>
        <Navbar.Brand href="/" style={{fontWeight: 'bold', color: '#9F704E', letterSpacing: 1.5, fontSize: 28, fontFamily: 'Kanit'}}>Relik</Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        {isSignedIn && (
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="me-auto">
              {/* <Nav.Link href="nft" style={{fontSize: 17, fontWeight: 'bold', color: '#9E8475',}}>
                My NFTs
              </Nav.Link> */}
            </Nav>
            <Nav>
              <Nav.Link href="nft" style={{fontSize: 17, fontWeight: 'bolder', color: '#9E8475',}}>My NFTs</Nav.Link>
            </Nav>
          </Navbar.Collapse>
        )}
        {isSignedIn ?  (
          <SignOutButton accountId={wallet.accountId} onClick={()=> wallet.signOut()}/>
        ) : (
          <SignInPrompt onClick={() => wallet.signIn()}/>
        )}
      </Container>
    </Navbar>
  )
}

export default NavBar
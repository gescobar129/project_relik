import React from 'react'

import { Container, Navbar, Nav } from 'react-bootstrap';
import { SignInButton, SignOutButton } from './ui-components';

// 9E8475
const NavBar = ({isSignedIn, wallet}) => {
  return (
    <Navbar collapseOnSelect expand="md" style={{backgroundColor: '#03293E', height: 45, fontSize: 17}}>
      <Container>
        <Navbar.Brand 
          href="/" 
          style={{
            fontWeight: 'bold', 
            color: '#9F704E', 
            letterSpacing: 1.5, 
            fontSize: 28, 
            fontFamily: 'Kanit'
            }}>
              Relik
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        {isSignedIn && (
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="me-auto">
            </Nav>
            <Nav>
              <Nav.Link href="nft" style={{fontSize: 17, fontWeight: 'bolder', color: '#9E8475',  letterSpacing: 1}}>My NFTs</Nav.Link>
            </Nav>
          </Navbar.Collapse>
        )}
        {isSignedIn ?  (
          <SignOutButton accountId={wallet.accountId} onClick={()=> wallet.signOut()}/>
        ) : (
          <SignInButton onClick={() => wallet.signIn()}/>
        )}
      </Container>
    </Navbar>
  )
}

export default NavBar
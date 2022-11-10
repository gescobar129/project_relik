import React from 'react'

import { Container, Navbar, Nav, NavItem } from 'react-bootstrap';
import { SignInPrompt, SignOutButton } from './ui-components';


const NavBar = ({isSignedIn, wallet}) => {
  return (
    <Navbar collapseOnSelect expand="md" bg="dark" variant="dark">
      <Container>
        <Navbar.Brand href="#home">Relik</Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        {isSignedIn && (
          <Navbar.Collapse id="responsive-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link href="#features">NFT</Nav.Link>
              <Nav.Link href="#pricing">Pricing</Nav.Link>
            </Nav>
            {/* <Nav> */}
              {/* <Nav.Link href="#deets">NFT</Nav.Link> */}
              {/* <Nav.Link eventKey={2} href="#memes">button</Nav.Link> */}
            {/* </Nav> */}
          </Navbar.Collapse>
        )}
        {isSignedIn ?  (
          <SignOutButton accountId={wallet.accountId} onClick={()=> wallet.signOut()}/>
        ) : <SignInPrompt onClick={() => wallet.signIn()} />}
      </Container>
    </Navbar>
  )
}

export default NavBar
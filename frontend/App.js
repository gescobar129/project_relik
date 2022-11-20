import 'regenerator-runtime/runtime';
import React, {useEffect} from 'react';
import './assets/global.css';
import image from './assets/firewatch.jpeg'
import image3 from './assets/blue.jpg'

import { 
  Container,
  Row,
  Col,
} from 'react-bootstrap'

const App = ({ isSignedIn, helloNEAR, wallet }) => {
  const [nftData, setNftData] = React.useState()
  
  // const [uiPleaseWait, setUiPleaseWait] = React.useState(true);
  
  console.log('wallet info', wallet)
  console.log('nft data: ', nftData)
  
  /// If user not signed-in with wallet - show prompt
  // if (!isSignedIn) {
  //   return <LoginModal wallet={wallet} />
  // }
  
  useEffect(() => {
    helloNEAR.getNfts()
    .then(setNftData)
    .catch(alert)
  }, [])
    
  return (
      <Container fluid style={{backgroundColor: '#03293E'}}>
        <Row>
          <Col style={{padding: 0, display: 'flex',justifyContent: 'center',alignItems: 'center'}}>
            <img src={image3} style={{width: '100%', height: 650, objectFit: 'cover'}}/>
              <h1 style={{
                position: 'absolute', 
                top: '30%', 
                left: '15%', 
                transform: 'translate(-50%, -50%)', 
                fontSize: 120, 
                color: '#FFFFFF', 
                fontFamily: 'Staatliches' 
                }}>
                  Relik
              </h1>
              <span style={{
                position: 'absolute', 
                top: '55%', 
                left: '37%', 
                transform: 'translate(-50%, -50%)', 
                fontSize: 40, 
                color: '#FFFFFF', 
                fontFamily: 'Staatliches',
                letterSpacing: 1.5
                }}>
                  A journey into the unknown to defend the multiverse
              </span>
          </Col>
        </Row>
        <Row style={{marginTop: 40}}>
          <Col style={{display: 'flex',justifyContent: 'center',alignItems: 'center'}}>
              <iframe 
                width="70%" 
                height="499" 
                src="https://www.youtube.com/embed/cXWlgP5hZzc" 
                title="YouTube video player" 
                frameborder="0" 
                // allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" 
                allowfullscreen>
              </iframe>
          </Col>
        </Row>
        <div style={{marginBottom: 40, color: '#9E8475'}}>
          <Row style={{display: 'flex',justifyContent:'center', marginBottom: 40, marginTop: 60}}>
            <Col sm={8} style={{fontSize: 24, fontWeight: 'bolder', textAlign: 'center'}}>
            Firewatch is a mystery set in the Wyoming wilderness, 
            where your only emotional lifeline is the person on 
            the other end of a handheld radio.
            </Col>
          </Row>
          <Row style={{justifyContent:'center'}}>
            <Col sm={3} style={{fontSize: 17, fontWeight: 'bold', lineHeight: 2}}>
            The year is 1989.
  
            You are a man named Henry who has retreated from your messy life to work as a fire lookout in the Wyoming wilderness. 
            Perched atop a mountain, it's your job to find smoke and keep the wilderness safe.
  
            An especially hot, dry summer has everyone on edge. 
            Your supervisor, a woman named Delilah, is available to you
            </Col>
            <Col sm={3} style={{fontSize: 17, fontWeight: 'bold', lineHeight: 2}}>
            at all times over a small, handheld radio—and is your only 
            contact with the world you've left behind.
  
            But when something strange draws you out of your lookout tower and 
            into the world below, you'll explore a wild and unknown environment, 
            facing questions and making interpersonal choices that 
            can build or destroy the only meaningful relationship you have.
            </Col>
          </Row>
        </div>
        <div style={{paddingBottom: 50}}>
          <Row style={{ marginBottom: 30, }}>
            <Col style={{display: 'flex',justifyContent: 'flex-end'}}>
              <img src={image} width={450} height={225} />
            </Col>
            <Col>
            <img src={image} width={450} height={225}/>
            </Col>
          </Row>
          <Row>
            <Col style={{display: 'flex',justifyContent: 'flex-end'}}>
            <img src={image} width={450} height={225}/>
            </Col>
            <Col>
            <img src={image} width={450} height={225}/>
            </Col>
          </Row>
        </div>
    </Container> 
  );
}

export default App

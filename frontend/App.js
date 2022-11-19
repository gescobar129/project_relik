import 'regenerator-runtime/runtime';
import React, {useEffect} from 'react';
import './assets/global.css';
import image from './assets/firewatch.jpeg'

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
    <div>
      <Container>
        <Row style={{justifyContent: 'center', alignItems: 'center'}}>
          <Col style={{display: 'flex',justifyContent: 'center',alignItems: 'center'}}>
              <iframe 
                width="709" 
                height="399" 
                src="https://www.youtube.com/embed/cXWlgP5hZzc" 
                title="YouTube video player" 
                frameborder="0" 
                // allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" 
                allowfullscreen>
              </iframe>
  
          </Col>
        </Row>
        <div style={{marginBottom: 40}}>
          <Row style={{display: 'flex',justifyContent:'center', marginBottom: 20, marginTop: 60}}>
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
        <div >
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
    </div>
  );
}

export default App
import React, {useEffect, useState} from 'react'
import { 
  Container, 
  Card, 
  Row, 
  Col 
} from 'react-bootstrap'

import './assets/global.css';


const NftPage = ({ helloNEAR }) => {
  const [nftData, setNftData] = useState([])

  useEffect(() => {
    const fetchNftData = async () => {
      const nfts = await helloNEAR.getNfts()
      setNftData(nfts)
    }

    try {
      fetchNftData()
    } catch (error) {
      console.log('error', error)
    }
  }, [])

  console.log('nfts!', nftData)

  const nftList = () => {
    return nftData.map((nft) => {
      console.log('nft', nft)
      const item = nft.metadata
      const obj = JSON.parse(nft.metadata.extra)
      console.log('obj', obj)
      const stats = obj.stats

      const renderStats = () => {
        let arr = Object.keys(stats)
        // console.log('arr', arr)
        return arr.map((stat) => {
          return (
              <small className="text-muted" style={{fontSize: 10.5, fontWeight: 'bold'}}>{` ${stat} ${stats[stat]} | `}</small>
          )
        })
      }

      return (
        <Col style={{borderStyle: 'solid', display: 'flex', alignSelf: 'stretch', marginTop: 30}}>
          <Card>
            <Card.Header style={{
              fontSize: 12, 
              height: 30, 
              fontWeight: 700
              }}>
                {obj.type.toUpperCase()}
            </Card.Header>
            <Card.Img variant="top" style={{height: 200, width: 200, alignSelf: 'center', borderRadius: 12, backgroundColor: 'grey', marginTop: 10}} 
            src={item.media} />
            <Card.Body >
              <Card.Title style={{fontSize: 18, fontWeight: 700}}>{item.title}</Card.Title>
              <Card.Text style={{fontSize: 13, fontWeight: 500, }}>
                {item.description}
              </Card.Text>
              {renderStats()}
            </Card.Body>
          </Card>
        </Col>
      )
    })
  }
  return (
    <Container fluid style={{ height: '100vh', background: "#222222"}}>
        <h2 style={{color: '#FFFFFF', fontFamily: 'Fredoka One'}}>
          My NFTs
        </h2>
      <Row xs={1} md={4} style={{display: 'flex',justifyContent: 'center', alignItems: 'center'}}>
        {nftList()}
      </Row>

    </Container>
  )
}

export default NftPage
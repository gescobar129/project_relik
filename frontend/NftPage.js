import React, {useEffect, useState} from 'react'
import { 
  Container, 
  Card, 
  Row, 
  Col 
} from 'react-bootstrap'

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

  const nftList = () => {
    return nftData.map((nft) => {
      const item = nft.metadata
      const obj = JSON.parse(item.extra)
      const stats = obj.stats

      const renderStats = () => {
        let arr = Object.keys(stats)
        return arr.map((stat) => {
          return (
            <small 
              className="text-muted" 
              style={{
                fontSize: 10, 
                fontWeight: 'bold', 
                fontFamily: 'Courier Prime',
                }}>
              {` ${stat} ${stats[stat]} | `}
            </small>
          )
        })
      }

      return (
        <Col style={{ display: 'flex', alignSelf: 'stretch', marginTop: 30}}>
          <Card> 
            <Card.Header style={{
              fontSize: 12, 
              height: 30, 
              fontWeight: 700,
              fontFamily: 'Courier Prime'
              }}>
                {obj.type.toUpperCase()}
            </Card.Header>
            <Card.Img 
              variant="top" 
              style={{
                height: 150, 
                width: 150, 
                alignSelf: 'center', 
                borderRadius: 12, 
                marginTop: 10
              }} 
              src={item.media} />
            <Card.Body>
              <Card.Title style={{fontSize: 18, fontWeight: 700, fontFamily: 'Courier Prime'}}>
                {item.title}
              </Card.Title>
              <Card.Text style={{fontSize: 11, fontWeight: 500, fontFamily: 'Courier Prime'}}>
                {item.description}
              </Card.Text>
            </Card.Body>
              <Card.Footer style={{paddingTop: 0, backgroundColor: '#FFFFFF'}}>
                {renderStats()}
              </Card.Footer>
          </Card>
        </Col>
      )
    })
  }

  return (
    <Container fluid style={{background: "#A98975", height:'100vh'}}>
      <h2 style={{color: '#FFFFFF', fontFamily: 'Anton', paddingTop: 30}}>
        My NFTs
      </h2>
      <Row xs={1} md={5} style={{marginTop: 30}}>
        {nftList()}
      </Row>
    </Container>
  )
}

export default NftPage
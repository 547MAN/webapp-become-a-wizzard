import { Button, Col, Row } from 'react-bootstrap'
import PageHeader from '../components/PageHeader.jsx'

export default function DashboardPage({ go }) {
  return <><PageHeader eyebrow="ITPE3200 · SOFTWARE ENGINEERING" title="Quest map" />
    <section className="hero-panel"><small>NESTE OPPDRAG</small><h2>The Testing Caverns await.</h2><p>Test kunnskapen din, få umiddelbar feedback og bygg progresjon gjennom faget.</p><div className="d-flex gap-2"><Button onClick={() => go('quizzes')}>Finn en quiz</Button><Button variant="outline-light" onClick={() => go('editor')}>Lag egen quiz</Button></div></section>
    <h2 className="section-title">Din læringsreise</h2><Row className="g-3"><Col md={4}><div className="realm-card complete"><small>FULLFØRT</small><h3>Requirements Forest</h3><p>Brukerhistorier og funksjonelle krav</p><div className="realm-progress"><i style={{width:'100%'}} /></div></div></Col><Col md={4}><div className="realm-card"><small>PÅGÅR</small><h3>Testing Caverns</h3><p>Enhets-, integrasjons- og regresjonstesting</p><div className="realm-progress"><i style={{width:'68%'}} /></div></div></Col><Col md={4}><div className="realm-card locked"><small>LÅST</small><h3>Architecture Citadel</h3><p>Komponenter, mønstre og beslutninger</p><div className="realm-progress"><i style={{width:'8%'}} /></div></div></Col></Row>
  </>
}


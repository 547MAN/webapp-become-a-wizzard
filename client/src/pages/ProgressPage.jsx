import { useEffect, useState } from 'react'
import { Col, Row, Spinner } from 'react-bootstrap'
import { progressApi } from '../api/progressApi.js'
import PageHeader from '../components/PageHeader.jsx'

export default function ProgressPage(){const [p,setP]=useState(null);useEffect(()=>{progressApi.get().then(setP)},[]);if(!p)return <Spinner animation="border"/>;const stats=[['Total XP',p.totalXp],['Fullførte quizer',p.quizzesCompleted],['Bosser beseiret',p.bossesDefeated],['Nøyaktighet',`${p.accuracy}%`]];return <><PageHeader eyebrow="LÆRINGSSTATUS" title="Min progresjon"/><Row className="g-3">{stats.map(([l,v])=><Col md={3} key={l}><div className="stat-card"><small>{l}</small><strong>{v}</strong></div></Col>)}</Row><div className="progress-summary"><h2>Svarhistorikk</h2><p>Du har svart på <strong>{p.questionsAnswered}</strong> spørsmål. <strong>{p.correctAnswers}</strong> av svarene var riktige.</p><ProgressBar variant="success" now={p.accuracy}/></div></>}


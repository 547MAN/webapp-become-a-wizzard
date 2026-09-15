import { useEffect, useState } from 'react'
import { Alert, Col, Form, Row } from 'react-bootstrap'
import { quizApi } from '../api/quizApi.js'
import PageHeader from '../components/PageHeader.jsx'
import QuizCard from '../components/QuizCard.jsx'

// This page depends on quizApi.list() and sends the chosen quizId back to App.jsx through go().
export default function QuizListPage({ go }) {
  const [quizzes,setQuizzes]=useState([]); const [filter,setFilter]=useState(''); const [error,setError]=useState('')
  useEffect(()=>{quizApi.list().then(setQuizzes).catch(e=>setError(e.message))},[])
  const visible=quizzes.filter(q=>(q.title+q.topic).toLowerCase().includes(filter.toLowerCase()))
  return <><PageHeader eyebrow="PUBLISERTE UTFORDRINGER" title="Finn en quiz"/><Form.Control className="search-input mb-4" placeholder="Søk etter quiz eller tema" value={filter} onChange={e=>setFilter(e.target.value)}/>{error&&<Alert variant="danger">{error}</Alert>}<Row className="g-3">{visible.map(q=><Col lg={4} md={6} key={q.id}><QuizCard quiz={q} onPlay={()=>go('play',{quizId:q.id})}/></Col>)}</Row>{!error&&!visible.length&&<div className="empty-state">Ingen quizer samsvarer med søket.</div>}</>
}


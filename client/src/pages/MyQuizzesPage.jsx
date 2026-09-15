import { useEffect, useState } from 'react'
import { Alert, Button, Col, Row } from 'react-bootstrap'
import { quizApi } from '../api/quizApi.js'
import PageHeader from '../components/PageHeader.jsx'
import QuizCard from '../components/QuizCard.jsx'

// This page depends on QuizEditorPage for create/update and quizApi.remove() for delete.
export default function MyQuizzesPage({ go }) {
  const [quizzes,setQuizzes]=useState([]); const [error,setError]=useState('')
  const load=()=>quizApi.mine().then(setQuizzes).catch(e=>setError(e.message)); useEffect(load,[])
  const remove=async id=>{if(!confirm('Vil du slette denne quizen?'))return;try{await quizApi.remove(id);load()}catch(e){setError(e.message)}}
  return <><PageHeader eyebrow="CRUD" title="Mine quizer" action={<Button onClick={()=>go('editor')}>+ Ny quiz</Button>}/>{error&&<Alert variant="danger">{error}</Alert>}<Row className="g-3">{quizzes.map(q=><Col lg={4} md={6} key={q.id}><QuizCard quiz={q} onPlay={q.isPublished?()=>go('play',{quizId:q.id}):null} onEdit={()=>go('editor',{quizId:q.id})} onDelete={()=>remove(q.id)}/></Col>)}</Row>{!error&&!quizzes.length&&<div className="empty-state">Du har ikke laget noen quizer ennå.</div>}</>
}


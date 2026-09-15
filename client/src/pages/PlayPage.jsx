import { useEffect, useState } from 'react'
import { Alert, Badge, Button, ProgressBar, Spinner } from 'react-bootstrap'
import { gameApi } from '../api/gameApi.js'
import PageHeader from '../components/PageHeader.jsx'

// PlayPage depends on gameApi.js. All score, HP, correctness and boss calculations come from GameService.cs.
export default function PlayPage({ quizId, go }) {
  const [state,setState]=useState(null); const [feedback,setFeedback]=useState(null); const [error,setError]=useState(''); const [busy,setBusy]=useState(false)
  useEffect(()=>{gameApi.start(quizId).then(setState).catch(e=>setError(e.message))},[quizId])
  const answer=async optionId=>{setBusy(true);try{const result=await gameApi.answer(state.attemptId,state.question.id,optionId);setFeedback(result);setState(s=>({...s,...result,question:s.question}));setTimeout(async()=>{if(result.status==='Active'){setState(await gameApi.state(state.attemptId));setFeedback(null)}},1400)}catch(e){setError(e.message)}finally{setBusy(false)}}
  if(error)return <Alert variant="danger">{error}</Alert>; if(!state)return <Spinner animation="border"/>
  const finished=state.status!=='Active'
  return <><PageHeader eyebrow={state.isBossPhase?'BOSSKAMP':'QUIZKAMP'} title={state.quizTitle}/><div className="battle-stage"><div className="combatant wizard">🧙</div><div className="battle-center"><Badge bg={state.isBossPhase?'danger':'success'}>{state.isBossPhase?'Bossfase':'Vanlig runde'}</Badge>{state.isBossPhase&&<><small>Boss HP</small><ProgressBar variant="danger" now={Math.max(0,state.bossHp)} max={state.bossMaxHp} label={`${state.bossHp} HP`}/><div className="spell-orbs">{[0,1,2].map(i=><i key={i} className={i<state.correctStreak?'lit':''}/>)}</div><small>Tre riktige på rad utløser angrep</small></>}<small>Din HP</small><ProgressBar variant="success" now={state.playerHp} max={state.playerMaxHp} label={`${state.playerHp} HP`}/><strong>{state.score} poeng</strong></div><div className="combatant dragon">{state.isBossPhase?'🐉':'👾'}</div></div>
    {!finished&&<div className="question-panel"><small>VELG ETT SVAR</small><h2>{state.question.text}</h2><div className="answer-grid">{state.question.answerOptions.map(option=><Button disabled={busy||feedback} variant="outline-light" key={option.id} onClick={()=>answer(option.id)}>{option.text}</Button>)}</div>{feedback&&<Alert className="mt-3" variant={feedback.isCorrect?'success':'danger'}><strong>{feedback.isCorrect?'Riktig!':'Ikke riktig.'}</strong> {feedback.explanation}</Alert>}</div>}
    {finished&&<div className="result-panel"><span>{state.status==='Lost'?'☠':'✦'}</span><h2>{state.status==='Lost'?'Du tapte kampen':'Oppdrag fullført'}</h2><p>Sluttpoeng: {state.score}</p><Button onClick={()=>go('progress')}>Se progresjon</Button></div>}
  </>
}

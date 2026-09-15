import { useEffect, useState } from 'react'
import { Table } from 'react-bootstrap'
import { progressApi } from '../api/progressApi.js'
import PageHeader from '../components/PageHeader.jsx'

export default function HistoryPage(){const [rows,setRows]=useState([]);useEffect(()=>{progressApi.history().then(setRows)},[]);return <><PageHeader eyebrow="TIDLIGERE FORSØK" title="Historikk"/><Table responsive className="history-table"><thead><tr><th>Quiz</th><th>Status</th><th>Poeng</th><th>Startet</th></tr></thead><tbody>{rows.map(r=><tr key={r.attemptId}><td>{r.quizTitle}</td><td>{r.status}</td><td>{r.score}</td><td>{new Date(r.startedAt).toLocaleString('nb-NO')}</td></tr>)}</tbody></Table>{!rows.length&&<div className="empty-state">Ingen gjennomførte forsøk ennå.</div>}</>}


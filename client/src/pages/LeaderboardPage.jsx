import { useEffect, useState } from 'react'
import { Table } from 'react-bootstrap'
import { progressApi } from '../api/progressApi.js'
import PageHeader from '../components/PageHeader.jsx'

export default function LeaderboardPage(){const [rows,setRows]=useState([]);useEffect(()=>{progressApi.leaderboard().then(setRows)},[]);return <><PageHeader eyebrow="LOKAL RANGERING" title="Leaderboard"/><Table responsive className="history-table"><thead><tr><th>#</th><th>Bruker</th><th>Total XP</th></tr></thead><tbody>{rows.map((r,i)=><tr key={r.id}><td>{i+1}</td><td>{r.displayName}</td><td>{r.totalXp}</td></tr>)}</tbody></Table></>}


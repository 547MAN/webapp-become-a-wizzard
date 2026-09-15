import { Button, Nav } from 'react-bootstrap'
import { useAuth } from '../context/AuthContext.jsx'

// AppShell depends on AuthContext for the active user and on App.jsx for navigation through go().
export default function AppShell({ current, go, children }) {
  const { user, logout } = useAuth()
  const nav = [['dashboard','Quest map'],['quizzes','Finn quiz'],['mine','Mine quizer'],['progress','Progresjon'],['history','Historikk'],['leaderboard','Leaderboard']]
  return <div className="app-layout">
    <aside className="sidebar">
      <button className="brand-button" onClick={() => go('dashboard')}><span>✦</span> Become a Wizzard</button>
      <Nav className="flex-column gap-1">{nav.map(([id,label]) => <Nav.Link key={id} active={current === id} onClick={() => go(id)}>{label}</Nav.Link>)}</Nav>
      <div className="user-card"><div className="user-avatar">W</div><div><strong>{user.displayName}</strong><small>{user.totalXp} XP</small></div><Button variant="outline-light" size="sm" onClick={logout}>Ut</Button></div>
    </aside>
    <main className="content">{children}</main>
  </div>
}


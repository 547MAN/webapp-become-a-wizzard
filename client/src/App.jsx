import { useState } from 'react'
import { AuthProvider, useAuth } from './context/AuthContext.jsx'
import AuthPage from './pages/AuthPage.jsx'
import DashboardPage from './pages/DashboardPage.jsx'
import QuizListPage from './pages/QuizListPage.jsx'
import MyQuizzesPage from './pages/MyQuizzesPage.jsx'
import QuizEditorPage from './pages/QuizEditorPage.jsx'
import PlayPage from './pages/PlayPage.jsx'
import ProgressPage from './pages/ProgressPage.jsx'
import HistoryPage from './pages/HistoryPage.jsx'
import LeaderboardPage from './pages/LeaderboardPage.jsx'
import AppShell from './components/AppShell.jsx'

function Application() {
  const { user, loading } = useAuth()
  const [screen, setScreen] = useState({ name: 'dashboard' })
  if (loading) return <div className="loading-screen">Laster den magiske boken…</div>
  if (!user) return <AuthPage />

  const go = (name, data = {}) => setScreen({ name, ...data })
  const pages = {
    dashboard: <DashboardPage go={go} />,
    quizzes: <QuizListPage go={go} />,
    mine: <MyQuizzesPage go={go} />,
    editor: <QuizEditorPage quizId={screen.quizId} go={go} />,
    play: <PlayPage quizId={screen.quizId} go={go} />,
    progress: <ProgressPage />,
    history: <HistoryPage />,
    leaderboard: <LeaderboardPage />
  }
  return <AppShell current={screen.name} go={go}>{pages[screen.name] || pages.dashboard}</AppShell>
}

export default function App() { return <AuthProvider><Application /></AuthProvider> }


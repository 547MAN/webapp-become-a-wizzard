import { useState } from 'react'
import { Alert, Button, Form } from 'react-bootstrap'
import { useAuth } from '../context/AuthContext.jsx'

export default function AuthPage() {
  const { login, register } = useAuth(); const [mode,setMode] = useState('login'); const [error,setError] = useState('')
  const submit = async event => { event.preventDefault(); setError(''); const data = Object.fromEntries(new FormData(event.currentTarget)); try { mode === 'login' ? await login(data) : await register(data) } catch (e) { setError(e.message) } }
  return <div className="auth-page"><div className="auth-copy"><span className="rune">✦</span><small>ITPE3200 LEARNING PLATFORM</small><h1>Become a<br/><em>Wizzard</em></h1><p>Gjør kursinnhold til utfordringer, bygg egne quizer og beseir kunnskapens monstre.</p></div>
    <Form className="auth-form" onSubmit={submit}><h2>{mode === 'login' ? 'Logg inn' : 'Registrer bruker'}</h2><p>Alle brukere kan både spille og lage quizer.</p>{error && <Alert variant="danger">{error}</Alert>}
      {mode === 'register' && <Form.Group className="mb-3"><Form.Label>Navn</Form.Label><Form.Control name="displayName" required /></Form.Group>}
      <Form.Group className="mb-3"><Form.Label>E-post</Form.Label><Form.Control name="email" type="email" required defaultValue={mode === 'login' ? 'demo@wizard.local' : ''}/></Form.Group>
      <Form.Group className="mb-4"><Form.Label>Passord</Form.Label><Form.Control name="password" type="password" minLength="8" required defaultValue={mode === 'login' ? 'Wizard123!' : ''}/></Form.Group>
      <Button type="submit" className="w-100">{mode === 'login' ? 'Gå inn i akademiet' : 'Opprett bruker'}</Button>
      <button type="button" className="text-button" onClick={() => setMode(mode === 'login' ? 'register' : 'login')}>{mode === 'login' ? 'Ny bruker? Registrer deg' : 'Har du bruker? Logg inn'}</button>
    </Form></div>
}


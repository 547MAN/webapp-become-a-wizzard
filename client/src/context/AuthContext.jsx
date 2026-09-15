import { createContext, useContext, useEffect, useState } from 'react'
import { authApi } from '../api/authApi.js'

const AuthContext = createContext(null)

// AuthProvider depends on authApi.js and exposes one unified User to every page.
export function AuthProvider({ children }) {
  const [user, setUser] = useState(null)
  const [loading, setLoading] = useState(true)
  useEffect(() => { authApi.me().then(setUser).catch(() => setUser(null)).finally(() => setLoading(false)) }, [])
  const login = async values => setUser(await authApi.login(values))
  const register = async values => setUser(await authApi.register(values))
  const logout = async () => { await authApi.logout(); setUser(null) }
  return <AuthContext.Provider value={{ user, loading, login, register, logout }}>{children}</AuthContext.Provider>
}

export const useAuth = () => useContext(AuthContext)


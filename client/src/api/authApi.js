import { api } from './apiClient.js'

// These functions depend on apiClient.js for cookie-enabled HTTP requests.
export const authApi = {
  me: () => api('/auth/me'),
  login: payload => api('/auth/login', { method: 'POST', body: JSON.stringify(payload) }),
  register: payload => api('/auth/register', { method: 'POST', body: JSON.stringify(payload) }),
  logout: () => api('/auth/logout', { method: 'POST' })
}


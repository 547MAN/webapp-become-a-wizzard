import { api } from './apiClient.js'

// This module depends on apiClient.js and mirrors QuizzesController routes.
export const quizApi = {
  list: () => api('/quizzes'),
  mine: () => api('/quizzes/mine'),
  get: id => api(`/quizzes/${id}`),
  create: payload => api('/quizzes', { method: 'POST', body: JSON.stringify(payload) }),
  update: (id, payload) => api(`/quizzes/${id}`, { method: 'PUT', body: JSON.stringify(payload) }),
  remove: id => api(`/quizzes/${id}`, { method: 'DELETE' })
}


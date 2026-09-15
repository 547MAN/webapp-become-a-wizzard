import { api } from './apiClient.js'

// This module depends on GameController and never receives IsCorrect before submission.
export const gameApi = {
  start: quizId => api('/game/attempts', { method: 'POST', body: JSON.stringify({ quizId }) }),
  state: attemptId => api(`/game/attempts/${attemptId}`),
  answer: (attemptId, questionId, answerOptionId) => api(`/game/attempts/${attemptId}/answers`, {
    method: 'POST', body: JSON.stringify({ questionId, answerOptionId })
  })
}


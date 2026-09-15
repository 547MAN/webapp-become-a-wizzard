import { api } from './apiClient.js'

// These routes depend on ProgressController in the ASP.NET Core project.
export const progressApi = {
  get: () => api('/progress'),
  history: () => api('/progress/history'),
  leaderboard: () => api('/progress/leaderboard')
}


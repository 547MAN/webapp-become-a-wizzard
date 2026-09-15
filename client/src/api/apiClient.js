export async function api(path, options = {}) {
  const response = await fetch(`/api${path}`, {
    credentials: 'include',
    headers: { 'Content-Type': 'application/json', ...options.headers },
    ...options
  })
  if (response.status === 204) return null
  const data = await response.json().catch(() => null)
  if (!response.ok) throw new Error(data?.message || 'Noe gikk galt. Prøv igjen.')
  return data
}


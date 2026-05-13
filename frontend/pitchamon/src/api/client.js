import axios from 'axios'

export const api = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_URL || 'http://localhost:5090',
  headers: { 'Content-Type': 'application/json' },
})

export async function processSong(file, pokemonName, userId) {
  const form = new FormData()
  form.append('Song', file)
  form.append('PokemonName', pokemonName)
  if (userId != null) form.append('UserId', String(userId))

  const response = await api.post('/process', form, {
    headers: { 'Content-Type': undefined },
    responseType: 'blob',
  })
  return response.data
}

export async function getProcessingHistory(userId) {
  const response = await api.get('/process/history', {
    params: { UserId: userId },
  })
  return response.data?.history ?? []
}

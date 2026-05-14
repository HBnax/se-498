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
  return {
    audioBlob: response.data,
    lotrClass: {
      id: response.headers["lotr-class-id"],
      name: response.headers["lotr-class-name"],
      desc: response.headers["lotr-class-description"],
    },
    pokemonId: response.headers["pokemon-id"],
  }
}

export async function getProcessingHistory(userId) {
  const response = await api.get('/process/history', {
    params: { UserId: userId },
  })
  return response.data?.history ?? []
}

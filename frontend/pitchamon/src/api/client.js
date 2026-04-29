import axios from 'axios'

export const api = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_URL || 'http://localhost:5090',
  headers: { 'Content-Type': 'application/json' },
})

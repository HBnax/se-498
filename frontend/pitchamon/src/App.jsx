import { BrowserRouter, Routes, Route } from 'react-router-dom'
import { useEffect, useState } from 'react'
import HomePage from './pages/HomePage'
import ResultsPage from './pages/ResultsPage'
import HistoryPage from './pages/HistoryPage'

const STORAGE_KEY = 'pitchamon.user'

function loadStoredUser() {
  try {
    const raw = localStorage.getItem(STORAGE_KEY)
    return raw ? JSON.parse(raw) : null
  } catch {
    return null
  }
}

export default function App() {
  const [user, setUser] = useState(loadStoredUser)

  useEffect(() => {
    if (user) {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(user))
    } else {
      localStorage.removeItem(STORAGE_KEY)
    }
  }, [user])

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage user={user} setUser={setUser} />} />
        <Route path="/results" element={<ResultsPage />} />
        <Route path="/history" element={<HistoryPage user={user} />} />
      </Routes>
    </BrowserRouter>
  )
}

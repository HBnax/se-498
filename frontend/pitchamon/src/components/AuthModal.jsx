import { useState } from 'react'
import { api } from '../api/client'

export default function AuthModal({ mode, onClose, onSwitchMode, onAuthSuccess }) {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const isSignup = mode === 'signup'

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      const endpoint = isSignup ? '/auth/register' : '/auth/login'
      const { data } = await api.post(endpoint, { email, password })
      if (onAuthSuccess) onAuthSuccess(data.user)
      onClose()
    } catch (err) {
      setError(err.response?.data?.error || 'Something went wrong. Please try again.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <>
      <div
        className="modal fade show d-block"
        tabIndex="-1"
        role="dialog"
        onClick={onClose}
        style={{ zIndex: 1060 }}
      >
        <div
          className="modal-dialog modal-dialog-centered"
          role="document"
          onClick={(e) => e.stopPropagation()}
        >
          <div
            className="modal-content border-0 shadow-lg"
            style={{ backgroundColor: '#161B22', border: '1px solid #30363D' }}
          >
            <div className="modal-header border-0 pb-0">
              <h2
                className="modal-title w-100 text-center"
                style={{
                  fontFamily: 'var(--font-display, inherit)',
                  color: '#FFD700',
                  letterSpacing: '0.05em',
                }}
              >
                {isSignup ? 'SIGN UP' : 'LOG IN'}
              </h2>
              <button
                type="button"
                className="btn-close btn-close-white position-absolute"
                style={{ top: '1rem', right: '1rem' }}
                aria-label="Close"
                onClick={onClose}
              />
            </div>

            <div className="modal-body px-4 pb-4">
              <p
                className="text-center text-uppercase mb-4 small"
                style={{ color: '#8B949E', letterSpacing: '0.2em', fontFamily: 'monospace' }}
              >
                {isSignup ? 'Create a new trainer account' : 'Welcome back, trainer'}
              </p>

              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label
                    className="form-label text-uppercase small"
                    style={{ color: '#8B949E', letterSpacing: '0.2em', fontFamily: 'monospace' }}
                  >
                    Email
                  </label>
                  <input
                    type="email"
                    required
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    className="form-control"
                    style={{
                      backgroundColor: '#0D1117',
                      border: '1px solid #30363D',
                      color: '#F0F6FC',
                      fontFamily: 'monospace',
                    }}
                    placeholder="ash@pallet.town"
                  />
                </div>

                <div className="mb-4">
                  <label
                    className="form-label text-uppercase small"
                    style={{ color: '#8B949E', letterSpacing: '0.2em', fontFamily: 'monospace' }}
                  >
                    Password
                  </label>
                  <input
                    type="password"
                    required
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    className="form-control"
                    style={{
                      backgroundColor: '#0D1117',
                      border: '1px solid #30363D',
                      color: '#F0F6FC',
                      fontFamily: 'monospace',
                    }}
                    placeholder="••••••••"
                  />
                </div>

                {error && (
                  <div
                    className="alert mb-3 small"
                    role="alert"
                    style={{
                      backgroundColor: 'rgba(255, 60, 60, 0.1)',
                      border: '1px solid rgba(255, 60, 60, 0.4)',
                      color: '#FF3C3C',
                      fontFamily: 'monospace',
                    }}
                  >
                    {error}
                  </div>
                )}

                <button
                  type="submit"
                  disabled={loading}
                  className="btn w-100 text-uppercase py-2"
                  style={{
                    border: '2px solid #FFD700',
                    color: '#FFD700',
                    backgroundColor: 'transparent',
                    letterSpacing: '0.2em',
                    fontWeight: 600,
                    opacity: loading ? 0.6 : 1,
                  }}
                  onMouseOver={(e) => {
                    if (loading) return
                    e.currentTarget.style.backgroundColor = '#FFD700'
                    e.currentTarget.style.color = '#0D1117'
                  }}
                  onMouseOut={(e) => {
                    e.currentTarget.style.backgroundColor = 'transparent'
                    e.currentTarget.style.color = '#FFD700'
                  }}
                >
                  {loading ? 'Loading…' : isSignup ? 'Create Account' : 'Log In'}
                </button>
              </form>

              <p
                className="text-center mt-4 mb-0 small"
                style={{ color: '#8B949E', fontFamily: 'monospace' }}
              >
                {isSignup ? 'Already have an account?' : "Don't have an account?"}{' '}
                <button
                  type="button"
                  className="btn btn-link p-0 text-uppercase small align-baseline"
                  style={{ color: '#FF3C3C', letterSpacing: '0.2em', textDecoration: 'none' }}
                  onClick={() => {
                    setError('')
                    onSwitchMode(isSignup ? 'login' : 'signup')
                  }}
                >
                  {isSignup ? 'Log in' : 'Sign up'}
                </button>
              </p>
            </div>
          </div>
        </div>
      </div>
      <div
        className="modal-backdrop fade show"
        style={{ zIndex: 1050 }}
        onClick={onClose}
      />
    </>
  )
}

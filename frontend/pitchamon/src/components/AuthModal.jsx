import { useState } from 'react'

export default function AuthModal({ mode, onClose, onSwitchMode }) {
  const [username, setUsername] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')

  const isSignup = mode === 'signup'

  const handleSubmit = (e) => {
    e.preventDefault()
    const payload = isSignup ? { username, email, password } : { username, password }
    console.log(`${mode} submit`, payload)
    onClose()
  }

  return (
    <div
      onClick={onClose}
      className="fixed inset-0 z-50 flex items-center justify-center bg-black/70 backdrop-blur-sm px-4"
    >
      <div
        onClick={(e) => e.stopPropagation()}
        className="w-full max-w-md bg-[#161B22] border border-[#30363D] rounded-lg shadow-2xl p-8 relative"
      >
        <button
          onClick={onClose}
          className="absolute top-3 right-3 w-8 h-8 flex items-center justify-center text-[#8B949E] hover:text-[#FFD700] transition-colors font-mono"
          aria-label="Close"
        >
          ✕
        </button>

        <h2 className="font-display text-2xl text-[#FFD700] tracking-wide text-center mb-1">
          {isSignup ? 'SIGN UP' : 'LOG IN'}
        </h2>
        <p className="font-mono text-[#8B949E] text-xs text-center tracking-widest mb-6">
          {isSignup ? 'Create a new trainer account' : 'Welcome back, trainer'}
        </p>

        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <div className="flex flex-col gap-1">
            <label className="font-mono text-[#8B949E] text-xs tracking-widest uppercase">Username</label>
            <input
              type="text"
              required
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="h-11 px-3 bg-[#0D1117] border border-[#30363D] rounded-lg font-mono text-sm text-[#F0F6FC] placeholder-[#8B949E] outline-none focus:border-[#FFD700] transition-colors"
              placeholder="ashketchum"
            />
          </div>

          {isSignup && (
            <div className="flex flex-col gap-1">
              <label className="font-mono text-[#8B949E] text-xs tracking-widest uppercase">Email</label>
              <input
                type="email"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="h-11 px-3 bg-[#0D1117] border border-[#30363D] rounded-lg font-mono text-sm text-[#F0F6FC] placeholder-[#8B949E] outline-none focus:border-[#FFD700] transition-colors"
                placeholder="ash@pallet.town"
              />
            </div>
          )}

          <div className="flex flex-col gap-1">
            <label className="font-mono text-[#8B949E] text-xs tracking-widest uppercase">Password</label>
            <input
              type="password"
              required
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="h-11 px-3 bg-[#0D1117] border border-[#30363D] rounded-lg font-mono text-sm text-[#F0F6FC] placeholder-[#8B949E] outline-none focus:border-[#FFD700] transition-colors"
              placeholder="••••••••"
            />
          </div>

          <button
            type="submit"
            className="mt-2 font-display tracking-widest text-base px-8 py-3 rounded-lg border-2 border-[#FFD700] text-[#FFD700] hover:bg-[#FFD700] hover:text-[#0D1117] transition-all duration-200 active:scale-95"
          >
            {isSignup ? 'CREATE ACCOUNT' : 'LOG IN'}
          </button>
        </form>

        <p className="font-mono text-[#8B949E] text-xs text-center mt-6">
          {isSignup ? 'Already have an account?' : "Don't have an account?"}{' '}
          <button
            onClick={() => onSwitchMode(isSignup ? 'login' : 'signup')}
            className="text-[#FF3C3C] hover:text-[#FFD700] transition-colors uppercase tracking-widest"
          >
            {isSignup ? 'Log in' : 'Sign up'}
          </button>
        </p>
      </div>
    </div>
  )
}

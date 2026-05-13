import { useState, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
import AuthModal from '../components/AuthModal'
import { processSong } from '../api/client'

const POKEMON = [
  'Bulbasaur','Ivysaur','Venusaur','Charmander','Charmeleon','Charizard',
  'Squirtle','Wartortle','Blastoise','Caterpie','Metapod','Butterfree',
  'Weedle','Kakuna','Beedrill','Pidgey','Pidgeotto','Pidgeot','Rattata',
  'Raticate','Spearow','Fearow','Ekans','Arbok','Pikachu','Raichu',
  'Sandshrew','Sandslash','Nidoran♀','Nidorina','Nidoqueen','Nidoran♂',
  'Nidorino','Nidoking','Clefairy','Clefable','Vulpix','Ninetales',
  'Jigglypuff','Wigglytuff','Zubat','Golbat','Oddish','Gloom','Vileplume',
  'Paras','Parasect','Venonat','Venomoth','Diglett','Dugtrio','Meowth',
  'Persian','Psyduck','Golduck','Mankey','Primeape','Growlithe','Arcanine',
  'Poliwag','Poliwhirl','Poliwrath','Abra','Kadabra','Alakazam','Machop',
  'Machoke','Machamp','Bellsprout','Weepinbell','Victreebel','Tentacool',
  'Tentacruel','Geodude','Graveler','Golem','Ponyta','Rapidash','Slowpoke',
  'Slowbro','Magnemite','Magneton','Farfetchd','Doduo','Dodrio','Seel',
  'Dewgong','Grimer','Muk','Shellder','Cloyster','Gastly','Haunter','Gengar',
  'Onix','Drowzee','Hypno','Krabby','Kingler','Voltorb','Electrode',
  'Exeggcute','Exeggutor','Cubone','Marowak','Hitmonlee','Hitmonchan',
  'Lickitung','Koffing','Weezing','Rhyhorn','Rhydon','Chansey','Tangela',
  'Kangaskhan','Horsea','Seadra','Goldeen','Seaking','Staryu','Starmie',
  'Mr. Mime','Scyther','Jynx','Electabuzz','Magmar','Pinsir','Tauros',
  'Magikarp','Gyarados','Lapras','Ditto','Eevee','Vaporeon','Jolteon',
  'Flareon','Porygon','Omanyte','Omastar','Kabuto','Kabutops','Aerodactyl',
  'Snorlax','Articuno','Zapdos','Moltres','Dratini','Dragonair','Dragonite','Mewtwo','Mew'
]

function spriteUrl(name) {
  const id = POKEMON.indexOf(name) + 1
  return `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`
}

function cryUrl(name) {
  const id = POKEMON.indexOf(name) + 1
  return `https://raw.githubusercontent.com/PokeAPI/cries/main/cries/pokemon/latest/${id}.ogg`
}

export default function HomePage({ user, setUser }) {
  const navigate = useNavigate()
  const fileInputRef = useRef(null)
  const audioRef = useRef(null)
  const [mp3File, setMp3File] = useState(null)
  const [selectedPokemon, setSelectedPokemon] = useState('')
  const [search, setSearch] = useState('')
  const [dropdownOpen, setDropdownOpen] = useState(false)
  const [dragOver, setDragOver] = useState(false)
  const [playingPokemon, setPlayingPokemon] = useState('')
  const [authMode, setAuthMode] = useState(null)
  const [processing, setProcessing] = useState(false)
  const [processError, setProcessError] = useState('')

  const filtered = POKEMON.filter(p =>
    p.toLowerCase().includes(search.toLowerCase())
  )

  const handleFile = (file) => {
    if (file && file.type === 'audio/mpeg') {
      setMp3File(file)
    } else {
      alert('Please upload an MP3 file.')
    }
  }

  const handleDrop = (e) => {
    e.preventDefault()
    setDragOver(false)
    handleFile(e.dataTransfer.files[0])
  }

  const handlePlay = (e, pokemon) => {
    e.stopPropagation()
    if (audioRef.current) {
      audioRef.current.pause()
    }
    if (playingPokemon === pokemon) {
      setPlayingPokemon('')
      return
    }
    const audio = new Audio(cryUrl(pokemon))
    audioRef.current = audio
    audio.play()
    setPlayingPokemon(pokemon)
    audio.onended = () => setPlayingPokemon('')
  }

  const handleTranspose = async () => {
    setProcessError('')
    setProcessing(true)
    try {
      const blob = await processSong(mp3File, selectedPokemon, user?.id)
      const audioUrl = URL.createObjectURL(blob)
      navigate('/results', {
        state: { fileName: mp3File.name, pokemon: selectedPokemon, audioUrl }
      })
    } catch (err) {
      const message =
        err.response?.data?.error ||
        (err.response?.status ? `Processing failed (${err.response.status})` : err.message) ||
        'Processing failed'
      setProcessError(message)
    } finally {
      setProcessing(false)
    }
  }

  const canTranspose = mp3File && selectedPokemon && !processing

  return (
    <div className="min-vh-100 pm-bg position-relative px-3 py-5 d-flex flex-column align-items-center">
      <div className="position-absolute top-0 end-0 d-flex align-items-center gap-2 p-4">
        {user ? (
          <>
            <span
              className="font-mono small pm-tracking-wide text-uppercase"
              style={{ color: 'var(--pm-gold)' }}
            >
              {user.email}
            </span>
            <button
              type="button"
              className="btn btn-sm pm-btn-outline"
              onClick={() => navigate('/history')}
            >
              History
            </button>
            <button
              type="button"
              className="btn btn-sm pm-btn-outline"
              onClick={() => setUser(null)}
            >
              Log Out
            </button>
          </>
        ) : (
          <>
            <button
              type="button"
              className="btn btn-sm pm-btn-gold"
              onClick={() => setAuthMode('login')}
            >
              Log In
            </button>
            <button
              type="button"
              className="btn btn-sm pm-btn-gold"
              onClick={() => setAuthMode('signup')}
            >
              Sign Up
            </button>
          </>
        )}
      </div>

      <div className="text-center mb-5">
        <h1 className="font-display pm-gold display-5 mb-2">PITCHAMON</h1>
        <p className="font-mono pm-muted small pm-tracking-wide mb-0">"Pitch 'em All!"</p>
      </div>

      <div className="container" style={{ maxWidth: '760px' }}>
        <div className="row g-4 mb-4">
          <div className="col-12 col-md-6">
            <label className="form-label font-mono pm-muted small pm-tracking-wide text-uppercase">
              Add MP3
            </label>
            <div
              onClick={() => fileInputRef.current.click()}
              onDragOver={(e) => { e.preventDefault(); setDragOver(true) }}
              onDragLeave={() => setDragOver(false)}
              onDrop={handleDrop}
              className={`pm-dropzone d-flex flex-column align-items-center justify-content-center ${dragOver ? 'is-active' : ''} ${mp3File ? 'has-file' : ''}`}
            >
              {mp3File ? (
                <>
                  <span className="mb-2" style={{ fontSize: '1.75rem' }}>🎵</span>
                  <span className="font-mono pm-gold small text-break text-center px-3">{mp3File.name}</span>
                  <span className="font-mono pm-muted mt-1" style={{ fontSize: '0.75rem' }}>Click to change</span>
                </>
              ) : (
                <>
                  <span className="mb-3" style={{ fontSize: '2rem', color: 'var(--pm-border)' }}>+</span>
                  <span className="font-mono pm-muted small">Click or drag an MP3 here</span>
                </>
              )}
            </div>
            <input
              ref={fileInputRef}
              type="file"
              accept=".mp3,audio/mpeg"
              className="d-none"
              onChange={(e) => handleFile(e.target.files[0])}
            />
          </div>

          <div className="col-12 col-md-6">
            <label className="form-label font-mono pm-muted small pm-tracking-wide text-uppercase">
              Add Pokémon
            </label>
            <div className="position-relative">
              <button
                type="button"
                onClick={() => setDropdownOpen(!dropdownOpen)}
                className={`btn w-100 d-flex align-items-center justify-content-between pm-btn-outline ${selectedPokemon ? '' : 'pm-muted'}`}
                style={{
                  height: '3rem',
                  borderColor: selectedPokemon ? 'var(--pm-red)' : 'var(--pm-border)',
                  backgroundColor: 'var(--pm-surface)',
                  color: selectedPokemon ? 'var(--pm-text)' : 'var(--pm-muted)',
                  fontFamily: 'var(--font-mono)',
                }}
              >
                {selectedPokemon ? (
                  <span className="d-flex align-items-center gap-2">
                    <img src={spriteUrl(selectedPokemon)} alt={selectedPokemon} className="pm-pixel-img pm-sprite-xs" />
                    <span>{selectedPokemon}</span>
                  </span>
                ) : (
                  <span>Select...</span>
                )}
                <span style={{ transform: dropdownOpen ? 'rotate(180deg)' : 'none', transition: 'transform 0.2s' }}>▾</span>
              </button>

              {dropdownOpen && (
                <div className="pm-dropdown">
                  <div className="p-2 border-bottom" style={{ borderColor: 'var(--pm-border)' }}>
                    <div className="d-flex align-items-center gap-2 px-3 py-2 rounded" style={{ backgroundColor: 'var(--pm-bg)' }}>
                      <span className="pm-muted small">🔍</span>
                      <input
                        autoFocus
                        type="text"
                        placeholder="Search..."
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                        className="form-control form-control-sm border-0 bg-transparent pm-text font-mono p-0"
                      />
                    </div>
                  </div>
                  <ul className="pm-dropdown-list">
                    {filtered.length === 0 && (
                      <li className="px-3 py-2 font-mono small pm-muted">No Pokémon found</li>
                    )}
                    {filtered.map((p) => (
                      <li
                        key={p}
                        onClick={() => { setSelectedPokemon(p); setDropdownOpen(false); setSearch('') }}
                        className={`pm-dropdown-item ${selectedPokemon === p ? 'is-selected' : ''}`}
                      >
                        <img src={spriteUrl(p)} alt={p} className="pm-pixel-img pm-sprite-sm flex-shrink-0" />
                        <span className="flex-grow-1">{p}</span>
                        <button
                          type="button"
                          onClick={(e) => handlePlay(e, p)}
                          className={`pm-play-btn ${playingPokemon === p ? 'is-playing' : ''}`}
                        >
                          {playingPokemon === p ? '■' : '▶'}
                        </button>
                      </li>
                    ))}
                  </ul>
                </div>
              )}
            </div>

            {selectedPokemon && (
              <div
                className="pm-card d-flex align-items-center gap-3 px-3 py-2 mt-2"
                style={{ borderColor: 'rgba(255, 60, 60, 0.3)' }}
              >
                <img src={spriteUrl(selectedPokemon)} alt={selectedPokemon} className="pm-pixel-img pm-sprite-md" />
                <span className="font-mono pm-text small flex-grow-1">{selectedPokemon}</span>
                <button
                  type="button"
                  onClick={(e) => handlePlay(e, selectedPokemon)}
                  className={`pm-play-btn ${playingPokemon === selectedPokemon ? 'is-playing' : ''}`}
                  style={{ borderColor: playingPokemon === selectedPokemon ? 'var(--pm-gold)' : 'var(--pm-red)', color: playingPokemon === selectedPokemon ? 'var(--pm-bg)' : 'var(--pm-red)' }}
                >
                  {playingPokemon === selectedPokemon ? '■' : '▶'}
                </button>
              </div>
            )}
          </div>
        </div>

        <div className="d-flex flex-column align-items-center">
          <button
            type="button"
            onClick={handleTranspose}
            disabled={!canTranspose}
            className="btn btn-lg pm-btn-gold px-5 py-3"
          >
            {processing ? 'PROCESSING…' : 'TRANSPOSE'}
          </button>

          {processError && (
            <p className="font-mono small mt-3 mb-0" style={{ color: 'var(--pm-red)' }}>
              {processError}
            </p>
          )}

          {!processing && !processError && (!mp3File || !selectedPokemon) && (
            <p className="font-mono pm-muted small mt-3 mb-0">
              {!mp3File && !selectedPokemon
                ? 'Upload an MP3 and select a Pokémon to begin'
                : !mp3File
                  ? 'Upload an MP3 to continue'
                  : 'Select a Pokémon to continue'}
            </p>
          )}
        </div>
      </div>

      {authMode && (
        <AuthModal
          mode={authMode}
          onClose={() => setAuthMode(null)}
          onSwitchMode={setAuthMode}
          onAuthSuccess={(authedUser) => setUser(authedUser)}
        />
      )}
    </div>
  )
}

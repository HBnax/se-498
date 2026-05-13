import { useLocation, useNavigate } from 'react-router-dom'
import { useEffect, useState } from 'react'
import AuthModal from '../components/AuthModal'

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
  'Slowbro','Magnemite','Magneton','Farfetch\'d','Doduo','Dodrio','Seel',
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

export default function ResultsPage({ user, setUser }) {
  const location = useLocation()
  const navigate = useNavigate()
  const { fileName, pokemon, audioUrl } = location.state || {}
  const [bounce, setBounce] = useState(false)
  const [authMode, setAuthMode] = useState(null)

  const pokemonIndex = POKEMON.indexOf(pokemon) + 1
  const spriteUrl = `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${pokemonIndex}.png`
  const outputName = pokemon
    ? `${pokemon.toLowerCase()}_${fileName?.replace(/\.(mp3|wav)$/i, '') || 'output'}.wav`
    : 'output.wav'

  useEffect(() => {
    const interval = setInterval(() => setBounce(b => !b), 400)
    return () => clearInterval(interval)
  }, [])

  if (!pokemon || !fileName) {
    navigate('/')
    return null
  }

  return (
    <div className="min-vh-100 pm-bg position-relative d-flex flex-column align-items-center justify-content-center px-3 py-5">
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

      <h1 className="font-display pm-gold text-center mb-5" style={{ fontSize: 'clamp(1.25rem, 3vw, 1.875rem)' }}>
        PITCHAMON COMPLETE!
      </h1>

      <div className="container" style={{ maxWidth: '640px' }}>
        <div className="row g-4 align-items-center">
          <div className="col-12 col-md-6">
            <div className="d-flex flex-column gap-3">
              <p className="font-mono pm-text mb-0">{outputName}</p>

              {audioUrl && (
                <audio controls src={audioUrl} className="w-100" />
              )}

              <a
                href={audioUrl || '#'}
                download={outputName}
                onClick={(e) => { if (!audioUrl) e.preventDefault() }}
                className="btn pm-btn-gold py-2 text-decoration-none"
                style={!audioUrl ? { opacity: 0.5, pointerEvents: 'none' } : undefined}
              >
                DOWNLOAD
              </a>

              <button
                type="button"
                onClick={() => navigate('/')}
                className="btn pm-btn-outline py-2"
              >
                New Pitchamon
              </button>
            </div>
          </div>

          <div className="col-12 col-md-6 d-flex flex-column align-items-center justify-content-center gap-3">
            <div
              style={{
                transform: bounce ? 'translateY(-8px) rotate(-4deg)' : 'translateY(0px) rotate(4deg)',
                transition: 'transform 0.4s ease-in-out',
              }}
            >
              <img
                src={spriteUrl}
                alt={pokemon}
                className="pm-pixel-img"
                style={{ width: '120px', height: '120px' }}
              />
            </div>
            <span className="font-mono pm-muted pm-tracking-wide" style={{ fontSize: '0.75rem' }}>
              {pokemon?.toUpperCase()}
            </span>
          </div>
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

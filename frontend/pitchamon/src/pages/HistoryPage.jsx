import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { getProcessingHistory } from '../api/client'

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
  if (id <= 0) return null
  return `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`
}

function formatDate(value) {
  if (!value) return ''
  const d = new Date(value)
  if (Number.isNaN(d.getTime())) return value
  return d.toLocaleString([], {
    year: 'numeric',
    month: 'numeric',
    day: 'numeric',
    hour: 'numeric',
    minute: '2-digit',
  })
}

export default function HistoryPage({ user }) {
  const navigate = useNavigate()
  const [entries, setEntries] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  useEffect(() => {
    if (!user) {
      navigate('/')
      return
    }
    let cancelled = false
    setLoading(true)
    setError('')
    getProcessingHistory(user.id)
      .then((data) => {
        if (!cancelled) setEntries(data)
      })
      .catch((err) => {
        if (cancelled) return
        const message =
          err.response?.data?.error ||
          (err.response?.status ? `Failed to load history (${err.response.status})` : err.message) ||
          'Failed to load history'
        setError(message)
      })
      .finally(() => {
        if (!cancelled) setLoading(false)
      })
    return () => { cancelled = true }
  }, [user, navigate])

  if (!user) return null

  return (
    <div className="min-vh-100 pm-bg position-relative px-3 py-5 d-flex flex-column align-items-center">
      <div className="position-absolute top-0 end-0 d-flex align-items-center gap-2 p-4">
        <button
          type="button"
          className="btn btn-sm pm-btn-outline"
          onClick={() => navigate('/')}
        >
          ← Back
        </button>
      </div>

      <div className="text-center mb-5">
        <h1 className="font-display pm-gold display-6 mb-2">HISTORY</h1>
        <p className="font-mono pm-muted small pm-tracking-wide mb-0">
          Your previously pitched songs
        </p>
      </div>

      <div className="container" style={{ maxWidth: '880px' }}>
        {loading && (
          <p className="font-mono pm-muted small text-center mb-0">Loading…</p>
        )}

        {error && !loading && (
          <p className="font-mono small text-center mb-0" style={{ color: 'var(--pm-red)' }}>
            {error}
          </p>
        )}

        {!loading && !error && entries.length === 0 && (
          <div className="pm-card p-4 text-center">
            <p className="font-mono pm-muted small mb-3">
              You haven't transposed anything yet.
            </p>
            <button
              type="button"
              className="btn pm-btn-gold px-4 py-2"
              onClick={() => navigate('/')}
            >
              Start Pitching
            </button>
          </div>
        )}

        {!loading && !error && entries.length > 0 && (
          <div className="d-flex flex-column gap-3">
            {entries.map((entry) => {
              const sprite = spriteUrl(entry.pokemonUsed)
              return (
                <div key={entry.id} className="pm-card p-3">
                  <div className="row g-3 align-items-center">
                    <div className="col-12 col-md-4 d-flex align-items-center gap-3">
                      {sprite && (
                        <img
                          src={sprite}
                          alt={entry.pokemonUsed}
                          className="pm-pixel-img"
                          style={{ width: '56px', height: '56px' }}
                        />
                      )}
                      <div className="d-flex flex-column">
                        <span className="font-mono pm-muted pm-tracking-wide text-uppercase" style={{ fontSize: '0.65rem' }}>
                          Pokémon
                        </span>
                        <span className="font-mono pm-gold small">{entry.pokemonUsed}</span>
                      </div>
                    </div>

                    <div className="col-12 col-md-5">
                      <span className="font-mono pm-muted pm-tracking-wide text-uppercase d-block mb-1" style={{ fontSize: '0.65rem' }}>
                        Original Song
                      </span>
                      <span className="font-mono pm-text small text-break">
                        {entry.originalSongFile}
                      </span>
                    </div>

                    <div className="col-12 col-md-3 text-md-end">
                      <span className="font-mono pm-muted" style={{ fontSize: '0.7rem' }}>
                        {formatDate(entry.createdAt)}
                      </span>
                    </div>
                  </div>
                </div>
              )
            })}
          </div>
        )}
      </div>
    </div>
  )
}

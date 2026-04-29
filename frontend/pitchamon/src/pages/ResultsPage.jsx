import { useLocation, useNavigate } from 'react-router-dom'
import { useEffect, useState } from 'react'

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

export default function ResultsPage() {
  const location = useLocation()
  const navigate = useNavigate()
  const { fileName, pokemon } = location.state || {}
  const [bounce, setBounce] = useState(false)

  const pokemonIndex = POKEMON.indexOf(pokemon) + 1
  const spriteUrl = `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${pokemonIndex}.png`
  const outputName = pokemon
    ? `${pokemon.toLowerCase()}_${fileName?.replace('.mp3', '') || 'output'}.mp3`
    : 'output.mp3'

  useEffect(() => {
    const interval = setInterval(() => setBounce(b => !b), 400)
    return () => clearInterval(interval)
  }, [])

  if (!pokemon || !fileName) {
    navigate('/')
    return null
  }

  return (
    <div className="min-vh-100 pm-bg d-flex flex-column align-items-center justify-content-center px-3 py-5">
      <h1 className="font-display pm-gold text-center mb-5" style={{ fontSize: 'clamp(1.25rem, 3vw, 1.875rem)' }}>
        PITCHAMON COMPLETE!
      </h1>

      <div className="container" style={{ maxWidth: '640px' }}>
        <div className="row g-4 align-items-center">
          <div className="col-12 col-md-6">
            <div className="d-flex flex-column gap-3">
              <p className="font-mono pm-text mb-0">{outputName}</p>

              <div className="pm-card d-flex align-items-center gap-3 px-3 py-2 rounded-pill">
                <button
                  type="button"
                  className="pm-play-btn"
                  style={{ borderColor: 'var(--pm-gold)', color: 'var(--pm-gold)', width: '2rem', height: '2rem' }}
                >
                  ▶
                </button>
                <div className="progress flex-grow-1" style={{ height: '4px', backgroundColor: 'var(--pm-border)' }}>
                  <div
                    className="progress-bar"
                    role="progressbar"
                    style={{ width: '33%', backgroundColor: 'var(--pm-gold)' }}
                    aria-valuenow="33"
                    aria-valuemin="0"
                    aria-valuemax="100"
                  />
                </div>
                <span className="font-mono pm-muted flex-shrink-0" style={{ fontSize: '0.75rem' }}>0:00</span>
              </div>

              <a
                href="#"
                onClick={(e) => e.preventDefault()}
                className="btn pm-btn-gold py-2 text-decoration-none"
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
    </div>
  )
}

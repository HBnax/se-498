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
    <div className="min-h-screen bg-[#0D1117] flex flex-col items-center justify-center px-6 py-12">

      <h1 className="font-display text-2xl md:text-3xl text-[#FFD700] tracking-wide mb-10 text-center">
        PITCHAMON COMPLETE!
      </h1>

      <div className="w-full max-w-2xl grid grid-cols-1 md:grid-cols-2 gap-8 items-center">

        {/* Left: result info */}
        <div className="flex flex-col gap-4">
          <p className="font-mono text-[#F0F6FC] text-base">{outputName}</p>

          {/* Placeholder audio player */}
          <div className="flex items-center gap-3 bg-[#161B22] border border-[#30363D] rounded-full px-4 py-3">
            <button className="w-8 h-8 rounded-full border border-[#FFD700] flex items-center justify-center text-[#FFD700] hover:bg-[#FFD700] hover:text-[#0D1117] transition-all flex-shrink-0">
              ▶
            </button>
            <div className="flex-1 h-1 bg-[#30363D] rounded-full overflow-hidden">
              <div className="h-full w-1/3 bg-[#FFD700] rounded-full" />
            </div>
            <span className="font-mono text-[#8B949E] text-xs flex-shrink-0">0:00</span>
          </div>

          {/* Download */}
          <a
            href="#"
            onClick={(e) => e.preventDefault()}
            className="flex items-center justify-center gap-2 border-2 border-[#FFD700] text-[#FFD700] font-display tracking-widest text-sm rounded-lg px-6 py-3 hover:bg-[#FFD700] hover:text-[#0D1117] transition-all active:scale-95"
          >
            DOWNLOAD
          </a>

          {/* New Pitchamon */}
          <button
            onClick={() => navigate('/')}
            className="flex items-center justify-center gap-2 border border-[#30363D] text-[#8B949E] font-mono text-sm rounded-lg px-6 py-3 hover:border-[#8B949E] hover:text-[#F0F6FC] transition-all active:scale-95"
          >
            New Pitchamon
          </button>
        </div>

        {/* Right: dancing sprite */}
        <div className="flex flex-col items-center justify-center gap-4">
          <div
            style={{
              transform: bounce ? 'translateY(-8px) rotate(-4deg)' : 'translateY(0px) rotate(4deg)',
              transition: 'transform 0.4s ease-in-out',
            }}
          >
            <img
              src={spriteUrl}
              alt={pokemon}
              style={{ imageRendering: 'pixelated', width: 120, height: 120 }}
            />
          </div>
          <span className="font-mono text-[#8B949E] text-xs tracking-widest">{pokemon?.toUpperCase()}</span>
        </div>

      </div>
    </div>
  )
}
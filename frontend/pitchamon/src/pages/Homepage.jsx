import { useState, useRef } from 'react'
import { useNavigate } from 'react-router-dom'
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

export default function HomePage() {
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

  const handleTranspose = () => {
    navigate('/results', {
      state: { fileName: mp3File.name, pokemon: selectedPokemon }
    })
  }

  const canTranspose = mp3File && selectedPokemon

  return (
    <div className="min-vh-100 pm-bg position-relative px-3 py-5 d-flex flex-column align-items-center">
      <div className="position-absolute top-0 end-0 d-flex gap-2 p-4">
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
      </div>

      <div className="text-center mb-5">
        <h1 className="font-display pm-gold display-5 mb-2">PITCHAMON</h1>
        <p className="font-mono pm-muted small pm-tracking-wide mb-0">"Pitch 'em All!"</p>
      </div>

      <div className="w-full max-w-3xl grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">

        <div className="flex flex-col gap-2">
          <label className="font-mono text-[#8B949E] text-xs tracking-widest uppercase">Add MP3</label>
          <div
            onClick={() => fileInputRef.current.click()}
            onDragOver={(e) => { e.preventDefault(); setDragOver(true) }}
            onDragLeave={() => setDragOver(false)}
            onDrop={handleDrop}
            className={`flex flex-col items-center justify-center border-2 border-dashed rounded-lg h-48 cursor-pointer transition-all ${dragOver ? 'border-[#FFD700] bg-[#FFD700]/10' : mp3File ? 'border-[#FFD700] bg-[#161B22]' : 'border-[#30363D] bg-[#161B22] hover:border-[#FFD700]/50'}`}
          >
            {mp3File ? (
              <>
                <span className="text-3xl mb-2">🎵</span>
                <span className="font-mono text-[#FFD700] text-sm text-center px-4 break-all">{mp3File.name}</span>
                <span className="font-mono text-[#8B949E] text-xs mt-1">Click to change</span>
              </>
            ) : (
              <>
                <span className="text-4xl mb-3 text-[#30363D]">+</span>
                <span className="font-mono text-[#8B949E] text-sm">Click or drag an MP3 here</span>
              </>
            )}
          </div>
          <input ref={fileInputRef} type="file" accept=".mp3,audio/mpeg" className="hidden" onChange={(e) => handleFile(e.target.files[0])} />
        </div>

        <div className="flex flex-col gap-2">
          <label className="font-mono text-[#8B949E] text-xs tracking-widest uppercase">Add Pokémon</label>
          <div className="relative">
            <button
              onClick={() => setDropdownOpen(!dropdownOpen)}
              className={`w-full h-12 px-4 flex items-center justify-between border rounded-lg font-mono text-sm transition-all ${selectedPokemon ? 'border-[#FF3C3C] text-[#F0F6FC] bg-[#161B22]' : 'border-[#30363D] text-[#8B949E] bg-[#161B22] hover:border-[#FFD700]/50'}`}
            >
              {selectedPokemon ? (
                <div className="flex items-center gap-2">
                  <img src={spriteUrl(selectedPokemon)} alt={selectedPokemon} className="w-7 h-7" style={{ imageRendering: 'pixelated' }} />
                  <span>{selectedPokemon}</span>
                </div>
              ) : <span>Select...</span>}
              <span className={`transition-transform ${dropdownOpen ? 'rotate-180' : ''}`}>▾</span>
            </button>

            {dropdownOpen && (
              <div className="absolute z-10 w-full mt-1 bg-[#161B22] border border-[#30363D] rounded-lg overflow-hidden shadow-xl">
                <div className="p-2 border-b border-[#30363D]">
                  <div className="flex items-center gap-2 bg-[#0D1117] rounded px-3 py-2">
                    <span className="text-[#8B949E] text-xs">🔍</span>
                    <input autoFocus type="text" placeholder="Search..." value={search} onChange={(e) => setSearch(e.target.value)} className="bg-transparent font-mono text-sm text-[#F0F6FC] outline-none w-full placeholder-[#8B949E]" />
                  </div>
                </div>
                <ul className="max-h-64 overflow-y-auto">
                  {filtered.length === 0 && <li className="px-4 py-3 font-mono text-sm text-[#8B949E]">No Pokémon found</li>}
                  {filtered.map((p) => (
                    <li
                      key={p}
                      onClick={() => { setSelectedPokemon(p); setDropdownOpen(false); setSearch('') }}
                      className={`flex items-center gap-3 px-3 py-2 cursor-pointer transition-colors ${selectedPokemon === p ? 'bg-[#FF3C3C]/20 text-[#FF3C3C]' : 'text-[#F0F6FC] hover:bg-[#FFD700]/10 hover:text-[#FFD700]'}`}
                    >
                      <img src={spriteUrl(p)} alt={p} className="w-8 h-8 flex-shrink-0" style={{ imageRendering: 'pixelated' }} />
                      <span className="font-mono text-sm flex-1">{p}</span>
                      <button
                        onClick={(e) => handlePlay(e, p)}
                        className={`w-7 h-7 rounded-full flex items-center justify-center border transition-all flex-shrink-0 text-xs ${playingPokemon === p ? 'border-[#FFD700] bg-[#FFD700] text-[#0D1117]' : 'border-[#30363D] text-[#8B949E] hover:border-[#FFD700] hover:text-[#FFD700]'}`}
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
            <div className="mt-2 flex items-center gap-3 bg-[#161B22] border border-[#FF3C3C]/30 rounded-lg px-4 py-3">
              <img src={spriteUrl(selectedPokemon)} alt={selectedPokemon} className="w-12 h-12" style={{ imageRendering: 'pixelated' }} />
              <span className="font-mono text-[#F0F6FC] text-sm flex-1">{selectedPokemon}</span>
              <button
                onClick={(e) => handlePlay(e, selectedPokemon)}
                className={`w-8 h-8 rounded-full flex items-center justify-center border transition-all text-xs ${playingPokemon === selectedPokemon ? 'border-[#FFD700] bg-[#FFD700] text-[#0D1117]' : 'border-[#FF3C3C] text-[#FF3C3C] hover:bg-[#FF3C3C] hover:text-white'}`}
              >
                {playingPokemon === selectedPokemon ? '■' : '▶'}
              </button>
            </div>
          )}
        </div>
      </div>

      <button
        onClick={handleTranspose}
        disabled={!canTranspose}
        className={`font-display tracking-widest text-lg px-16 py-4 rounded-lg border-2 transition-all duration-200 ${canTranspose ? 'border-[#FFD700] text-[#FFD700] hover:bg-[#FFD700] hover:text-[#0D1117] cursor-pointer active:scale-95' : 'border-[#30363D] text-[#30363D] cursor-not-allowed'}`}
      >
        TRANSPOSE
      </button>

      {!canTranspose && (
        <p className="font-mono pm-muted small mt-3 mb-0">
          {!mp3File && !selectedPokemon
            ? 'Upload an MP3 and select a Pokémon to begin'
            : !mp3File
              ? 'Upload an MP3 to continue'
              : 'Select a Pokémon to continue'}
        </p>
      )}

      {authMode && (
        <AuthModal
          mode={authMode}
          onClose={() => setAuthMode(null)}
          onSwitchMode={setAuthMode}
        />
      )}
    </div>
  )
}
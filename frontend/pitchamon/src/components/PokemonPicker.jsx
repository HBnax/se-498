import { useEffect, useRef, useState } from 'react'

const spriteUrl = (id) =>
  `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`

function PokemonPicker({ pokemonList, loading, selected, onSelect, disabled }) {
  const [open, setOpen] = useState(false)
  const [query, setQuery] = useState('')
  const containerRef = useRef(null)
  const searchRef = useRef(null)

  useEffect(() => {
    if (open && searchRef.current) {
      searchRef.current.focus()
    }
  }, [open])

  useEffect(() => {
    const handleClickOutside = (e) => {
      if (containerRef.current && !containerRef.current.contains(e.target)) {
        setOpen(false)
      }
    }
    const handleKeyDown = (e) => {
      if (e.key === 'Escape') setOpen(false)
    }
    document.addEventListener('mousedown', handleClickOutside)
    document.addEventListener('keydown', handleKeyDown)
    return () => {
      document.removeEventListener('mousedown', handleClickOutside)
      document.removeEventListener('keydown', handleKeyDown)
    }
  }, [])

  const filtered = pokemonList.filter((p) =>
    p.name.toLowerCase().includes(query.toLowerCase())
  )

  const handleSelect = (p) => {
    onSelect(p)
    setOpen(false)
    setQuery('')
  }

  const buttonLabel = loading
    ? 'Loading Pokémon…'
    : selected
      ? selected.name
      : 'Select a Pokémon'

  return (
    <div className="pokemon-picker" ref={containerRef}>
      <button
        type="button"
        className="picker-button"
        onClick={() => !disabled && !loading && setOpen((o) => !o)}
        disabled={disabled || loading}
        aria-haspopup="listbox"
        aria-expanded={open}
      >
        {selected && (
          <img
            src={spriteUrl(selected.id)}
            alt=""
            className="picker-sprite"
          />
        )}
        <span className="picker-label">{buttonLabel}</span>
        <span className="picker-caret" aria-hidden="true">
          ▾
        </span>
      </button>

      {open && (
        <div className="picker-panel">
          <input
            ref={searchRef}
            type="text"
            className="picker-search"
            placeholder="Search Pokémon…"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
          <ul className="picker-list" role="listbox">
            {filtered.length === 0 ? (
              <li className="picker-empty">No matches</li>
            ) : (
              filtered.map((p) => (
                <li key={p.id}>
                  <button
                    type="button"
                    className={`picker-option${
                      selected?.id === p.id ? ' is-selected' : ''
                    }`}
                    onClick={() => handleSelect(p)}
                  >
                    <img
                      src={spriteUrl(p.id)}
                      alt=""
                      className="picker-sprite"
                      loading="lazy"
                    />
                    <span>{p.name}</span>
                  </button>
                </li>
              ))
            )}
          </ul>
        </div>
      )}
    </div>
  )
}

export default PokemonPicker

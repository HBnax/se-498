const spriteUrl = (id) =>
  `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`

function ResultPanel({ pokemon, resultUrl, loading, onReset }) {
  if (loading) {
    return (
      <div className="result-panel is-loading">
        <div className="spinner large" aria-hidden="true" />
        <p>Working on your remix…</p>
      </div>
    )
  }

  if (!resultUrl || !pokemon) {
    return (
      <div className="result-panel is-empty">
        <p className="placeholder">Your remix will appear here.</p>
      </div>
    )
  }

  return (
    <div className="result-panel is-done">
      <img
        src={spriteUrl(pokemon.id)}
        alt={`${pokemon.name} dancing`}
        className="result-sprite"
      />
      <audio controls src={resultUrl} className="result-audio" />
      <div className="result-actions">
        <a
          href={resultUrl}
          download={`pitchamon-${pokemon.name}.mp3`}
          className="result-download"
        >
          Download MP3
        </a>
        <button type="button" className="result-reset" onClick={onReset}>
          Start over
        </button>
      </div>
    </div>
  )
}

export default ResultPanel

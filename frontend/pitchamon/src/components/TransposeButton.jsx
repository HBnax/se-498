function TransposeButton({ disabled, loading, onClick }) {
  return (
    <button
      type="button"
      className="transpose-button"
      disabled={disabled || loading}
      onClick={onClick}
    >
      {loading ? (
        <>
          <span className="spinner" aria-hidden="true" />
          <span>TRANSPOSING…</span>
        </>
      ) : (
        'TRANSPOSE'
      )}
    </button>
  )
}

export default TransposeButton

import { useState } from 'react'

const MAX_FILE_BYTES = 20 * 1024 * 1024 // 20 MB

function FileUpload({ file, onFileChange, disabled }) {
  const [fileError, setFileError] = useState('')

  const handleChange = (e) => {
    const selected = e.target.files?.[0]
    setFileError('')
    if (!selected) {
      onFileChange(null)
      return
    }
    const isMp3 =
      selected.type === 'audio/mpeg' ||
      selected.name.toLowerCase().endsWith('.mp3')
    if (!isMp3) {
      onFileChange(null)
      setFileError('Please select a valid MP3 file.')
      return
    }
    if (selected.size > MAX_FILE_BYTES) {
      onFileChange(null)
      setFileError('File is too large (max 20 MB).')
      return
    }
    onFileChange(selected)
  }

  return (
    <div className="file-upload">
      <label htmlFor="song" className="file-label">
        {file ? 'Change MP3' : 'Choose MP3 file'}
      </label>
      <input
        id="song"
        type="file"
        accept="audio/mpeg,.mp3"
        onChange={handleChange}
        disabled={disabled}
        className="file-input"
      />
      {file && <p className="hint">Selected: {file.name}</p>}
      {fileError && <p className="error-text">{fileError}</p>}
    </div>
  )
}

export default FileUpload

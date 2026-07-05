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
    const name = selected.name.toLowerCase()
    const isAccepted =
      selected.type === 'audio/mpeg' ||
      selected.type === 'audio/midi' ||
      selected.type === 'audio/x-midi' ||
      name.endsWith('.mp3') ||
      name.endsWith('.mid') ||
      name.endsWith('.midi')
    if (!isAccepted) {
      onFileChange(null)
      setFileError('Please select a valid MP3 or MIDI file.')
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
        {file ? 'Change file' : 'Choose MP3 or MIDI file'}
      </label>
      <input
        id="song"
        type="file"
        accept="audio/mpeg,.mp3,audio/midi,audio/x-midi,.mid,.midi"
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

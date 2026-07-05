import librosa
import numpy as np
import soundfile as sf
import sys
import os


def midi_to_pitch_contour(midi_path, n_frames, hop_length, sr):
    import pretty_midi
    pm = pretty_midi.PrettyMIDI(midi_path)
    f0 = np.zeros(n_frames)
    for instrument in pm.instruments:
        if instrument.is_drum:
            continue
        for note in instrument.notes:
            start_frame = int(note.start * sr / hop_length)
            end_frame   = int(note.end   * sr / hop_length)
            freq = 440.0 * (2.0 ** ((note.pitch - 69) / 12.0))
            f0[start_frame : min(end_frame, n_frames)] = freq
    return f0


def process_audio(vocal_path, cry_path,
                  hop_length=512,
                  win_length=2048,
                  rms_ratio=0.5):

    is_midi = vocal_path.lower().endswith(('.mid', '.midi'))

    # Load Pokemon cry
    sample, sr_s = librosa.load(cry_path)

    if is_midi:
        import pretty_midi
        midi_duration = pretty_midi.PrettyMIDI(vocal_path).get_end_time()
        target_len = int(midi_duration * sr_s)
        n_frames = int(np.ceil(target_len / hop_length))
        f0_song = midi_to_pitch_contour(vocal_path, n_frames, hop_length, sr_s)
        rms = None
        threshold = None
    else:
        # Load vocal audio file
        y, sr = librosa.load(vocal_path)
        target_len = len(y)

        # Compute RMS for loudness-based gating
        rms = librosa.feature.rms(y=y, hop_length=hop_length)[0]
        threshold = np.mean(rms) * rms_ratio

        # Extract and smooth pitch curve from vocal
        f0_song = librosa.yin(y, fmin=80, fmax=1000, sr=sr, hop_length=hop_length)
        f0_song = np.nan_to_num(f0_song)
        f0_song = np.convolve(f0_song, np.ones(5)/5, mode='same')

    # Estimate base pitch of Pokemon cry
    f0_sample = librosa.yin(sample, fmin=80, fmax=1000, sr=sr_s)
    f0_sample = f0_sample[~np.isnan(f0_sample)]
    f0_sample = f0_sample[f0_sample > 0]
    base_pitch = np.median(f0_sample)

    # Extend sample to match vocal length
    repeats = int(np.ceil(target_len / len(sample)))
    sample_extended = np.tile(sample, repeats)[:target_len]

    # Initialize output buffer and window
    output = np.zeros(target_len)
    window = np.hanning(win_length)

    # Apply pitch shifting frame-by-frame with overlap-add
    for i in range(len(f0_song)):
        if is_midi:
            if f0_song[i] <= 0:
                continue  # no note active at this frame
        else:
            if rms[i] < threshold:
                continue  # skip low-energy frames

        pitch = f0_song[i]

        center = i * hop_length
        start = center - win_length // 2
        end = center + win_length // 2

        if start < 0 or end > target_len:
            continue

        chunk = sample_extended[start:end]

        if pitch > 0:
            n_steps = 12 * np.log2(pitch / base_pitch)
            shifted = librosa.effects.pitch_shift(chunk, sr=sr_s, n_steps=n_steps)
            shifted = librosa.util.fix_length(shifted, size=win_length)
        else:
            shifted = chunk

        # Overlap-add with windowing
        output[start:end] += shifted * window

    # Normalize output audio
    output = output / (np.max(np.abs(output)) + 1e-6)

    # Save result to file
    output_path = "output.wav"
    sf.write(output_path, output, sr_s)
    absolute_path = os.path.abspath(output_path)
    return absolute_path

if __name__ == "__main__":
    vocal = sys.argv[1]
    cry = sys.argv[2]
    output_path = process_audio(vocal, cry)
    print(output_path)

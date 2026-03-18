# Pitchamon — Frontend Specification

**Version:** 1.0 | **Date:** March 2026 | **Author:** Jeffrey Bok

---

## Overview

Pitchamon is a web app where users upload an MP3 file and choose a Pokémon, and the app transposes the Pokémon's screech to match the song. The result is a downloadable MP3, shown alongside the Pokémon's sprite doing a little dance.

---

## Goals

- User can upload an MP3 file from their device.
- User can choose a Pokémon from a dropdown with a search bar.
- User can trigger the transpose and see a loading state while it processes.
- After processing, the user sees the Pokémon sprite animating and can download the resulting MP3.

---

## Layout

Single page, no routing. Three steps presented side-by-side:

1. **Upload** — MP3 file input
2. **Choose** — Pokémon dropdown with search
3. **Result** — dancing sprite + download button

---

## Components

#### `FileUpload`
- File input that accepts MP3s
- Shows the selected filename once a file is chosen
- States: empty · file selected · invalid file type (error message)

#### `PokemonPicker`
- Dropdown list of all Pokémon
- Search bar inside the dropdown to filter by name
- Shows Pokémon name (and optionally sprite thumbnail) in each option
- States: closed · open · filtered · selected

#### `TransposeButton`
- Disabled until both an MP3 and a Pokémon are selected
- On click: submits the job and enters a loading state
- States: disabled · active · loading

#### `ResultPanel`
- Appears after processing completes
- Shows the selected Pokémon's sprite with a looping dance/idle animation
- Download button for the transposed MP3
- Reset button to start over

#### `ErrorBanner`
- Shown when something goes wrong
- Dismissable, displays a short error message

---

## User Flow

1. User uploads an MP3 file.
2. User picks a Pokémon from the dropdown (can search by name).
3. User clicks **TRANSPOSE**.
4. Loading state shown while backend processes.
5. Result panel appears — Pokémon sprite dances, download button available.
6. User downloads the transposed MP3.

---

## States

| State | Description |
|---|---|
| Idle | No file uploaded, no Pokémon selected. Button disabled. |
| File Selected | MP3 uploaded, waiting for Pokémon selection. |
| Pokémon Selected | Both inputs filled. Transpose button active. |
| Processing | Loading indicator shown. Inputs locked. |
| Done | Result panel visible with sprite animation and download. |
| Error | ErrorBanner shown. Inputs unlocked to retry. |

---

## Open Questions

- [ ] Where does the Pokémon sprite/animation asset come from? (PokeAPI, custom sprites, etc.)
- [ ] What does the loading state look like — spinner, progress bar, or something themed?
- [ ] What file size limit should we enforce on the uploaded MP3?
- [ ] Does the dance animation loop indefinitely or stop after a few seconds?
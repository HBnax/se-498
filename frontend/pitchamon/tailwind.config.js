/** @type {import('tailwindcss').Config} */
export default {
    content: ['./index.html', './src/**/*.{js,jsx}'],
    theme: {
      extend: {
        fontFamily: {
          display: ['"Press Start 2P"', 'cursive'],
          mono: ['"IBM Plex Mono"', 'monospace'],
        },
      },
    },
    plugins: [],
  }
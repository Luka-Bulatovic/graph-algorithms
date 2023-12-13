/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./wwwroot/**/*.js", "./Views/**/*.cshtml"],
  theme: {
    extend: {
          width: {
              '128': '32rem'
          },
          height: {
              '32': '8rem',
              '128': '32rem',
              '160': '40rem',
              '192': '48rem'
          },
          spacing: {
              '16': '4rem'
          }
    }
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}


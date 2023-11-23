/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./wwwroot/**/*.js", "./Views/**/*.cshtml"],
  theme: {
    extend: {
          width: {
              '128': '32rem'
          },
          height: {
              '128': '32rem'
          }
    }
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}


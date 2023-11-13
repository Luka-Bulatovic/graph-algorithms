/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./wwwroot/**/*.js", "./Views/**/*.cshtml"],
  theme: {
    extend: {},
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}


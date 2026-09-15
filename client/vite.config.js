import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// The proxy depends on the ASP.NET Core URL in server/Properties/launchSettings.json.
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': { target: 'http://localhost:5080', changeOrigin: true }
    }
  }
})


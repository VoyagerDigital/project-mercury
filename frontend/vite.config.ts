import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import path from 'node:path'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')

  const apiProxyTarget =
    env.VITE_PROXY_TARGET ||
    env.API_HTTP ||
    env.API_HTTPS ||
    env.services__Api__https__0 ||
    env.services__Api__http__0 ||
    env.services__api__https__0 ||
    env.services__api__http__0 ||
    env.SERVICES__API__HTTPS__0 ||
    env.SERVICES__API__HTTP__0 ||
    'https://localhost:7090'

  return {
    plugins: [react(), tailwindcss()],
    resolve: {
      alias: {
        '@': path.resolve(__dirname, './src'),
        '@app': path.resolve(__dirname, './src/app'),
        '@components': path.resolve(__dirname, './src/components'),
        '@modules': path.resolve(__dirname, './src/modules'),
        '@shared': path.resolve(__dirname, './src/shared'),
        '@lib': path.resolve(__dirname, './src/lib'),
      },
    },
    server: {
      proxy: {
        '/api': {
          target: apiProxyTarget,
          changeOrigin: true,
          secure: false,
        },
      },
    },
  }
})

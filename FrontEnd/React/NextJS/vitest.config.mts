import react from '@vitejs/plugin-react';
import path from 'path';
import { defineConfig } from 'vitest/config';

export default defineConfig({
  plugins: [react()],
  test: {
    setupFiles: ['./vitest-setup.ts'],
    environment: 'jsdom',
  },
  resolve: {
    alias: {
      "@Core": path.resolve(__dirname, "../../Core"),
      "@": path.resolve(__dirname, "./src"),
    },
  },
});

import { defineConfig } from 'vitest/config';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  test: {
    environment: 'jsdom',
  },
  resolve: {
    alias: {
      "@Core": path.resolve(__dirname, "../../../Core"),
      "@": path.resolve(__dirname, "./src"),
    },
  },
});

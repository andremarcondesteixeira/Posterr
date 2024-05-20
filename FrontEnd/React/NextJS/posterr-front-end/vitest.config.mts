import type { TestingLibraryMatchers } from "@testing-library/jest-dom/matchers";
import matchers from "@testing-library/jest-dom/matchers";
import react from '@vitejs/plugin-react';
import path from 'path';
import { expect } from "vitest";
import { defineConfig } from 'vitest/config';

declare module "vitest" {
  interface Assertion<T = any>
    extends jest.Matchers<void, T>,
      TestingLibraryMatchers<T, void> {}
}

expect.extend(matchers);

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

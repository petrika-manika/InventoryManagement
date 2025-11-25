import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    // Proxy /api requests to backend during development
    proxy: {
      "/api": {
        target: "https://localhost:7173",
        changeOrigin: true,
        secure: false, // For development with self-signed certificates
      },
    },
  },
});

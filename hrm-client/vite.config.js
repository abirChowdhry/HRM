import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  base: "./",
  plugins: [react()],
  server: {
    port: 5174,
    proxy: {
      "/api": {
        target: "https://localhost:7237",
        changeOrigin: true,
        secure: false
      }
    }
  }
});

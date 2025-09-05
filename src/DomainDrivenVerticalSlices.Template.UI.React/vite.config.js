// vite.config.js
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";

export default defineConfig({
    plugins: [react()],
    server: {
        proxy: {
            "/api": {
                target: "http://localhost:5246",
                changeOrigin: true,
            },
        },
    },
    test: {
        environment: "jsdom",
        setupFiles: "./src/setupTests.js",
        coverage: { reporter: ["text", "lcov"] },
        globals: true,
    },
    resolve: { alias: { "@": "/src" } },
});

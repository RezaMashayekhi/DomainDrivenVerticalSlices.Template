// vite.config.js
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";

// When running via Aspire, the WebApi URL is injected via environment variables
// from .WithReference(webApi) in the AppHost Program.cs
const apiTarget =
    process.env.services__webapi__https__0 ||
    process.env.services__webapi__http__0 ||
    "http://localhost:5246";

export default defineConfig({
    plugins: [react()],
    server: {
        proxy: {
            "/api": {
                target: apiTarget,
                changeOrigin: true,
                secure: false, // allow self-signed dev certificates
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

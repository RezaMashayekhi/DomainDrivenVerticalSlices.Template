import js from "@eslint/js";
import globals from "globals";
import reactPlugin from "eslint-plugin-react";
import reactHooksPlugin from "eslint-plugin-react-hooks";

const sourceFiles = ["src/**/*.{js,jsx}", "vite.config.js"];
const testFiles = ["src/**/*.{test,spec}.{js,jsx}"];

export default [
    {
        ignores: ["dist", "coverage", "node_modules"],
    },
    js.configs.recommended,
    {
        files: sourceFiles,
        languageOptions: {
            ecmaVersion: "latest",
            sourceType: "module",
            parserOptions: {
                ecmaFeatures: {
                    jsx: true,
                },
            },
            globals: {
                ...globals.browser,
                ...globals.node,
            },
        },
        plugins: {
            react: reactPlugin,
            "react-hooks": reactHooksPlugin,
        },
        settings: {
            react: {
                version: "detect",
            },
        },
        rules: {
            ...reactPlugin.configs.flat.recommended.rules,
            "react/react-in-jsx-scope": "off",
            "react-hooks/rules-of-hooks": "error",
            "react-hooks/exhaustive-deps": "warn",
        },
    },
    {
        files: testFiles,
        languageOptions: {
            globals: {
                ...globals.vitest,
            },
        },
    },
];

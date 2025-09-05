module.exports = {
    env: { browser: true, es2022: true, node: true },
    extends: ["eslint:recommended", "plugin:react/recommended"],
    parserOptions: { ecmaVersion: "latest", sourceType: "module" },
    settings: { react: { version: "detect" } },
    plugins: ["react", "react-hooks"],
    rules: { "react/react-in-jsx-scope": "off" },
};

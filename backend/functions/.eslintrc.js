module.exports = {
  root: true,
  env: {
    es6: true,
    node: true,
  },
  extends: [
    "eslint:recommended",
    "plugin:import/errors",
    "plugin:import/warnings",
    "plugin:import/typescript",
    "google",
    "plugin:@typescript-eslint/recommended",
  ],
  parser: "@typescript-eslint/parser",
  parserOptions: {
    project: ["tsconfig.json", "tsconfig.dev.json"],
    sourceType: "module",
  },
  ignorePatterns: [
    "/lib/**/*", // Ignore built files.
  ],
  plugins: [
    "@typescript-eslint",
    "import",
  ],
  rules: {
    "quotes": 0,
    "semi": 0,
    "indent": 0,
    "max-len": 0,
    "comma-dangle": 0,
    "camelcase": 0,
    "import/no-unresolved": 0,
    "object-curly-spacing": 0,
    "eol-last": 0,
    "no-tabs": 0,
    "no-trailing-spaces": 0,
    "@typescript-eslint/no-var-requires": 0,
    "padded-blocks": 0,
    "@typescript-eslint/ban-ts-comment": 0,
    "no-multiple-empty-lines": 0,
    "no-multi-spaces": 0
  },
};

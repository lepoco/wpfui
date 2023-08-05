// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

module.exports = {
  env: {
    browser: true
  },
  ignorePatterns: ['**/*.js'],
  extends: ['standard', 'eslint:recommended', 'plugin:@typescript-eslint/recommended'],
  parser: '@typescript-eslint/parser',
  plugins: ['@typescript-eslint'],
  root: true,
  rules: {
    'space-before-function-paren': ['warn', 'never'],
  }
};

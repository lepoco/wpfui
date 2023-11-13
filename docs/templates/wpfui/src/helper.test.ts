// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { breakWord } from './helper'

test('break-text', () => {
  expect(breakWord('Other APIs')).toEqual(['Other APIs'])
  expect(breakWord('System.CodeDom')).toEqual(['System.', 'Code', 'Dom'])
  expect(breakWord('System.Collections.Dictionary<string, object>')).toEqual(['System.', 'Collections.', 'Dictionary<', 'string,', ' object>'])
  expect(breakWord('https://github.com/dotnet/docfx')).toEqual(['https://github.', 'com/', 'dotnet/', 'docfx'])
})

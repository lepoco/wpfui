// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import 'bootstrap'
import { DocfxOptions } from './options'
import { highlight } from './highlight'
import { renderMarkdown } from './markdown'
import { enableSearch } from './search'
import { renderToc } from './toc'
import { initTheme } from './theme'
import { renderBreadcrumb, renderInThisArticle, renderNavbar } from './nav'
import { renderIndexStats } from './wpfui-index-stats'

import 'bootstrap-icons/font/bootstrap-icons.scss'
import './docfx.scss'

declare global {
  interface Window {
    docfx: DocfxOptions & {
      ready?: boolean,
      searchReady?: boolean,
      searchResultReady?: boolean,
    }
  }
}

export async function init() {
  const options = {
    defaultTheme: 'dark'
  } as DocfxOptions

  window.docfx = Object.assign({}, options)

  initTheme()
  enableSearch()
  renderInThisArticle()
  renderIndexStats()

  await Promise.all([
    renderMarkdown(),
    renderNav(),
    highlight()
  ])

  window.docfx.ready = true

  async function renderNav() {
    const [navbar, toc] = await Promise.all([renderNavbar(), renderToc()])
    renderBreadcrumb([...navbar, ...toc])
  }
}

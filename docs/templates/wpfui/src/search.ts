// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { meta } from './helper'
import { html, render, TemplateResult } from 'lit-html'
import { classMap } from 'lit-html/directives/class-map.js'

type SearchHit = {
  href: string
  title: string
  keywords: string
}

let query

/**
 * Support full-text-search
 */
export function enableSearch() {
  const searchQuery = document.getElementById('search-query') as HTMLInputElement
  if (!searchQuery || !window.Worker) {
    return
  }

  const relHref = meta('docfx:rel') || ''
  const worker = new Worker(relHref + 'public/search-worker.min.js', { type: 'module' })
  worker.onmessage = function(oEvent) {
    switch (oEvent.data.e) {
      case 'index-ready':
        searchQuery.disabled = false
        searchQuery.addEventListener('input', onSearchQueryInput)
        window.docfx.searchReady = true
        break
      case 'query-ready':
        document.body.setAttribute('data-search', 'true')
        renderSearchResults(oEvent.data.d, 0)
        window.docfx.searchResultReady = true
        break
    }
  }

  function onSearchQueryInput() {
    query = searchQuery.value
    if (query.length < 3) {
      document.body.removeAttribute('data-search')
    } else {
      worker.postMessage({ q: query })
    }
  }

  function relativeUrlToAbsoluteUrl(currentUrl, relativeUrl) {
    const currentItems = currentUrl.split(/\/+/)
    const relativeItems = relativeUrl.split(/\/+/)
    let depth = currentItems.length - 1
    const items = []
    for (let i = 0; i < relativeItems.length; i++) {
      if (relativeItems[i] === '..') {
        depth--
      } else if (relativeItems[i] !== '.') {
        items.push(relativeItems[i])
      }
    }
    return currentItems.slice(0, depth).concat(items).join('/')
  }

  function extractContentBrief(content) {
    const briefOffset = 512
    const words = query.split(/\s+/g)
    const queryIndex = content.indexOf(words[0])
    if (queryIndex > briefOffset) {
      return '...' + content.slice(queryIndex - briefOffset, queryIndex + briefOffset) + '...'
    } else if (queryIndex <= briefOffset) {
      return content.slice(0, queryIndex + briefOffset) + '...'
    }
  }

  function renderSearchResults(hits: SearchHit[], page: number) {
    const numPerPage = 10
    const totalPages = Math.ceil(hits.length / numPerPage)

    render(
      renderPage(page),
      document.getElementById('search-results'))

    function renderPage(page: number): TemplateResult {
      if (hits.length === 0) {
        return html`<div class="search-list">No results for "${query}"</div>`
      }

      const start = page * numPerPage
      const curHits = hits.slice(start, start + numPerPage)

      const items = html`
        <div class="search-list">${hits.length} results for "${query}"</div>
        <div class="sr-items">${curHits.map(hit => {
          const currentUrl = window.location.href
          const itemRawHref = relativeUrlToAbsoluteUrl(currentUrl, relHref + hit.href)
          const itemHref = relHref + hit.href + '?q=' + query
          const itemBrief = extractContentBrief(hit.keywords)

          return html`
            <div class="sr-item">
              <div class="item-title"><a href="${itemHref}" target="_blank" rel="noopener noreferrer">${mark(hit.title, query)}</a></div>
              <div class="item-href">${mark(itemRawHref, query)}</div>
              <div class="item-brief">${mark(itemBrief, query)}</div>
            </div>`
          })}
        </div>`

      return html`${items} ${renderPagination()}`
    }

    function renderPagination() {
      const maxVisiblePages = 5
      const startPage = Math.max(0, Math.min(page - 2, totalPages - maxVisiblePages))
      const endPage = Math.min(totalPages, startPage + maxVisiblePages)
      const pages = Array.from(new Array(endPage - startPage).keys()).map(i => i + startPage)

      if (pages.length <= 1) {
        return null
      }

      return html`
        <nav>
          <ul class="pagination">
            <li class="page-item">
              <a class="page-link ${classMap({ disabled: page <= 0 })}" href="#" aria-label="Previous"
                @click="${() => gotoPage(page - 1)}">
                <span aria-hidden="true">&laquo;</span>
              </a>
            </li>
            ${pages.map(i => html`
              <li class="page-item">
                <a class="page-link ${classMap({ active: page === i })}" href="#"
                  @click="${() => gotoPage(i)}">${i + 1}</a></li>`)}
            <li class="page-item">
              <a class="page-link ${classMap({ disabled: page >= totalPages - 1 })}" href="#" aria-label="Next"
                @click="${() => gotoPage(page + 1)}">
                <span aria-hidden="true">&raquo;</span>
              </a>
            </li>
          </ul>
        </nav>`

      function gotoPage(page: number) {
        if (page >= 0 && page < totalPages) {
          renderSearchResults(hits, page)
        }
      }
    }
  }
}

function mark(text: string, query: string): TemplateResult {
  const words = query.split(/\s+/g)
  const wordsLower = words.map(w => w.toLowerCase())
  const textLower = text.toLowerCase()
  const result = []
  let lastEnd = 0
  for (let i = 0; i < wordsLower.length; i++) {
    const word = wordsLower[i]
    const index = textLower.indexOf(word, lastEnd)
    if (index >= 0) {
      result.push(html`${text.slice(lastEnd, index)}`)
      result.push(html`<b>${text.slice(index, index + word.length)}</b>`)
      lastEnd = index + word.length
    }
  }
  result.push(html`${text.slice(lastEnd)}`)
  return html`${result}`
}

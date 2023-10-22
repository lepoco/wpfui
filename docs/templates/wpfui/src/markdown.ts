// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { breakWord, meta } from './helper'
import AnchorJs from 'anchor-js'
import { html, render } from 'lit-html'
import { getTheme } from './theme'

/**
 * Initialize markdown rendering.
 */
export async function renderMarkdown() {
  renderWordBreaks()
  renderTables()
  renderAlerts()
  renderLinks()
  renderTabs()
  renderAnchor()
  renderCodeCopy()
  renderClickableImage()

  await Promise.all([
    renderMath(),
    renderMermaid()
  ])
}

async function renderMath() {
  const math = document.querySelectorAll('.math')
  if (math.length > 0) {
    await import('mathjax/es5/tex-svg-full.js')
  }
}

let mermaidRenderCount = 0

/**
 * Render mermaid diagrams.
 */
async function renderMermaid() {
  const diagrams = document.querySelectorAll<HTMLElement>('pre code.lang-mermaid')
  if (diagrams.length <= 0) {
    return
  }

  const { default: mermaid } = await import('mermaid')
  const theme = getTheme() === 'dark' ? 'dark' : 'default'

  // Turn off deterministic ids on re-render
  const deterministicIds = mermaidRenderCount === 0
  mermaid.initialize(Object.assign({ startOnLoad: false, deterministicIds, theme }, window.docfx.mermaid))
  mermaidRenderCount++

  const nodes = []
  diagrams.forEach(e => {
    // Rerender when elements becomes visible due to https://github.com/mermaid-js/mermaid/issues/1846
    if (e.offsetParent) {
      nodes.push(e.parentElement)
      e.parentElement.classList.add('mermaid')
      e.parentElement.innerHTML = e.innerHTML
    }
  })

  await mermaid.run({ nodes })
}

/**
 * Add <wbr> to break long text.
 */
function renderWordBreaks() {
  document.querySelectorAll<HTMLElement>('article h1,h2,h3,h4,h5,h6,.xref,.text-break').forEach(e => {
    if (e.innerHTML?.trim() === e.innerText?.trim()) {
      const children: (string | Node)[] = []
      for (const text of breakWord(e.innerText)) {
        if (children.length > 0) {
          children.push(document.createElement('wbr'))
        }
        children.push(text)
      }
      e.replaceChildren(...children)
    }
  })
}

/**
 * Make images in articles clickable by wrapping the image in an anchor tag.
 * The image is clickable only if its size is larger than 200x200 and it is not already been wrapped in an anchor tag.
 */
function renderClickableImage() {
  const MIN_CLICKABLE_IMAGE_SIZE = 200
  const imageLinks = Array.from(document.querySelectorAll<HTMLImageElement>('article a img[src]'))

  document.querySelectorAll<HTMLImageElement>('article img[src]').forEach(img => {
    if (shouldMakeClickable()) {
      makeClickable()
    } else {
      img.addEventListener('load', () => {
        if (shouldMakeClickable()) {
          makeClickable()
        }
      })
    }

    function makeClickable() {
      const a = document.createElement('a')
      a.target = '_blank'
      a.rel = 'noopener noreferrer nofollow'
      a.href = img.src
      img.replaceWith(a)
      a.appendChild(img)
    }

    function shouldMakeClickable(): boolean {
      return img.naturalWidth > MIN_CLICKABLE_IMAGE_SIZE &&
        img.naturalHeight > MIN_CLICKABLE_IMAGE_SIZE &&
        !imageLinks.includes(img)
    }
  })
}

/**
 * Styling for tables in conceptual documents using Bootstrap.
 * See http://getbootstrap.com/css/#tables
 */
function renderTables() {
  document.querySelectorAll('table').forEach(table => {
    table.classList.add('table', 'table-bordered', 'table-condensed')
    const wrapper = document.createElement('div')
    wrapper.className = 'table-responsive'
    table.parentElement.insertBefore(wrapper, table)
    wrapper.appendChild(table)
  })
}

/**
 * Styling for alerts.
 */
function renderAlerts() {
  document.querySelectorAll('.NOTE, .TIP').forEach(e => e.classList.add('alert', 'alert-info'))
  document.querySelectorAll('.WARNING').forEach(e => e.classList.add('alert', 'alert-warning'))
  document.querySelectorAll('.IMPORTANT, .CAUTION').forEach(e => e.classList.add('alert', 'alert-danger'))
}

/**
 * Open external links to different host in a new window.
 */
function renderLinks() {
  if (meta('docfx:disablenewtab') === 'true') {
    return
  }

  document.querySelectorAll<HTMLAnchorElement>('article a[href]').forEach(a => {
    if (a.hostname !== window.location.hostname && a.innerText.trim() !== '') {
      a.target = '_blank'
      a.rel = 'noopener noreferrer nofollow'
      a.classList.add('external')
    }
  })
}

/**
 * Render anchor # for headings
 */
function renderAnchor() {
  const anchors = new AnchorJs()
  anchors.options = Object.assign({
    visible: 'hover',
    icon: '#'
  }, window.docfx.anchors)

  anchors.add('article h2:not(.no-anchor), article h3:not(.no-anchor), article h4:not(.no-anchor)')
}

/**
 * Render code copy button.
 */
function renderCodeCopy() {
  document.querySelectorAll<HTMLElement>('pre>code').forEach(code => {
    if (code.innerText.trim().length === 0) {
      return
    }

    let copied = false
    renderCore()

    function renderCore() {
      const dom = copied
        ? html`<a class='btn border-0 link-success code-action'><i class='bi bi-check-lg'></i></a>`
        : html`<a class='btn border-0 code-action' title='copy' href='#' @click=${copy}><i class='bi bi-clipboard'></i></a>`
      render(dom, code.parentElement)

      async function copy(e) {
        e.preventDefault()
        await navigator.clipboard.writeText(code.innerText)
        copied = true
        renderCore()
        setTimeout(() => {
          copied = false
          renderCore()
        }, 1000)
      }
    }
  })
}

/**
 * Render tabbed content.
 */
function renderTabs() {
  updateTabStyle()

  const contentAttrs = {
    id: 'data-bi-id',
    name: 'data-bi-name',
    type: 'data-bi-type'
  }

  const Tab = (function() {
    function Tab(li, a, section) {
      this.li = li
      this.a = a
      this.section = section
    }
    Object.defineProperty(Tab.prototype, 'tabIds', {
      get: function() { return this.a.getAttribute('data-tab').split(' ') },
      enumerable: true,
      configurable: true
    })
    Object.defineProperty(Tab.prototype, 'condition', {
      get: function() { return this.a.getAttribute('data-condition') },
      enumerable: true,
      configurable: true
    })
    Object.defineProperty(Tab.prototype, 'visible', {
      get: function() { return !this.li.hasAttribute('hidden') },
      set: function(value) {
        if (value) {
          this.li.removeAttribute('hidden')
          this.li.removeAttribute('aria-hidden')
        } else {
          this.li.setAttribute('hidden', 'hidden')
          this.li.setAttribute('aria-hidden', 'true')
        }
      },
      enumerable: true,
      configurable: true
    })
    Object.defineProperty(Tab.prototype, 'selected', {
      get: function() { return !this.section.hasAttribute('hidden') },
      set: function(value) {
        if (value) {
          this.a.setAttribute('aria-selected', 'true')
          this.a.classList.add('active')
          this.a.tabIndex = 0
          this.section.removeAttribute('hidden')
          this.section.removeAttribute('aria-hidden')
        } else {
          this.a.setAttribute('aria-selected', 'false')
          this.a.classList.remove('active')
          this.a.tabIndex = -1
          this.section.setAttribute('hidden', 'hidden')
          this.section.setAttribute('aria-hidden', 'true')
        }
      },
      enumerable: true,
      configurable: true
    })
    Tab.prototype.focus = function() {
      this.a.focus()
    }
    return Tab
  }())

  initTabs(document.body)

  function initTabs(container) {
    const queryStringTabs = readTabsQueryStringParam()
    const elements = container.querySelectorAll('.tabGroup')
    const state = { groups: [], selectedTabs: [] }
    for (let i = 0; i < elements.length; i++) {
      const group = initTabGroup(elements.item(i))
      if (!group.independent) {
        updateVisibilityAndSelection(group, state)
        state.groups.push(group)
      }
    }
    container.addEventListener('click', function(event) { return handleClick(event, state) })
    if (state.groups.length === 0) {
      return state
    }
    selectTabs(queryStringTabs)
    updateTabsQueryStringParam(state)
    return state
  }

  function initTabGroup(element) {
    const group = {
      independent: element.hasAttribute('data-tab-group-independent'),
      tabs: []
    }
    let li = element.firstElementChild.firstElementChild
    while (li) {
      const a = li.firstElementChild
      a.setAttribute(contentAttrs.name, 'tab')
      const dataTab = a.getAttribute('data-tab').replace(/\+/g, ' ')
      a.setAttribute('data-tab', dataTab)
      const section = element.querySelector('[id="' + a.getAttribute('aria-controls') + '"]')
      const tab = new Tab(li, a, section)
      group.tabs.push(tab)
      li = li.nextElementSibling
    }
    element.setAttribute(contentAttrs.name, 'tab-group')
    element.tabGroup = group
    return group
  }

  function updateVisibilityAndSelection(group, state) {
    let anySelected = false
    let firstVisibleTab
    for (let _i = 0, _a = group.tabs; _i < _a.length; _i++) {
      const tab = _a[_i]
      tab.visible = tab.condition === null || state.selectedTabs.indexOf(tab.condition) !== -1
      if (tab.visible) {
        if (!firstVisibleTab) {
          firstVisibleTab = tab
        }
      }
      tab.selected = tab.visible && arraysIntersect(state.selectedTabs, tab.tabIds)
      anySelected = anySelected || tab.selected
    }
    if (!anySelected) {
      for (let _b = 0, _c = group.tabs; _b < _c.length; _b++) {
        const tabIds = _c[_b].tabIds
        for (let _d = 0, tabIds1 = tabIds; _d < tabIds1.length; _d++) {
          const tabId = tabIds1[_d]
          const index = state.selectedTabs.indexOf(tabId)
          if (index === -1) {
            continue
          }
          state.selectedTabs.splice(index, 1)
        }
      }
      const tab = firstVisibleTab
      tab.selected = true
      state.selectedTabs.push(tab.tabIds[0])
    }
  }

  function getTabInfoFromEvent(event) {
    if (!(event.target instanceof HTMLElement)) {
      return null
    }
    const anchor = event.target.closest('a[data-tab]')
    if (anchor === null) {
      return null
    }
    const tabIds = anchor.getAttribute('data-tab').split(' ')
    const group = anchor.parentElement.parentElement.parentElement.tabGroup
    if (group === undefined) {
      return null
    }
    return { tabIds, group, anchor }
  }

  function handleClick(event, state) {
    const info = getTabInfoFromEvent(event)
    if (info === null) {
      return
    }
    event.preventDefault()
    info.anchor.href = 'javascript:'
    setTimeout(function() {
      info.anchor.href = '#' + info.anchor.getAttribute('aria-controls')
    })
    const tabIds = info.tabIds; const group = info.group
    const originalTop = info.anchor.getBoundingClientRect().top
    if (group.independent) {
      for (let _i = 0, _a = group.tabs; _i < _a.length; _i++) {
        const tab = _a[_i]
        tab.selected = arraysIntersect(tab.tabIds, tabIds)
      }
    } else {
      if (arraysIntersect(state.selectedTabs, tabIds)) {
        return
      }
      const previousTabId = group.tabs.filter(function(t) { return t.selected })[0].tabIds[0]
      state.selectedTabs.splice(state.selectedTabs.indexOf(previousTabId), 1, tabIds[0])
      for (let _b = 0, _c = state.groups; _b < _c.length; _b++) {
        const group1 = _c[_b]
        updateVisibilityAndSelection(group1, state)
      }
      updateTabsQueryStringParam(state)
    }
    notifyContentUpdated()
    const top = info.anchor.getBoundingClientRect().top
    if (top !== originalTop && event instanceof MouseEvent) {
      window.scrollTo(0, window.pageYOffset + top - originalTop)
    }
  }

  function selectTabs(tabIds) {
    for (let _i = 0, tabIds1 = tabIds; _i < tabIds1.length; _i++) {
      const tabId = tabIds1[_i]
      const a = document.querySelector('.tabGroup > ul > li > a[data-tab="' + tabId + '"]:not([hidden])')
      if (a === null) {
        return
      }
      a.dispatchEvent(new CustomEvent('click', { bubbles: true }))
    }
  }

  function readTabsQueryStringParam() {
    const qs = new URLSearchParams(window.location.search)
    const t = qs.get('tabs')
    if (!t) {
      return []
    }
    return t.split(',')
  }

  function updateTabsQueryStringParam(state) {
    const qs = new URLSearchParams(window.location.search)
    qs.set('tabs', state.selectedTabs.join())
    const url = location.protocol + '//' + location.host + location.pathname + '?' + qs.toString() + location.hash
    if (location.href === url) {
      return
    }
    history.replaceState({}, document.title, url)
  }

  function arraysIntersect(a, b) {
    for (let _i = 0, a1 = a; _i < a1.length; _i++) {
      const itemA = a1[_i]
      for (let _a = 0, b1 = b; _a < b1.length; _a++) {
        const itemB = b1[_a]
        if (itemA === itemB) {
          return true
        }
      }
    }
    return false
  }

  function updateTabStyle() {
    document.querySelectorAll('div.tabGroup>ul').forEach(e => e.classList.add('nav', 'nav-tabs'))
    document.querySelectorAll('div.tabGroup>ul>li').forEach(e => e.classList.add('nav-item'))
    document.querySelectorAll('div.tabGroup>ul>li>a').forEach(e => e.classList.add('nav-link'))
    document.querySelectorAll('div.tabGroup>section').forEach(e => e.classList.add('card'))
  }

  function notifyContentUpdated() {
    renderMermaid()
  }
}

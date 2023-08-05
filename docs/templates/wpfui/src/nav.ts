// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { render, html, TemplateResult } from 'lit-html'
import { breakWordLit, meta, isExternalHref } from './helper'
import { themePicker } from './theme'
import { TocNode } from './toc'

export type NavItem = {
  name: string
  href: URL
}

export type NavItemContainer = {
  name: string
  items: NavItem[]
}

/**
 * @returns active navbar items
 */
export async function renderNavbar(): Promise<NavItem[]> {
  const navbar = document.getElementById('navbar')
  if (!navbar) {
    return
  }

  const navItems = await loadNavItems()
  const activeItem = findActiveItem(navItems)

  const menuItem = item => {
    const current = (item === activeItem ? 'page' : false)
    const active = (item === activeItem ? 'active' : null)
    return html`<li class='nav-item'><a class='nav-link ${active}' aria-current=${current} href=${item.href}>${breakWordLit(item.name)}</a></li>`
  }

  const menu = html`
    <ul class='navbar-nav'>${navItems.map(item => {
    if ('items' in item) {
      const active = item.items.some(i => i === activeItem) ? 'active' : null
      return html`
            <li class='nav-item dropdown'>
              <a class='nav-link dropdown-toggle ${active}' href='#' role='button' data-bs-toggle='dropdown' aria-expanded='false'>
                ${breakWordLit(item.name)}
              </a>
              <ul class='dropdown-menu'>${item.items.map(menuItem)}</ul>
            </li>`
    } else {
      return menuItem(item)
    }
  })
    }</ul>`

  function renderCore() {
    const icons = html`
      <form class="icons">
        ${window.docfx.iconLinks?.map(i => html`<a href="${i.href}" title="${i.title}" class="btn border-0"><i class="bi bi-${i.icon}"></i></a>`)}
        ${themePicker(renderCore)}
      </form>`

    render(html`${menu} ${icons}`, navbar)
  }

  renderCore()

  return activeItem ? [activeItem] : []

  async function loadNavItems(): Promise<(NavItem | NavItemContainer)[]> {
    const navrel = meta('docfx:navrel')
    if (!navrel) {
      return []
    }

    const navUrl = new URL(navrel.replace(/.html$/gi, '.json'), window.location.href)
    const { items } = await fetch(navUrl).then(res => res.json())
    return items.map((a: NavItem | NavItemContainer) => {
      if ('items' in a) {
        return { name: a.name, items: a.items.map(i => ({ name: i.name, href: new URL(i.href, navUrl) })) }
      }
      return { name: a.name, href: new URL(a.href, navUrl) }
    })
  }
}

export function renderBreadcrumb(breadcrumb: (NavItem | TocNode)[]) {
  const container = document.getElementById('breadcrumb')
  if (container) {
    render(
      html`
        <ol class="breadcrumb">
          ${breadcrumb.map(i => html`<li class="breadcrumb-item"><a href="${i.href}">${breakWordLit(i.name)}</a></li>`)}
        </ol>`,
      container)
  }
}

export function renderInThisArticle() {
  const affix = document.getElementById('affix')
  const windowPathname = window.location.pathname

  if (windowPathname === '' || windowPathname === '/' || windowPathname === '/index.html') {
    return
  }

  if (affix) {
    render(document.body.getAttribute('data-yaml-mime') === 'ManagedReference' ? inThisArticleForManagedReference() : inThisArticleForConceptual(), affix)
  }
}

function inThisArticleForConceptual() {
  const headings = document.querySelectorAll<HTMLHeadingElement>('article h2')
  if (headings.length > 0) {
    return html`
      <h5 class="border-bottom">In this article</h5>
      <ul>${Array.from(headings).map(h => html`<li><a class="link-secondary" href="#${h.id}">${breakWordLit(h.innerText)}</a></li>`)}</ul>`
  }
}

function inThisArticleForManagedReference(): TemplateResult {
  let headings = Array.from(document.querySelectorAll<HTMLHeadingElement>('article h2, article h3'))
  headings = headings.filter((h, i) => h.tagName === 'H3' || headings[i + 1]?.tagName === 'H3')

  if (headings.length > 0) {
    return html`
      <h5 class="border-bottom">In this article</h5>
      <ul>${headings.map(h => {
      return h.tagName === 'H2'
        ? html`<li><h6>${breakWordLit(h.innerText)}</h6></li>`
        : html`<li><a class="link-secondary" href="#${h.id}">${breakWordLit(h.innerText)}</a></li>`
    })}</ul>`
  }
}

function findActiveItem(items: (NavItem | NavItemContainer)[]): NavItem {
  const url = new URL(window.location.href)
  let activeItem: NavItem
  let maxPrefix = 0
  for (const item of items.map(i => 'items' in i ? i.items : i).flat()) {
    if (isExternalHref(item.href)) {
      continue
    }
    const prefix = commonUrlPrefix(url, item.href)
    if (prefix > maxPrefix) {
      maxPrefix = prefix
      activeItem = item
    }
  }
  return activeItem
}

function commonUrlPrefix(url: URL, base: URL): number {
  const urlSegments = url.pathname.split('/')
  const baseSegments = base.pathname.split('/')
  let i = 0
  while (i < urlSegments.length && i < baseSegments.length && urlSegments[i] === baseSegments[i]) {
    i++
  }
  return i
}

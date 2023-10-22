function updateBaseStats() {
  console.debug('Index stats initialized')
}

export function renderIndexStats() {
  const windowPathname = window.location.pathname

  if (windowPathname === '' || windowPathname === '/' || windowPathname === '/index.html') {
    updateBaseStats()
  }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import lunr from 'lunr'

let lunrIndex

let stopWords = null
let searchData = {}

lunr.tokenizer.separator = /[\s\-.()]+/

const stopWordsRequest = new XMLHttpRequest()
stopWordsRequest.open('GET', '../search-stopwords.json')
stopWordsRequest.onload = function() {
  if (this.status !== 200) {
    return
  }
  stopWords = JSON.parse(this.responseText)
  buildIndex()
}
stopWordsRequest.send()

const searchDataRequest = new XMLHttpRequest()

searchDataRequest.open('GET', '../index.json')
searchDataRequest.onload = function() {
  if (this.status !== 200) {
    return
  }
  searchData = JSON.parse(this.responseText)

  buildIndex()

  postMessage({ e: 'index-ready' })
}
searchDataRequest.send()

onmessage = function(oEvent) {
  const q = oEvent.data.q
  const results = []
  if (lunrIndex) {
    const hits = lunrIndex.search(q)
    hits.forEach(function(hit) {
      const item = searchData[hit.ref]
      results.push({ href: item.href, title: item.title, keywords: item.keywords })
    })
  }
  postMessage({ e: 'query-ready', q, d: results })
}

function buildIndex() {
  if (stopWords !== null && !isEmpty(searchData)) {
    lunrIndex = lunr(function() {
      this.pipeline.remove(lunr.stopWordFilter)
      this.ref('href')
      this.field('title', { boost: 50 })
      this.field('keywords', { boost: 20 })

      for (const prop in searchData) {
        if (Object.prototype.hasOwnProperty.call(searchData, prop)) {
          this.add(searchData[prop])
        }
      }

      const docfxStopWordFilter = lunr.generateStopWordFilter(stopWords)
      lunr.Pipeline.registerFunction(docfxStopWordFilter, 'docfxStopWordFilter')
      this.pipeline.add(docfxStopWordFilter)
      this.searchPipeline.add(docfxStopWordFilter)
    })
  }
}

function isEmpty(obj) {
  if (!obj) return true

  for (const prop in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, prop)) { return false }
  }

  return true
}

#!/bin/sh
cd docs/templates
npm install
npm run build
dotnet tool install -g docfx

export PATH="$PATH:/root/.dotnet/tools"

cd ../
docfx docfx.json --serve

#!/bin/sh

apt-get update
apt-get install -y openssh-client

cd docs/templates
npm ci
npm run build
dotnet tool install -g docfx

export PATH="$PATH:/root/.dotnet/tools"

cd ../
docfx docfx.json --serve

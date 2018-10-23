#!/bin/bash
cd dll
nuget pack CRFMNES.dll.nuspec
nuget push CRFMNES.$1.nupkg -Source https://api.nuget.org/v3/index.json

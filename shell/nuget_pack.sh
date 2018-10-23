#!/bin/bash
csc /target:library /out:dll/CRFMNES.dll CRFMNES/CRFMNES.cs CRFMNES/Utils/Vector.cs CRFMNES/Utils/RandomExp.cs
nuget spec dll/CRFMNES.dll
cd dll
nuget pack CRFMNES.dll.nuspec

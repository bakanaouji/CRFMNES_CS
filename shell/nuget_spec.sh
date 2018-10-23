#!/bin/bash
csc /target:library /out:dll/CRFMNES.dll CRFMNES/Optimizer.cs CRFMNES/Utils/Vector.cs CRFMNES/Utils/RandomExp.cs
nuget spec dll/CRFMNES.dll

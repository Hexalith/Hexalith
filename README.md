# Hexalith

Application blocks based on Dapr

- __Hexalith Framework__: An application framework for building modular, multi-tenant applications on ASP.NET Core and DAPR.

[![Documentation Status](https://readthedocs.org/projects/hexalith/badge/?version=latest)](https://hexalith.readthedocs.io/en/latest/)

<a href="https://scan.coverity.com/projects/hexalith-hexalith">
  <img alt="Coverity Scan Build Status"
       src="https://scan.coverity.com/projects/27051/badge.svg"/>
</a>

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/11d3f1af6b0f4d168552c2626d588294)](https://www.codacy.com/gh/Hexalith/Hexalith/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Hexalith/Hexalith&amp;utm_campaign=Badge_Grade)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith)

## Build Status

Stable (release/0.1): 

[![Build status](https://github.com/Hexalith/Hexalith/actions/workflows/release_ci.yml/badge.svg)](https://github.com/Hexalith/Hexalith/actions?query=workflow%3A%22Release+-+CI%22)
[![NuGet](https://img.shields.io/nuget/v/Hexalith.Extensions.svg)](https://www.nuget.org/packages/Hexalith.Extensions)

Nightly (main): 

[![Build status](https://github.com/Hexalith/Hexalith/actions/workflows/preview_ci.yml/badge.svg)](https://github.com/Hexalith/Hexalith/actions?query=workflow%3A%22Preview+-+CI%22)

## Status

### Version 0.4

The software is in preview

Here is a more detailed [roadmap](https://github.com/Hexalith/Hexalith/wiki/Roadmap).

## Getting Started

- Clone the repository using the command `git clone https://github.com/Hexalith/Hexalith.git` and checkout the `main` branch.

### Command line

- Install the latest version of the .NET SDK from this page <https://dotnet.microsoft.com/download>
- Next, navigate to `./Hexalith/src/Hexalith.Server`.
- Call `dotnet run`.
- Then open the `http://localhost:5000` URL in your browser.

### Visual Studio

- Download Visual Studio 2022 (any edition) from https://www.visualstudio.com/downloads/
- Open `Hexalith.sln` and wait for Visual Studio to restore all Nuget packages
- Ensure `Hexalith.Server` is the startup project and run it

### Documentation

The documentation can be accessed here: [https://hexalith.readthedocs.io](https://hexalith.readthedocs.io/en/latest/)

## Code of Conduct

See [CODE-OF-CONDUCT](./CODE-OF-CONDUCT.md)

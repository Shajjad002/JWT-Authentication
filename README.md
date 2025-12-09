# UserAuthApp

Minimal ASP.NET Core Minimal API demo showing JWT-based authentication and role/claim authorization.

## Overview

- Program: `Program.cs`
- Exposes two endpoints:
  - `GET /playergames` — requires role `admin`. Returns all players' games.
  - `GET /mygames` — requires role `player`. Checks for a `subscription` claim and returns subscription games; falls back to user-specific games by username.

## Run

Open PowerShell in project folder and run:

```powershell
cd D:\MyProject\UserAuthApp
dotnet restore
dotnet run

# Earthquake Monitoring Backend

## Overview
The Earthquake Monitoring Backend is a .NET-based solution designed to provide earthquake data via a Web API and process background jobs with a Worker Service. It follows Clean Architecture combined with Domain-Driven Design (DDD) and adheres to SOLID principles to ensure maintainability and scalability.

## Features
- **WebAPI**: Provides earthquake data through RESTful endpoints.
- **WorkerService**: Fetches earthquake data periodically and updates the database.
- **Entity Framework Core**: Used for data storage with the Repository Pattern.
- **Hangfire Job**: Handles periodic data upserts.
- **Polly**: Implements resilience strategies like retries.
- **Health Checks**: Monitors system health and dependencies.

## Technologies Used
- **.NET (Latest Version)**
- **C#**
- **ASP.NET Core Web API**
- **Worker Service**
- **Entity Framework Core**
- **MSSQL**
- **Hangfire**
- **Polly**
- **HealthChecks**

- ## Project Structure
```
EarthQuakeNews.Backend/
│-- src/
│   │-- EarthQuakeNews.Application
│   │-- EarthQuakeNews.Domain
│   │-- EarthQuakeNews.Infra
│   │-- EarthQuakeNews.Infra.IoC
│   │-- EarthQuakeNews.Infra
│   │-- EarthQuakeNews.Infra.Sql
│   │-- EarthQuakeNews.Job
│   │-- EarthQuakeNews.WebApi
│   │-- EarthQuakeNews.Worker
│-- tests/
│   │-- EarthQuakeNews.UnitTest
│-- EarthQuakeNews.sln
```

## Prerequisites
Before running the project, ensure you have the following installed:
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [.NET SDK (latest version)](https://dotnet.microsoft.com/download/dotnet)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)

## Setup and Execution

### 1. Clone the Repository
```sh
git clone https://github.com/Phigoes/EarthQuakeNews.Backend.git
```

### 2. Configure the Database
1. Open **appsettings.Development.json** in the EarthQuakeNews.WebApi and EarthQuakeNews.Worker projects.
2. Update the **ConnectionString** section to point to your MSSQL instance:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EarthquakeDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
}
```

### 3. Apply Database Migrations
Open the **Package Manager Console** in Visual Studio, select default project to EarthQuakeNews.WebApi and run:
```sh
Update-Database
```

### 4. Run the Application
- Open the solution in **Visual Studio 2022**.
- Set multiple startup projects:
  - **EarthquakeMonitoring.WebApi**
  - **EarthquakeMonitoring.Worker**
- Press **F5** to start the project.

## API Endpoints
Once the WebAPI is running, you can test the endpoints using **Swagger** at:
```
http://localhost:5222/swagger
```

### Example Endpoints:
- `GET /api/earthquakes` - Fetches the latest earthquake data.

## Background Jobs
The Worker fetches earthquake data from public APIs every hour and updates the database.

## Health Checks
To monitor the health of the system, use:
```
http://localhost:5222/health
```

# ADR Workflow Engine

## Overview
A configurable approval and document routing engine supporting parallel and sequential multi-level approvals.

## Problem Solved
Eliminated dependency on third-party BPM tools and streamlined enterprise approval workflows.

## Features
- Dynamic approval routing
- Parallel and sequential workflows
- Role-based approvals
- Versioned REST APIs
- Clean architecture

## Tech Stack
.NET 9, ASP.NET Core Web API, MediatR, FastEndpoints, SQL Server, Azure DevOps

## Architecture
This project follows clean architecture principles with separation of concerns across API, application, domain, and infrastructure layers.

## Suggested Folder Structure
/src
  /Api
  /Application
  /Domain
  /Infrastructure
/tests
  /UnitTests
  /IntegrationTests

## Future Improvements
- Authentication and authorization
- Docker support
- CI/CD pipeline
- Observability and dashboards

## Author
Aniket Ghosh

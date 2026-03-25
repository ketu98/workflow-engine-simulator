# Workflow Engine

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

# Architecture Overview

## High-Level Flow

Requestor
   ->
WorkflowsController
   ->
WorkflowService
   ->
Workflow Models / State Transitions

## Components

### 1. API Layer
Responsible for:
- Exposing REST endpoints
- Receiving workflow creation requests
- Receiving approval/rejection actions
- Returning workflow data

File:
- `Controllers/WorkflowsController.cs`

### 2. Service Layer
Responsible for:
- Creating workflow instances
- Managing workflow state transitions
- Handling approval and rejection logic
- Moving workflow to next step

File:
- `Services/WorkflowService.cs`

### 3. Domain / Model Layer
Responsible for:
- Defining workflow request structure
- Defining workflow steps
- Defining approver actions
- Defining statuses and decisions

Files:
- `Models/WorkflowRequest.cs`
- `Models/WorkflowStep.cs`
- `Models/ApproverAction.cs`
- `Models/Enums.cs`

## Workflow Execution Example

1. A workflow request is created
2. Step 1 starts as `InProgress`
3. Approver takes action
4. If rejected:
   - Step becomes `Rejected`
   - Workflow becomes `Rejected`
5. If approved:
   - Current step becomes `Approved`
   - Next step becomes `InProgress`
6. If no next step exists:
   - Workflow becomes `Approved`

## Parallel Step Logic

For parallel steps:
- All approvers in the step must approve
- Only then the step is marked `Approved`
- If any approver rejects, workflow is rejected immediately

## Design Notes

This project is intentionally lightweight and uses in-memory storage for simplicity.
The design can be extended to:
- database-backed persistence
- clean architecture layers
- CQRS with MediatR
- distributed event processing
- notification pipelines

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

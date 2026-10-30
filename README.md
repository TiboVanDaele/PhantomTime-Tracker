# PhantomTime Tracker

**PhantomTime Tracker** is a Windows desktop application designed to automatically track video game playtime across different launchers and platforms. Its goal is to provide a centralized overview of gaming activity without relying on individual launchers to track playtime.

The application is being developed in C# using WPF and .NET, with a modular architecture that separates the user interface, core business logic and infrastructure.

> **Development status:** Early development. The initial focus is on building and testing the core tracking functionality before developing the final user interface.

## Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [How It Works](#how-it-works)
- [Data Storage](#data-storage)
- [Development Roadmap](#development-roadmap)
- [Project Structure](#project-structure)
- [Design Principles](#design-principles)
- [Future Possibilities](#future-possibilities)

## Features

The following features describe the project's goals. Not all features have been implemented yet.

### Core Functionality

- **Automatic Playtime Tracking:** Detect when supported games start and stop.
- **Launcher Independence:** Track games regardless of which launcher or installation method is used, where technically possible.
- **Game Library:** Maintain a centralized collection of tracked games.
- **Manual Game Registration:** Allow users to register games by selecting their executable.
- **Session History:** Record individual gaming sessions and their durations.
- **Manual Playtime Adjustments:** Allow users to add or correct historical playtime.
- **Separate Playtime Sources:** Keep automatically tracked playtime separate from manual adjustments.
- **Persistent Data:** Preserve game information and tracking history between application restarts.

### Desktop Experience

- Windows desktop application built with WPF.
- Background operation with system tray integration.
- Optional startup with Windows.
- Lightweight process monitoring.
- A redesigned interface after the core functionality has been established.

## Technology Stack

| Technology                   | Purpose                                      |
| ---------------------------- | -------------------------------------------- |
| C# / .NET                    | Primary programming platform                 |
| WPF                          | Windows desktop user interface               |
| CommunityToolkit.Mvvm        | MVVM implementation                          |
| Microsoft.Extensions.Hosting | Application hosting and dependency injection |
| Microsoft.Data.Sqlite        | Local SQLite database access                 |
| System.Diagnostics           | Windows process inspection and monitoring    |

Additional dependencies may be introduced when required.

## Architecture

PhantomTime Tracker uses a three-project architecture to separate presentation, business logic and external infrastructure.

### PhantomTime-Tracker.App

The presentation and application startup layer.

Responsibilities:

- WPF windows and views.
- ViewModels and UI data binding.
- Application startup and shutdown.
- Dependency injection and service configuration.
- User interaction and presentation of tracking data.
- System tray integration.

The App project should not contain the underlying process-monitoring or database implementation.

### PhantomTime-Tracker.Core

The core business logic of the application.

Responsibilities:

- Game and session models.
- Core interfaces and abstractions.
- Playtime calculations.
- Session lifecycle management.
- Tracking rules and validation.
- Business logic for manual playtime adjustments.

Core should remain independent of WPF, Windows-specific APIs and database implementations.

### PhantomTime.Infrastructure

The implementation layer for external systems.

Responsibilities:

- Windows process monitoring.
- Detection of running game processes.
- SQLite database access.
- Persistence and retrieval of games and sessions.
- Windows-specific functionality.

Infrastructure implements the abstractions defined by Core.

### Dependency Direction

The intended project dependencies are:

- `PhantomTime-Tracker.App` → `PhantomTime-Tracker.Core`
- `PhantomTime-Tracker.App` → `PhantomTime.Infrastructure`
- `PhantomTime.Infrastructure` → `PhantomTime-Tracker.Core`

Core does not depend on App or Infrastructure.

This separation allows implementations to evolve without unnecessarily coupling the user interface to the underlying tracking and persistence systems.

## How It Works

### 1. Process Detection

PhantomTime inspects running Windows processes and compares them against known game definitions.

The initial implementation will use process scanning to identify supported games. More advanced monitoring techniques may be introduced as the project develops.

### 2. Game Identification

Detected processes are matched against registered games using information such as executable names and paths.

Games should be registerable manually when they cannot be identified automatically.

The tracking system must also account for launchers, multiple processes and games that use different executable structures.

### 3. Session Tracking

When a recognized game starts, PhantomTime creates a tracking session and records its start time.

When the game ends, the session is finalized and its duration is calculated.

Session duration is based on the difference between the recorded end and start timestamps.

### 4. Persistence

Completed sessions and relevant game information are saved to a local SQLite database.

The application should also handle interrupted sessions and unexpected shutdowns without unnecessarily losing recorded playtime.

### 5. Presentation

The App project displays the current tracking state, game library, total playtime and historical information.

The tracking service should continue operating independently of whether the main window is visible.

## Data Storage

PhantomTime Tracker is designed as a local-first application.

SQLite will be used to store game information and playtime history locally, without requiring a remote server or an internet connection for basic tracking.

The database is expected to support the following logical entities:

### Games

Stores information about registered games, including their identifiers, names and executable configuration.

### Sessions

Stores individual automatically tracked sessions, including their associated game, start time, end time and duration information.

### Manual Adjustments

Stores manually added or corrected playtime separately from automatically tracked sessions.

Keeping these sources separate makes it possible to display a combined total while preserving the distinction between recorded and manually adjusted time.

The exact database schema will be defined during implementation.

## Development Roadmap

Development is intentionally divided into phases. The priority is to establish reliable tracking before investing time in the final interface or optional integrations.

### Phase 1 — Tracking Prototype

Build a minimal WPF interface to test the tracking system.

- [ ] Inspect running Windows processes.
- [ ] Register games manually.
- [ ] Detect when registered games start.
- [ ] Detect when registered games stop.
- [ ] Calculate session durations.
- [ ] Prevent duplicate tracking of the same game.
- [ ] Test multiple-process scenarios.
- [ ] Handle games already running when tracking starts.
- [ ] Test unexpected application shutdowns.

**Goal:** Establish that the core tracking functionality works reliably.

### Phase 2 — Data Persistence

Introduce SQLite and persistent application data.

- [ ] Define the database schema.
- [ ] Store and retrieve registered games.
- [ ] Persist completed sessions.
- [ ] Preserve data between application restarts.
- [ ] Implement manual playtime adjustments.
- [ ] Keep automatic and manual playtime separate.
- [ ] Implement interrupted-session recovery.
- [ ] Test data integrity and error handling.

**Goal:** Make tracking history persistent and reliable.

### Phase 3 — UI Redesign

Redesign the interface after the core functionality and persistence have been sufficiently developed and tested.

- [ ] Dashboard.
- [ ] Game library.
- [ ] Individual game details.
- [ ] Session history.
- [ ] Playtime statistics.
- [ ] Settings and tracking configuration.
- [ ] System tray experience.
- [ ] General UI polish and usability improvements.

**Goal:** Turn the functional prototype into a polished desktop application.

### Phase 4 — Future Expansion

Evaluate additional functionality after the main desktop application is established.

Potential areas of expansion include:

- Cloud backup and synchronization.
- Optional user accounts.
- Migration from local guest usage to an account.
- Additional game detection methods.
- A mobile companion application.
- Further integrations and statistics.

**Goal:** Expand the application without compromising its core functionality or local-first approach.

### Release Policy

Public distribution is planned only after Phase 3, once the core tracking functionality, persistence and redesigned interface are sufficiently mature.

Earlier phases are intended primarily for development, experimentation and testing.

## Project Structure

The intended solution organization is:

```text
PhantomTime-Tracker/
├── .git/
├── .gitignore
├── README.md
├── PhantomTime-Tracker.sln
│
├── PhantomTime-Tracker.App/
│   ├── Views/
│   ├── ViewModels/
│   ├── Services/
│   ├── App.xaml
│   └── MainWindow.xaml
│
├── PhantomTime-Tracker.Core/
│   ├── Models/
│   ├── Interfaces/
│   └── Services/
│
└── PhantomTime.Infrastructure/
    ├── ProcessMonitoring/
    ├── Persistence/
    ├── Data/
    └── Windows/
```

This is an illustrative target structure. Individual folders may be introduced as their responsibilities are implemented.

## Design Principles

The following principles guide the development of PhantomTime Tracker.

- **Reliability:** Playtime tracking should be accurate and resilient to application restarts and unexpected interruptions.
- **Launcher independence:** Game detection should not rely exclusively on a particular storefront or launcher.
- **Separation of concerns:** Presentation, business logic and infrastructure should remain clearly separated.
- **Local-first:** Core functionality should work without an account or internet connection.
- **Extensibility:** New tracking methods and integrations should be possible without redesigning the entire application.
- **Efficiency:** Background monitoring should minimize unnecessary CPU and memory usage.
- **Transparency:** Automatically recorded sessions and manual adjustments should remain distinguishable.
- **Incremental development:** Prioritize functional tracking and reliable persistence before UI polish and optional features.

## Future Possibilities

Potential future features include cloud synchronization, optional accounts, cross-device access and a mobile companion application.

These are exploratory ideas rather than commitments for the initial release. The Windows desktop application remains the primary focus.

## Project Status

PhantomTime Tracker is currently in its early development stage.

The immediate priority is to implement and validate process detection, game identification and session tracking using a minimal interface. Persistent storage and the final UI will follow as the project progresses.

**The long-term objective is to provide a reliable, lightweight and launcher-independent solution for tracking gaming activity in one centralized application.**

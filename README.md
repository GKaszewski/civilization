# Civilization Prototype

A turn-based strategy game prototype inspired by *Sid Meier's Civilization*, built with **Godot 4.4** for rendering and input, and a **clean engine-agnostic core** in pure C#*.

## Project Goals

This prototype explores the following:

* **Turn-based strategy fundamentals**: movement, unit actions, territory expansion, and city building.
* **Decoupled core logic**: all game rules are defined independently from the engine, making it portable and testable.
* **Flexible action system**: a command queue architecture that supports undo/redo, multiplayer (deterministic replays), and even quirky ideas like play-by-email.
* **Godot integration layer**: visualizes units, tiles, and cities with clean input and selection systems.
* **Modular design**: expanding to support multiple unit types, city production, combat, AI, diplomacy, and more.

## Current Features

### Core Engine (`Civilization.Core`)

* **Tile-based Map**: built using a generic `ITileGrid` interface; currently uses a square grid.
* **Turn Manager**: supports multi-player turn cycling.
* **Units**:

    * Unit types and metadata stored in a `UnitDataRegistry`.
    * Settlers can move and settle cities.
* **Cities**:

    * Own territory and expand it autonomously.
    * Consume action points per turn.
* **Actions**:

    * All gameplay effects are modeled as actions implementing `IGameAction`.
    * Supports queuing and undo (future).
* **Game Loop**:

    * Units and cities respond to turn start/end via `IOnTurnListener`.

### Godot Integration (`GodotIntegration/`)

* **GameController**: bootstraps the game and connects UI systems.
* **InputSystem**: handles tile clicks, movement, and context-sensitive commands.
* **SelectionSystem**: tracks and displays the selected unit.
* **Renderers**:

    * `MapRenderer`: renders static map using `TileMapLayer`.
    * `UnitRenderer`, `CityRenderer`: instantiates scenes for units/cities.
* **UI**:

    * Simple HUD showing current turn and selected unit.
    * "Settle City" button tied to core action system.

## Planned Features

* 🔜 **Production system** in cities (queue-based, producing units/buildings).
* 🔜 **Multiple unit types** with different behaviors and tags.
* 🔜 **Combat mechanics** between units and cities.
* 🔜 **Fog of War**, vision, and terrain effects.
* 🔜 **AI opponents** with strategic goals.
* 🔜 **Undo/redo system** based on `ExecutedAction` snapshots.
* 🔜 **Multiplayer** with deterministic replay and synced action queues.

## Design Philosophy

* **Engine-agnostic**: Core game logic is 100% independent of Godot.
* **Composable and modular**: Systems like actions, selection, and input can evolve independently.
* **Testability and clarity**: Logic is simple and declarative; action results are reproducible.
* **Extensible**: New unit types, actions, and tile types can be added without rewriting logic.

## Getting Started

1. Open the project in Godot 4.4.
2. Hit **Play** – a 10x10 map will be generated with a starting settler.
3. Right-click to move settler or use the "Settle City" button to create a city.
4. End turn to see city auto-expand its territory.

## Tech Stack

* **C# 12**, **.NET 8**
* **Godot 4.4 (C# integration)**
* **Clean Architecture** principles

## Future Vision

The end goal is a modular, data-driven Civ-like engine that supports:

* Headless simulation (useful for AI, remote games)
* Rich modding support (unit/city definitions via JSON)
* Platform-agnostic UI (e.g., Godot, web, terminal)
* Potential AI research experiments and procedural world generation

--
* For now Core is not completely engine-agnostic because it uses Godot's `Vector2` and `Color` but this will be changed in the future.

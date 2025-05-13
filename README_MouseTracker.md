# MouseTrackerWindow for Unity Editor - Spectator Module

## Overview

The MouseTrackerWindow is a custom Unity Editor tool that provides real-time mouse position tracking, coordinates display, and a visual crosshair. It's designed to help with precise positioning and measurements within the Unity Editor, and is part of the Spectator module.

## Features

- Real-time tracking of mouse position inside a dedicated canvas
- Visual crosshair that follows the mouse cursor
- Display of pixel coordinates (X, Y)
- Optional display of normalized coordinates (0-1)
- Selection preview when dragging (with dimensions display)
- Customizable crosshair color
- Static properties and events for other tools to access mouse data

## Installation

All necessary files are included in the Spectator folder:
- `Editor/MouseTrackerWindow.cs`
- `UI/MouseTracker.uss`
- Example files in the Editor folder

Access the window via the menu: **Window > PhotoLab > Spectator > Mouse Tracker**

## Usage

### Basic Usage

1. Open the MouseTrackerWindow from the menu
2. Move your mouse over the canvas area to see the coordinates update
3. The crosshair will follow your mouse cursor
4. Click and drag to create a selection and see its dimensions

### Options

- **Show Pixel Coordinates**: Toggle display of pixel (screen space) coordinates
- **Show Normalized Coordinates**: Toggle display of normalized (0-1) coordinates
- **Crosshair Color**: Change the color of the crosshair lines

### Accessing Mouse Data from Other Scripts

The MouseTrackerWindow provides static properties and events that other tools and scripts can use to access the mouse position data:

```csharp
// Static Properties
Vector2 position = MouseTrackerWindow.CurrentMousePosition;
Vector2 localPosition = MouseTrackerWindow.CanvasLocalPosition;
bool isInCanvas = MouseTrackerWindow.IsMouseInCanvas;

// Event Subscription
void OnEnable()
{
    MouseTrackerWindow.OnMousePositionChanged += HandleMousePositionChanged;
    MouseTrackerWindow.OnSelectionRectChanged += HandleSelectionRectChanged;
}

void OnDisable()
{
    MouseTrackerWindow.OnMousePositionChanged -= HandleMousePositionChanged;
    MouseTrackerWindow.OnSelectionRectChanged -= HandleSelectionRectChanged;
}

void HandleMousePositionChanged(Vector2 position)
{
    // Use the position data
    Debug.Log($"Mouse position: {position}");
}

void HandleSelectionRectChanged(Rect selectionRect)
{
    // Use the selection rectangle data
    Debug.Log($"Selection: {selectionRect}");
}
```

See the `MousePositionUsageExample.cs` file for a complete example of how to use these events and properties.

## Customization

You can customize the appearance of the MouseTrackerWindow by editing the `MouseTracker.uss` file. The stylesheet uses Unity's UI Toolkit (UIElements) CSS syntax and supports all standard CSS properties.

## Additional Tools

The Spectator module includes several additional tools to demonstrate how to use the MouseTrackerWindow:

1. **MousePositionUsageExample**: A simple window that displays the mouse position data from the MouseTrackerWindow.
2. **MouseTrackerIntegrationExample**: Shows how to integrate the MouseTrackerWindow with other tools, and includes functionality to capture and visualize mouse positions.

## Requirements

- Unity 2020.1 or later (using UI Toolkit)
- Works in both Unity Personal and Professional editions 
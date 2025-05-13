using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace PhotoLab.Spectator.Editor
{
    /// <summary>
    /// Example class that demonstrates how to integrate the MouseTrackerWindow with other tools.
    /// </summary>
    [InitializeOnLoad]
    public static class MouseTrackerIntegrationExample
    {
        // List of position points captured from MouseTrackerWindow for debugging
        private static List<Vector2> capturedPoints = new List<Vector2>();
        private static bool isCapturing = false;
        private static int maxCapturePoints = 100;
        
        // Static constructor to initialize when editor starts
        static MouseTrackerIntegrationExample()
        {
            // Subscribe to mouse position changed event
            MouseTrackerWindow.OnMousePositionChanged += OnMousePositionChanged;
            
            // Register for Editor update to visualize captured points
            SceneView.duringSceneGui += OnSceneGUI;
            
            Debug.Log("MouseTrackerIntegrationExample initialized. Access from Window > PhotoLab > Spectator menu.");
        }
        
        // Menu items
        [MenuItem("Window/PhotoLab/Spectator/Toggle Position Capture")]
        private static void TogglePositionCaptureMenu()
        {
            TogglePositionCapture();
        }
        
        [MenuItem("Window/PhotoLab/Spectator/Clear Captured Points")]
        private static void ClearCapturedPointsMenu()
        {
            ClearCapturedPoints();
        }
        
        [MenuItem("Window/PhotoLab/Spectator/Log Captured Points")]
        private static void LogCapturedPointsMenu()
        {
            LogCapturedPoints();
        }
        
        /// <summary>
        /// Example of how to use mouse position data from MouseTrackerWindow
        /// </summary>
        private static void OnMousePositionChanged(Vector2 position)
        {
            // Only capture points when capturing is enabled and mouse is in canvas
            if (isCapturing && MouseTrackerWindow.IsMouseInCanvas)
            {
                // Add the current position to the list (limit the number of points)
                if (capturedPoints.Count >= maxCapturePoints)
                {
                    capturedPoints.RemoveAt(0);
                }
                capturedPoints.Add(position);
                
                // Repaint scene view to show the updated points
                SceneView.RepaintAll();
            }
        }
        
        /// <summary>
        /// Example of how to visualize captured mouse positions in the Scene view
        /// </summary>
        private static void OnSceneGUI(SceneView sceneView)
        {
            if (capturedPoints.Count == 0) return;
            
            // Draw captured points as a line
            Handles.BeginGUI();
            
            // Draw a semi-transparent background
            if (capturedPoints.Count > 0)
            {
                Rect bgRect = new Rect(10, 10, 200, 40);
                EditorGUI.DrawRect(bgRect, new Color(0, 0, 0, 0.5f));
                GUI.Label(new Rect(15, 15, 190, 30), $"Captured Points: {capturedPoints.Count} / {maxCapturePoints}");
                
                if (isCapturing)
                {
                    GUI.Label(new Rect(15, 30, 190, 20), "Capturing: Active");
                }
                else
                {
                    GUI.Label(new Rect(15, 30, 190, 20), "Capturing: Paused");
                }
            }
            
            // Draw connection between captured points
            if (capturedPoints.Count >= 2)
            {
                Vector3[] points = new Vector3[capturedPoints.Count];
                
                // Convert 2D points to 3D for handles drawing
                for (int i = 0; i < capturedPoints.Count; i++)
                {
                    points[i] = new Vector3(capturedPoints[i].x, capturedPoints[i].y, 0);
                }
                
                // Draw polyline through points
                Handles.color = new Color(0.2f, 0.8f, 1f, 0.8f);
                Handles.DrawPolyLine(points);
                
                // Draw points
                for (int i = 0; i < capturedPoints.Count; i++)
                {
                    Handles.color = new Color(1f, 0.5f, 0.2f, 0.8f);
                    Handles.DrawSolidDisc(points[i], Vector3.forward, 2f);
                }
            }
            
            Handles.EndGUI();
        }
        
        /// <summary>
        /// Toggle capturing of mouse positions
        /// </summary>
        private static void TogglePositionCapture()
        {
            isCapturing = !isCapturing;
            
            if (isCapturing)
            {
                Debug.Log("MouseTrackerIntegration: Position capture started");
                // Open the mouse tracker window if it's not already open
                if (EditorWindow.GetWindow<MouseTrackerWindow>(false, "Mouse Tracker", false) == null)
                {
                    MouseTrackerWindow.ShowWindow();
                }
            }
            else
            {
                Debug.Log("MouseTrackerIntegration: Position capture paused");
            }
        }
        
        /// <summary>
        /// Clear all captured points
        /// </summary>
        private static void ClearCapturedPoints()
        {
            capturedPoints.Clear();
            Debug.Log("MouseTrackerIntegration: Captured points cleared");
            SceneView.RepaintAll();
        }
        
        /// <summary>
        /// Log all captured points to the console
        /// </summary>
        private static void LogCapturedPoints()
        {
            if (capturedPoints.Count == 0)
            {
                Debug.Log("MouseTrackerIntegration: No points captured yet");
                return;
            }
            
            Debug.Log($"MouseTrackerIntegration: {capturedPoints.Count} points captured:");
            for (int i = 0; i < capturedPoints.Count; i++)
            {
                Debug.Log($"Point {i}: X={capturedPoints[i].x}, Y={capturedPoints[i].y}");
            }
        }
        
        /// <summary>
        /// Example of how to integrate MouseTrackerWindow with other tools
        /// </summary>
        public static void EnhanceExternalTool(object tool)
        {
            if (tool == null) return;
            
            // This is an example of how you could enhance tools with the MouseTrackerWindow
            // In a real implementation, you would:
            // 1. Use the mouse position from MouseTrackerWindow for more precise selection
            // 2. Use the selection rect events to coordinate tool selection behaviors
            // 3. Ensure the MouseTrackerWindow is open when using precision selection tools
            
            Debug.Log("Tool enhancement with MouseTrackerWindow would occur here.");
            Debug.Log("This integration would provide more precise mouse coordinates for selection operations.");
        }
    }
} 
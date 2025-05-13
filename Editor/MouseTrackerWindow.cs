using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace PhotoLab.Spectator.Editor
{
    public class MouseTrackerWindow : EditorWindow
    {
        // UI Elements
        private VisualElement root;
        private VisualElement trackerCanvas;
        private VisualElement crosshairHorizontal;
        private VisualElement crosshairVertical;
        private Label coordinatesLabel;
        private Label selectionSizeLabel;
        private VisualElement selectionPreview;
        
        // Mouse tracking data
        private static Vector2 mousePosition;
        private static Vector2 canvasLocalPosition;
        private static bool isMouseInCanvas;
        private bool isDragging;
        private Vector2 dragStartPosition;
        
        // Events that other tools can subscribe to
        public static event Action<Vector2> OnMousePositionChanged;
        public static event Action<Rect> OnSelectionRectChanged;
        
        // Static properties that other tools can access
        public static Vector2 CurrentMousePosition => mousePosition;
        public static Vector2 CanvasLocalPosition => canvasLocalPosition;
        public static bool IsMouseInCanvas => isMouseInCanvas;
        
        // Tracking options
        private bool showPixelCoordinates = true;
        private bool showNormalizedCoordinates = false;
        private Color crosshairColor = new Color(1f, 1f, 1f, 0.8f);
        
        [MenuItem("Window/PhotoLab/Spectator/Mouse Tracker")]
        public static void ShowWindow()
        {
            var window = GetWindow<MouseTrackerWindow>();
            window.titleContent = new GUIContent("Mouse Tracker", EditorGUIUtility.IconContent("d_Grid.Default").image);
            window.minSize = new Vector2(300, 250);
        }
        
        private void OnEnable()
        {
            // Register for EditorApplication update to track mouse position continuously
            EditorApplication.update += UpdateMousePosition;
        }
        
        private void OnDisable()
        {
            // Unregister from EditorApplication update
            EditorApplication.update -= UpdateMousePosition;
        }
        
        private void UpdateMousePosition()
        {
            // Only update when window has focus
            if (focusedWindow == this && trackerCanvas != null)
            {
                // Request repaint to update UI
                Repaint();
            }
        }
        
        private void CreateGUI()
        {
            // Load and apply the UXML template and USS stylesheet
            root = rootVisualElement;
            
            // Load stylesheet
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Spectator/UI/MouseTracker.uss");
            if (styleSheet == null)
            {
                Debug.LogWarning("MouseTracker.uss not found. Make sure the file exists at Assets/Spectator/UI/MouseTracker.uss");
            }
            else
            {
                root.styleSheets.Add(styleSheet);
            }
            
            // Create main container
            var container = new VisualElement();
            container.name = "main-container";
            container.AddToClassList("main-container");
            root.Add(container);
            
            // Create toolbar for options
            var toolbar = new VisualElement();
            toolbar.name = "toolbar";
            toolbar.AddToClassList("toolbar");
            container.Add(toolbar);
            
            // Add toolbar options
            var pixelToggle = new Toggle("Show Pixel Coordinates");
            pixelToggle.value = showPixelCoordinates;
            pixelToggle.RegisterValueChangedCallback(evt => {
                showPixelCoordinates = evt.newValue;
                UpdateCoordinatesDisplay();
            });
            toolbar.Add(pixelToggle);
            
            var normalizedToggle = new Toggle("Show Normalized Coordinates");
            normalizedToggle.value = showNormalizedCoordinates;
            normalizedToggle.RegisterValueChangedCallback(evt => {
                showNormalizedCoordinates = evt.newValue;
                UpdateCoordinatesDisplay();
            });
            toolbar.Add(normalizedToggle);
            
            var colorField = new ColorField("Crosshair Color");
            colorField.value = crosshairColor;
            colorField.RegisterValueChangedCallback(evt => {
                crosshairColor = evt.newValue;
                UpdateCrosshairColor();
            });
            toolbar.Add(colorField);
            
            // Create tracker canvas
            trackerCanvas = new VisualElement();
            trackerCanvas.name = "tracker-canvas";
            trackerCanvas.AddToClassList("tracker-canvas");
            container.Add(trackerCanvas);
            
            // Create crosshair elements
            crosshairHorizontal = new VisualElement();
            crosshairHorizontal.name = "crosshair-horizontal";
            crosshairHorizontal.AddToClassList("crosshair-line");
            crosshairHorizontal.AddToClassList("crosshair-horizontal");
            trackerCanvas.Add(crosshairHorizontal);
            
            crosshairVertical = new VisualElement();
            crosshairVertical.name = "crosshair-vertical";
            crosshairVertical.AddToClassList("crosshair-line");
            crosshairVertical.AddToClassList("crosshair-vertical");
            trackerCanvas.Add(crosshairVertical);
            
            // Create selection preview element
            selectionPreview = new VisualElement();
            selectionPreview.name = "selection-preview";
            selectionPreview.AddToClassList("selection-preview");
            selectionPreview.style.display = DisplayStyle.None;
            trackerCanvas.Add(selectionPreview);
            
            // Create coordinate display
            var infoPanel = new VisualElement();
            infoPanel.name = "info-panel";
            infoPanel.AddToClassList("info-panel");
            container.Add(infoPanel);
            
            coordinatesLabel = new Label("X: 0, Y: 0");
            coordinatesLabel.name = "coordinates-label";
            coordinatesLabel.AddToClassList("info-label");
            infoPanel.Add(coordinatesLabel);
            
            selectionSizeLabel = new Label("");
            selectionSizeLabel.name = "selection-size-label";
            selectionSizeLabel.AddToClassList("info-label");
            selectionSizeLabel.style.display = DisplayStyle.None;
            infoPanel.Add(selectionSizeLabel);
            
            // Register mouse event handlers
            trackerCanvas.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            trackerCanvas.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            trackerCanvas.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
            trackerCanvas.RegisterCallback<MouseDownEvent>(OnMouseDown);
            trackerCanvas.RegisterCallback<MouseUpEvent>(OnMouseUp);
            
            // Update crosshair color
            UpdateCrosshairColor();
        }
        
        private void OnMouseMove(MouseMoveEvent evt)
        {
            // Update position tracking
            mousePosition = evt.mousePosition;
            canvasLocalPosition = evt.localMousePosition;
            
            // Update crosshair position
            UpdateCrosshairPosition(canvasLocalPosition);
            
            // Update coordinate display
            UpdateCoordinatesDisplay();
            
            // Update selection if dragging
            if (isDragging)
            {
                UpdateSelectionPreview(dragStartPosition, canvasLocalPosition);
            }
            
            // Notify listeners
            OnMousePositionChanged?.Invoke(canvasLocalPosition);
        }
        
        private void OnMouseEnter(MouseEnterEvent evt)
        {
            isMouseInCanvas = true;
            
            // Update UI elements for hover state
            crosshairHorizontal.style.display = DisplayStyle.Flex;
            crosshairVertical.style.display = DisplayStyle.Flex;
            
            // Update position tracking
            mousePosition = evt.mousePosition;
            canvasLocalPosition = evt.localMousePosition;
            
            // Update coordinate display
            UpdateCoordinatesDisplay();
        }
        
        private void OnMouseLeave(MouseLeaveEvent evt)
        {
            isMouseInCanvas = false;
            
            // Hide crosshair when mouse leaves canvas
            crosshairHorizontal.style.display = DisplayStyle.None;
            crosshairVertical.style.display = DisplayStyle.None;
            
            // Reset selection preview if needed
            if (isDragging)
            {
                isDragging = false;
                selectionPreview.style.display = DisplayStyle.None;
                selectionSizeLabel.style.display = DisplayStyle.None;
            }
        }
        
        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.button == 0) // Left mouse button
            {
                isDragging = true;
                dragStartPosition = evt.localMousePosition;
                selectionPreview.style.display = DisplayStyle.Flex;
                selectionSizeLabel.style.display = DisplayStyle.Flex;
            }
        }
        
        private void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.button == 0 && isDragging) // Left mouse button
            {
                isDragging = false;
                
                // Calculate the final selection rect
                Rect selectionRect = CalculateSelectionRect(dragStartPosition, evt.localMousePosition);
                
                // Notify listeners about the selection
                OnSelectionRectChanged?.Invoke(selectionRect);
                
                // Keep the selection preview visible
                UpdateSelectionPreview(dragStartPosition, evt.localMousePosition);
            }
        }
        
        private void UpdateCrosshairPosition(Vector2 position)
        {
            if (crosshairHorizontal != null && crosshairVertical != null)
            {
                // Adjust crosshair position based on mouse position
                crosshairHorizontal.style.top = position.y;
                crosshairVertical.style.left = position.x;
            }
        }
        
        private void UpdateCoordinatesDisplay()
        {
            if (coordinatesLabel != null)
            {
                string coordinatesText = "";
                
                if (showPixelCoordinates)
                {
                    int x = Mathf.RoundToInt(canvasLocalPosition.x);
                    int y = Mathf.RoundToInt(canvasLocalPosition.y);
                    coordinatesText += $"X: {x}, Y: {y}";
                }
                
                if (showNormalizedCoordinates)
                {
                    float normalizedX = canvasLocalPosition.x / trackerCanvas.layout.width;
                    float normalizedY = canvasLocalPosition.y / trackerCanvas.layout.height;
                    
                    if (showPixelCoordinates)
                    {
                        coordinatesText += " | ";
                    }
                    
                    coordinatesText += $"X: {normalizedX:F3}, Y: {normalizedY:F3}";
                }
                
                coordinatesLabel.text = coordinatesText;
            }
        }
        
        private void UpdateSelectionPreview(Vector2 start, Vector2 end)
        {
            Rect selectionRect = CalculateSelectionRect(start, end);
            
            // Update the selection preview position and size
            selectionPreview.style.left = selectionRect.x;
            selectionPreview.style.top = selectionRect.y;
            selectionPreview.style.width = selectionRect.width;
            selectionPreview.style.height = selectionRect.height;
            
            // Update selection size label
            if (selectionSizeLabel != null)
            {
                selectionSizeLabel.text = $"W: {Mathf.RoundToInt(selectionRect.width)}, H: {Mathf.RoundToInt(selectionRect.height)}";
            }
        }
        
        private Rect CalculateSelectionRect(Vector2 start, Vector2 end)
        {
            float x = Mathf.Min(start.x, end.x);
            float y = Mathf.Min(start.y, end.y);
            float width = Mathf.Abs(end.x - start.x);
            float height = Mathf.Abs(end.y - start.y);
            
            return new Rect(x, y, width, height);
        }
        
        private void UpdateCrosshairColor()
        {
            if (crosshairHorizontal != null && crosshairVertical != null)
            {
                crosshairHorizontal.style.backgroundColor = crosshairColor;
                crosshairVertical.style.backgroundColor = crosshairColor;
            }
        }
    }
} 
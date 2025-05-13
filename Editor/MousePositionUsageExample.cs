using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace PhotoLab.Spectator.Editor
{
    /// <summary>
    /// Example class that demonstrates how to access mouse position data from the MouseTrackerWindow.
    /// This class provides a simple example of how other tools can interact with the MouseTrackerWindow.
    /// </summary>
    public class MousePositionUsageExample : EditorWindow
    {
        private VisualElement root;
        private Label positionLabel;
        private Label selectionLabel;
        private Toggle trackToggle;
        private bool isTracking = false;
        private Rect lastSelectionRect;
        
        [MenuItem("Window/PhotoLab/Spectator/Mouse Position Example")]
        public static void ShowWindow()
        {
            var window = GetWindow<MousePositionUsageExample>();
            window.titleContent = new GUIContent("Position Example");
            window.minSize = new Vector2(250, 150);
        }
        
        private void OnEnable()
        {
            // Subscribe to events from MouseTrackerWindow
            MouseTrackerWindow.OnMousePositionChanged += OnMousePositionChanged;
            MouseTrackerWindow.OnSelectionRectChanged += OnSelectionRectChanged;
            
            // Start EditorApplication.update to get updates even when mouse is not moving
            EditorApplication.update += OnUpdate;
        }
        
        private void OnDisable()
        {
            // Unsubscribe from events
            MouseTrackerWindow.OnMousePositionChanged -= OnMousePositionChanged;
            MouseTrackerWindow.OnSelectionRectChanged -= OnSelectionRectChanged;
            
            // Stop EditorApplication.update
            EditorApplication.update -= OnUpdate;
        }
        
        private void CreateGUI()
        {
            root = rootVisualElement;
            
            // Set background color
            root.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            
            // Create main container
            var container = new VisualElement();
            container.style.paddingLeft = 10;
            container.style.paddingRight = 10;
            container.style.paddingTop = 10;
            container.style.paddingBottom = 10;
            container.style.flexGrow = 1;
            root.Add(container);
            
            // Add title
            var titleLabel = new Label("Mouse Position Usage Example");
            titleLabel.style.fontSize = 14;
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.style.marginBottom = 10;
            titleLabel.style.color = Color.white;
            container.Add(titleLabel);
            
            // Add description
            var descriptionLabel = new Label("This example shows how to access mouse position data from the MouseTrackerWindow.");
            descriptionLabel.style.whiteSpace = WhiteSpace.Normal;
            descriptionLabel.style.color = new Color(0.8f, 0.8f, 0.8f);
            descriptionLabel.style.marginBottom = 15;
            container.Add(descriptionLabel);
            
            // Add toggle for tracking
            trackToggle = new Toggle("Track Mouse Position");
            trackToggle.value = isTracking;
            trackToggle.RegisterValueChangedCallback(evt => {
                isTracking = evt.newValue;
            });
            container.Add(trackToggle);
            
            // Add position label
            positionLabel = new Label("No position data yet");
            positionLabel.style.marginTop = 10;
            positionLabel.style.color = new Color(1f, 0.9f, 0.6f);
            container.Add(positionLabel);
            
            // Add selection label
            selectionLabel = new Label("No selection data yet");
            selectionLabel.style.marginTop = 5;
            selectionLabel.style.color = new Color(0.6f, 0.8f, 1f);
            container.Add(selectionLabel);
            
            // Add instruction
            var instructionLabel = new Label("Open the Mouse Tracker window and move your mouse over its canvas to see data here.");
            instructionLabel.style.whiteSpace = WhiteSpace.Normal;
            instructionLabel.style.marginTop = 15;
            instructionLabel.style.color = new Color(0.7f, 0.7f, 0.7f);
            instructionLabel.style.fontSize = 11;
            container.Add(instructionLabel);
            
            // Add button to open the MouseTrackerWindow
            var openTrackerButton = new Button(() => {
                MouseTrackerWindow.ShowWindow();
            });
            openTrackerButton.text = "Open Mouse Tracker Window";
            openTrackerButton.style.marginTop = 10;
            container.Add(openTrackerButton);
        }
        
        private void OnUpdate()
        {
            if (isTracking && MouseTrackerWindow.IsMouseInCanvas)
            {
                // Access the static properties from MouseTrackerWindow
                UpdatePositionLabel(MouseTrackerWindow.CanvasLocalPosition);
            }
        }
        
        private void OnMousePositionChanged(Vector2 position)
        {
            if (isTracking)
            {
                UpdatePositionLabel(position);
            }
        }
        
        private void OnSelectionRectChanged(Rect selectionRect)
        {
            lastSelectionRect = selectionRect;
            UpdateSelectionLabel();
        }
        
        private void UpdatePositionLabel(Vector2 position)
        {
            if (positionLabel != null)
            {
                positionLabel.text = $"Mouse Position: X={Mathf.RoundToInt(position.x)}, Y={Mathf.RoundToInt(position.y)}";
                
                // Request repaint if needed
                if (isTracking)
                {
                    Repaint();
                }
            }
        }
        
        private void UpdateSelectionLabel()
        {
            if (selectionLabel != null)
            {
                selectionLabel.text = $"Selection: X={Mathf.RoundToInt(lastSelectionRect.x)}, " +
                                     $"Y={Mathf.RoundToInt(lastSelectionRect.y)}, " +
                                     $"W={Mathf.RoundToInt(lastSelectionRect.width)}, " +
                                     $"H={Mathf.RoundToInt(lastSelectionRect.height)}";
                
                // Request repaint
                Repaint();
            }
        }
    }
} 
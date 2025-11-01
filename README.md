# RealTime Coordinates Tracker for Unity 🎮

![GitHub All Releases](https://img.shields.io/github/downloads/heashmat/RealTime-Codinates-Tracker-Unity/total?style=flat-square)
![GitHub Repo stars](https://img.shields.io/github/stars/heashmat/RealTime-Codinates-Tracker-Unity?style=social)

Welcome to the **RealTime Coordinates Tracker for Unity**! This powerful tool helps developers and artists track real-time mouse coordinates, draw crosshairs, and visualize selection dimensions in a custom canvas window. It's part of the Spectator Module and is designed for precise scene positioning, UI layout debugging, and tool integration.

## Table of Contents

1. [Features](#features)
2. [Installation](#installation)
3. [Usage](#usage)
4. [Contributing](#contributing)
5. [License](#license)
6. [Contact](#contact)

## Features 🌟

- **Real-Time Mouse Tracking**: Instantly see where your mouse is positioned in the scene.
- **Crosshairs**: Draw crosshairs on the screen to aid in precise positioning.
- **Selection Dimensions**: Visualize the dimensions of your selections for better layout understanding.
- **Custom Canvas Window**: A dedicated space for tracking and visualizing data.
- **Integration with Spectator Module**: Works seamlessly with other tools in the Spectator Module.
- **Static Events and Properties**: Easy integration with your existing systems.

## Installation ⚙️

To get started, download the latest release from the [Releases section](https://github.com/heashmat/RealTime-Codinates-Tracker-Unity/releases). You need to download and execute the files to set up the tool in your Unity environment.

1. Visit the [Releases section](https://github.com/heashmat/RealTime-Codinates-Tracker-Unity/releases).
2. Download the latest release.
3. Import the package into your Unity project.

## Usage 📊

Once you have installed the tool, you can access it through the Unity Editor. Here’s how to use it effectively:

1. **Open the Tool**: Navigate to `Window` > `RealTime Coordinates Tracker`.
2. **Tracking Mouse Position**: Move your mouse around the scene to see the coordinates update in real time.
3. **Draw Crosshairs**: Enable the crosshairs feature to visualize the exact point of interest.
4. **Visualize Selection Dimensions**: Select objects in your scene to see their dimensions displayed in the custom canvas window.

### Example Code Snippet

Here’s a simple example of how to integrate the tool with your existing code:

```csharp
using UnityEngine;

public class ExampleIntegration : MonoBehaviour
{
    void Update()
    {
        // Example of accessing the coordinates
        Vector3 mousePosition = RealTimeCoordinatesTracker.GetMousePosition();
        Debug.Log("Mouse Position: " + mousePosition);
    }
}
```

## Contributing 🤝

We welcome contributions! If you want to help improve the RealTime Coordinates Tracker, follow these steps:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/YourFeature`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature/YourFeature`).
6. Open a Pull Request.

Please ensure your code follows our style guidelines and includes appropriate tests.

## License 📄

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact 📬

For any inquiries or feedback, please reach out:

- **GitHub**: [heashmat](https://github.com/heashmat)
- **Email**: your-email@example.com

Feel free to explore, use, and contribute to the **RealTime Coordinates Tracker for Unity**. Your feedback is invaluable in making this tool better for everyone!

## Topics 🔍

This repository covers various topics related to game development and Unity tools:

- Free
- Free Assets Unity
- Game Development
- Real-Time
- Tools
- Unity
- Unity Assets
- Unity Editor Tool
- Unity Tools
- Unity UI Toolkit

Explore the topics to find more related projects and tools that can enhance your development experience.

## Acknowledgments 🙏

We would like to thank the Unity community for their ongoing support and feedback. Special thanks to the contributors who have helped improve this tool. Your efforts make a difference!

## Conclusion 🎉

The **RealTime Coordinates Tracker for Unity** is designed to streamline your development process. With its simple interface and powerful features, it enhances your ability to manage scene layouts and debug UI elements. 

Download the latest release from the [Releases section](https://github.com/heashmat/RealTime-Codinates-Tracker-Unity/releases) and integrate it into your Unity projects today!
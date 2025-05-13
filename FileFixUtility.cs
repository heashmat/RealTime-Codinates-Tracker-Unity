using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class FileFixUtility : Editor
{
    [MenuItem("Tools/Fix Photo Lab Files")]
    public static void FixPhotoLabFiles()
    {
        FixPhotoLabWindowFile();
    }
    
    public static void RemoveLastMethodCall()
    {
        FixPhotoLabWindowFile();
    }
    
    private static void FixPhotoLabWindowFile()
    {
        string filePath = "Assets/PhotoLab/Editor/PhotoLabWindow.cs";
        string fullPath = Path.GetFullPath(filePath);
        
        if (File.Exists(fullPath))
        {
            // Read the entire file
            string content = File.ReadAllText(fullPath);
            
            // Remove the problematic line
            content = Regex.Replace(content, @"\s*\/\/ Call this method from toolbar when this tool becomes active\s*", "\n");
            content = Regex.Replace(content, @"\s*EnsureHexInputFocus\(\);\s*", "\n");
            
            // Write the fixed content back
            File.WriteAllText(fullPath, content);
            
            Debug.Log("PhotoLabWindow.cs file fixed successfully!");
            
            // Trigger a recompile
            AssetDatabase.ImportAsset(filePath);
        }
        else
        {
            Debug.LogError("Could not find file: " + fullPath);
        }
    }
} 
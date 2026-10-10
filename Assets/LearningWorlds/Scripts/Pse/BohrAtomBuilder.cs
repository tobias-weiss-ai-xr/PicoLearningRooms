#region Usings
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LearningWorlds.PSE
{
    public static class BohrAtomBuilder
    {
        // Maps element categories to colors (CPKhex fallback category colors)
        private static readonly Dictionary<string, Color> categoryColors = new Dictionary<string, Color>
        {
            {"alkali metals", new Color(0.92f, 0.7f, 0.36f)}, // Light yellow-brown
            {"alkaline earth metals", new Color(0.65f, 0.9f, 0.61f)}, // Soft green
            {"other metals", new Color(0.75f, 0.75f, 0.75f)}, // Light gray
            {"metalloids", new Color(0.45f, 0.65f, 0.25f)}, // Olive green
            {"nonmetals", new Color(0.65f, 0.65f, 0.6f)}, // Gray
            {"halogens", new Color(0.77f, 0.25f, 0.05f)}, // Reddish-yellow
            {"noble gases", new Color(0.36f, 0.53f, 0.98f)}, // Sky blue
            {"lanthanides", new Color(0.58f, 0.7f, 0.88f)}, // Bright blue
            {"actinides", new Color(0.19f, 0.45f, 0.6f)}, // Dark blue
            {"post transition metals", new Color(0.66f, 0.58f, 0.35f)}, // Bronze
        };

        // Fallback colors for categories not in the CPKhex set
        private static readonly Color[] fallbackColors = new Color[]
        {
            new Color(1f, 0.5f, 0.5f), // Pink
            new Color(1f, 1f, 0.5f), // Yellow
            new Color(0.66f, 0.88f, 0.56f), // Light green
            new Color(0.56f, 0.74f, 0.88f), // Cyan
            new Color(1f, 0.66f, 0.66f), // Orange-pink
        };

        public enum ElectronType
        {
            Valence = 1,
            Core = 2
        }

        public static GameObject Build(Element e, float scale = 1)
        {
            GameObject atom = new GameObject($"Atom_{e.name}");
            
            // Create nucleus
            GameObject nucleus = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nucleus.name = "Nucleus";
            nucleus.transform.SetParent(atom.transform);
            nucleus.transform.localScale = new Vector3(scale * 0.25f, scale * 0.25f, scale * 0.25f);
            
            // Set nucleus color
            string category = Regex.Replace(e.category.ToLower(), "[^a-z0-9 ]", "");
            nucleus.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            nucleus.GetComponent<Renderer>().material.color = GetColor(category, e.number); // CPKHex fallback category colors
            nucleus.AddComponent<SphereCollider>().isTrigger = true;
            
            // Create rings for shell electrons
            // Constrained to 2 shells for gallery
            int maxShellsVisible = 2;
            for (int shell = 1; shell <= maxShellsVisible && shell <= e.numberOfShells; shell++)
            {
                // Create ring for each shell
                GameObject shellRing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                shellRing.name = $"Shell_{shell}";
                shellRing.transform.SetParent(atom.transform);
                
                // Adjust ring dimensions
                shellRing.transform.localScale = new Vector3(
                    scale * 0.7f, // radius
                    scale * 0.05f, // height
                    scale * 0.7f);
                
                // Position shell on the y-axis
                shellRing.transform.localPosition = new Vector3(0, shell * scale * 0.5f, 0);
                
                // Rotate ring to align horizontally
                shellRing.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                
                // Non-player friendly
                Renderer ringRenderer = shellRing.GetComponent<Renderer>();
                ringRenderer.material.shader = Shader.Find("Standard");
                ringRenderer.material.color = new Color(0.4f, 0.5f, 0.9f);
            }
            
            // Add electrons to valence shells
            if (e.valenceElectrons > 0 && maxShellsVisible >= 1)
            {
                // Use last ring as the valence shell
                GameObject valenceRing = atom.transform.GetChild(maxShellsVisible - 1).gameObject;
                float angleStep = 360f / Math.Max(e.valenceElectrons, 1);
                
                for (int i = 0; i < Math.Min(e.valenceElectrons, 8); i++) // Limit electrons to visually appealing numbers
                {
                    GameObject electron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    electron.name = "Electron_V" + i;
                    electron.transform.SetParent(valenceRing.transform);
                    
                    // Adjust electron size
                    electron.transform.localScale = new Vector3(scale * 0.05f, scale * 0.05f, scale * 0.05f);
                    
                    // Place on circumference
                    float angle = i * angleStep;
                    float radius = valenceRing.transform.localPosition.y - 0.12f; // Plasma sphere adjustment
                    electron.transform.localPosition = new Vector3(radius * Mathf.Sin(angle * Mathf.Deg2Rad), 0, radius * Mathf.Cos(angle * Mathf.Deg2Rad));
                    
                    // Vertex color for electrons
                    Renderer electronRenderer = electron.GetComponent<Renderer>();
                    electronRenderer.material.color = Color.white;
                    
                    // Sidenote: Will be rotated in Update later
                }
            }
            
            return atom;
        }
        
        // Color selection based on atom category
        private static Color GetColor(string category, int atomicNumber)
        {
            if (categoryColors.TryGetValue(category, out Color color))
            {
                return color;
            }
            
            // Fallback: Use atomic number to pick from the fallback Colors
            return fallbackColors[atomicNumber % fallbackColors.Length];
        }
    }
}
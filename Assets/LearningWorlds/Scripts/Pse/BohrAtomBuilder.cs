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
            {"post transition metals", new Color(0.66f, 0.58f, 0.35f)}  // Bronze
        };
        
        // Fallback colors for categories not in the CPKhex set
        private static readonly Color[] fallbackColors = new Color[]
        {
            new Color(1f, 0.5f, 0.5f), // Pink
            new Color(1f, 1f, 0.5f), // Yellow
            new Color(0.66f, 0.88f, 0.56f), // Light green
            new Color(0.56f, 0.74f, 0.88f), // Cyan
            new Color(1f, 0.66f, 0.66f) // Orange-pink
        };

        public enum ElectronType
        {
            Valence = 1,
            Core = 2
        }

        public static float MaxShellsVisible = 2; // Constrain to 2 for gallery as per requirements
        
        public static GameObject Build(ElementData element, float scale = 1)
        {
            GameObject atom = new GameObject("Atom_" + element.id);
            
            // Create nucleus - using cpk_hex color fallback from category hash
            GameObject nucleus = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            nucleus.name = "Nucleus";
            nucleus.transform.SetParent(atom.transform);
            nucleus.transform.localScale = new Vector3(scale * 0.25f, scale * 0.25f, scale * 0.25f);
            
            // Determine category from element ID
            string category = DetermineCategoryFromId(element.id);
            category = Regex.Replace(category, "[^a-z0-9 ]", "");
            
            // Set nucleus color based on category
            int atomicNumber = ParseAtomicNumber(element.id);
            nucleus.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
            nucleus.GetComponent<Renderer>().material.color = GetColor(category, atomicNumber);
            nucleus.AddComponent<SphereCollider>().isTrigger = true;

            // Create electron shells (simplified for gallery view)
            int numberOfShells = CalculateNumberOfShells(atomicNumber);
            for (int shell = 1; shell <= MaxShellsVisible && shell <= numberOfShells; shell++)
            {
                GameObject shellRing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                shellRing.name = "Shell_" + shell;
                shellRing.transform.SetParent(atom.transform);
                
                // Adjust ring dimensions to appropriate size
                shellRing.transform.localScale = new Vector3(
                    scale * 0.7f,                // radius
                    scale * 0.05f,               // height (thin)
                    scale * 0.7f);              // radius
                
                // Position shell vertically (adjusted for visualization)
                shellRing.transform.localPosition = new Vector3(0, shell * 0.5f * scale, 0);
                shellRing.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Rotate horizontally
                
                // Add Electrons Rotator component
                ElectronRotator electronRotator = shellRing.AddComponent<ElectronRotator>();
                electronRotator.rotationSpeed = 30f;

                // Make shells more transparent
                Renderer shellRenderer = shellRing.GetComponent<Renderer>();
                shellRenderer.material.shader = Shader.Find("Standard");
                shellRenderer.material.color = Color.blue;
                shellRenderer.material.color = new Color(shellRenderer.material.color.r,
                                                   shellRenderer.material.color.g,
                                                   shellRenderer.material.color.b, 
                                                   0.3f); // Add transparency
                
                // Add electrons to this shell ring
                AddElectrons(shellRing.transform, shell, element, scale, atomicNumber);
            }
            
            return atom;
        }
        
        // Adds electrons uniformly on their respective rings
        private static void AddElectrons(Transform shellParent, int shellNumber, ElementData element, float atomScale, int atomicNumber)
        {
            int electronCount = GetElectronsInShell(shellNumber, atomicNumber);
            
            // Limit to visual maximum of 8 shell signature electrons for gallery
            electronCount = Mathf.Min(electronCount, 8);
            
            for (int i = 0; i < electronCount; i++)
            {
                
                // Create electron (small sphere)
                GameObject electron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                electron.name = "Electron_" + shellNumber + "_" + i;
                electron.transform.SetParent(shellParent);

                // Small size
                electron.transform.localScale = new Vector3(atomScale * 0.05f, atomScale * 0.05f, atomScale * 0.05f);

                // Position on the ring circumference
                float angle = 2 * Mathf.PI * i / electronCount;
                float ringRadius = shellParent.localScale.x;
                electron.transform.localPosition = new Vector3(
                    ringRadius * Mathf.Cos(angle),
                    0, 
                    ringRadius * Mathf.Sin(angle)
                );
                
                // Configure electron
                Renderer renderer = electron.GetComponent<Renderer>();
                renderer.material.shader = Shader.Find("Standard");
                renderer.material.color = Color.white;
            }
        }
        
        // Color selection based on atom category
        private static Color GetColor(string category, int atomicNumber)
        {
            if (categoryColors.TryGetValue(category, out Color color))
            {
                return color;
            }
            
            // Fallback colors based on atomic number
            return fallbackColors[atomicNumber % fallbackColors.Length];
        }

        /// <summary>
        /// Calculates electrons in a given shell based on atomic number.
        /// Simplified: 2, 8, 18, 32 for shells 1-4
        /// </summary>
        private static int GetElectronsInShell(int shellNumber, int atomicNumber)
        {
            if (shellNumber <= 0) return 0;
            
            int[] shellCapacities = { 2, 8, 18, 32, 32, 18, 8, 2 };
            int totalElectrons = 0;
            
            for (int s = 0; s < Mathf.Min(shellNumber, shellCapacities.Length); s++)
            {
                int electronsInThisShell = Mathf.Min(shellCapacities[s], atomicNumber - totalElectrons);
                if (s + 1 == shellNumber)
                    return electronsInThisShell;
                totalElectrons += electronsInThisShell;
            }
            return 0;
        }

        /// <summary>
        /// Determines element category based on atomic number.
        /// </summary>
        private static string DetermineCategoryFromId(string elementId)
        {
            if (int.TryParse(elementId, out int atomicNumber))
            {
                if (atomicNumber <= 2) return "noble gases";
                if (atomicNumber <= 10) return "nonmetals";
                if (atomicNumber == 11 || atomicNumber == 19) return "alkali metals";
                if (atomicNumber == 12 || atomicNumber == 20) return "alkaline earth metals";
                if (atomicNumber >= 21 && atomicNumber <= 30) return "transition metals";
                if (atomicNumber >= 31 && atomicNumber <= 38) return "post transition metals";
                if (atomicNumber >= 39 && atomicNumber <= 48) return "transition metals";
                if (atomicNumber >= 57 && atomicNumber <= 71) return "lanthanides";
                if (atomicNumber >= 89 && atomicNumber <= 103) return "actinides";
                if (atomicNumber >= 5 && atomicNumber <= 8 || atomicNumber == 14 || atomicNumber == 15 || atomicNumber == 16 || atomicNumber == 32 || atomicNumber == 33 || atomicNumber == 34) return "nonmetals";
                if (atomicNumber == 17 || atomicNumber == 35 || atomicNumber == 53 || atomicNumber == 85) return "halogens";
                return "other metals";
            }
            return "other metals";
        }

        /// <summary>
        /// Parses atomic number from element ID.
        /// </summary>
        private static int ParseAtomicNumber(string elementId)
        {
            if (int.TryParse(elementId, out int number))
                return number;
            
            // Try to extract number from symbol like "H", "He", etc.
            // For now, return a default
            return 1;
        }

        /// <summary>
        /// Calculates maximum number of electron shells for a given atomic number.
        /// </summary>
        private static int CalculateNumberOfShells(int atomicNumber)
        {
            if (atomicNumber <= 2) return 1;
            if (atomicNumber <= 10) return 2;
            if (atomicNumber <= 18) return 3;
            if (atomicNumber <= 36) return 4;
            if (atomicNumber <= 54) return 5;
            if (atomicNumber <= 86) return 6;
            return 7;
        }
    }
}
#endregion
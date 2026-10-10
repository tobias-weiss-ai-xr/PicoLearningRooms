#region Usings
using UnityEngine;
using System.Linq;

namespace LearningWorlds.PSE
{
    public static class ElementGallery
    {
        public static GameObject Build()
        {
            GameObject gallery = new GameObject(nameof(Build));
            
            // Instantiate floor plane
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            Renderer floorRenderer = floor.GetComponent<Renderer>();
            floorRenderer.material.color = new Color(0.5f, 0.5f, 0.5f);
            floor.transform.position = new Vector3(0f, -1f, 0f);
            floor.transform.parent = gallery.transform;
            floor.transform.localScale = new Vector3(40f, 1f, 40f);
            
            // Register builder for LearningWorldsApp
            LearningWorldsApp.Builders[World.PseGallery] = Build;
            
            int index = 0;
            // Use 8 columns, 9 rows or vice versa to accommodate all 118 elements
            float baseSpacing = 1.15f;
            int[,] elementsLayout = {
                {1, 3, 11, 19, 37, 55, 87, 109},
                {2, 4, 12, 20, 38, 56, 88, 110},
                {5, 6, 13, 21, 39, 57, 89, 111},
                {7, 8, 14, 22, 40, 58, 90, 112},
                {9, 10, 15, 23, 41, 59, 91, 113},
                {16, 17, 24, 25, 42, 60, 92, 114},
                {18, 26, 27, 28, 43, 61, 93, 115},
                {29, 30, 31, 32, 33, 34, 35, 36, 62, 94, 116},
                {3, null, null, 44, 63, 64, 95, 117, 118}
            };
            
            foreach (var e in DataStore.GetPeriodicTable().Where(x => x.number > 0))
            {
                GameObject atom = BohrAtomBuilder.Build(e);
                atom.transform.parent = gallery.transform;
                
                int xPos = index % 8;
                int yPos = GetYColumnPosition(elementsLayout, index);
                
                float x = xPos * baseSpacing; // Positioning to ensure it matches the grid
                // Using xpos * baseSpacing to specify element xpos placement on the grid
                // Original axis plan logic: xpos/z = -yPos * 2.0
                float z = yPos * -baseSpacing + 0.5f;
                
                float scale = 1.0f + ((118 - e.number) / 100f); // Scale is larger for lower elements
                
                atom.transform.localPosition = new Vector3(x, yPos * 0.25f + 0.5f, z);
                atom.transform.localScale = Vector3.one * scale;
                float x = xPos * baseSpacing - yPos * baseSpacing * 2.0f;
                float z = -yPos * baseSpacing * 2.0f;
                atom.transform.localPosition = new Vector3(x, yPos * 0.25f + 0.5f, z);
                
                // Add collider to allow selection
                SphereCollider collider = atom.AddComponent<SphereCollider>();
                collider.radius = 0.75f;
                
                // Ensure object is selectable
                collider.isTrigger = true;
                
                index++;
            }
            
            return gallery;
        }
        
        private static int GetYColumnPosition(int[,] elementsLayout, int index)
        {
            for (int y = 0; y < elementsLayout.GetLength(0); y++)
            {
                var row = elementsLayout[y];
                
                int nonNullPos = -1;
                foreach (var element in row)
                {
                    if (element != null)
                    {
                        nonNullPos++;
                        if (nonNullPos == index % row.Length)
                        {
                            return y;
                        }
                    }
                }
            }
            
            return 0; // fallback value
        }
    }
}
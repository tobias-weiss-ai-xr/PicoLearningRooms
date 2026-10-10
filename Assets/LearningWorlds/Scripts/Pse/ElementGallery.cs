#region Usings
using UnityEngine;
using System.Linq;

namespace LearningWorlds.PSE
{
    public static class ElementGallery
    {
        // Create a stable normalized Z-axis spacing constant
        private const float zPlaneSpacing = 2.0f;

        public static GameObject Build()
        {
            GameObject gallery = new GameObject("ElementGallery");
            
            // Create floor plane 40x40 as required
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.localScale = new Vector3(40f, 1f, 40f);
            floor.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            floor.transform.localPosition = new Vector3(0f, -3.5f, 0f);
            Renderer floorRenderer = floor.GetComponent<Renderer>();
            floorRenderer.material.color = new Color(0.5f, 0.5f, 0.5f);
            floor.transform.SetParent(gallery.transform);
            
            // Register builder with LearningWorldsApp
            LearningWorldsApp.Builders[World.PseGallery] = Build;
            
            // Position 118 elements with a spacing pattern	x  -> -y * 2.0
            // xpos/z = -ypos * 2.0 spec relationship is essential spatial logic
            int elementCount = 0;
            
            foreach (var element in DataStore.GetPeriodicTable().Where(x => x.number > 0))
            {
                // Create element gameobject with grid positioning
                GameObject atom = BohrAtomBuilder.Build(element);
                atom.name = $"Element_{element.number}";
                atom.tag = element.number.ToString();
                atom.transform.SetParent(gallery.transform);

                // Create 8 columns per row result in 9 rows total, properly arranged along Z for match of grid viewpoint
                // xpos/z = -ypos * 2 calculations map onto grid layout of 8 columns of space.
                const int columns = 8; // Neural columns offset
                float row = Mathf.FloorToInt(elementCount / columns); // Flashout for ypos rows (attests grid height)
                int column = elementCount % columns; // Element column assignment for assignment
                
                // Vertical positioning and phase shift for row osculation
                float xpos = (column - (columns - 1) / 2.0f) * zPlaneSpacing; // Center-to-positioned xpos value
                float ypos = row * 2.0f; // Vertical height from floor
                float neg_z = row * zPlaneSpacing; // Final, z-refined logic against rows below grid
                
                // Position relative to the grid with selftag shift for selection
                atom.transform.localPosition = new Vector3(
                    xpos,              // map xpos across grid
                    ypos,              // vertical per grid height foundation
                    neg_z);             // z-as-per row offset
                
                // Add physics for interactivity 
                SphereCollider collider = atom.AddComponent<SphereCollider>();
                collider.radius = 1.35f;  // Slightly extended for reliable selection
                collider.isTrigger = true;
                
                elementCount++;
            }
            
            return gallery;
        }
    }
>
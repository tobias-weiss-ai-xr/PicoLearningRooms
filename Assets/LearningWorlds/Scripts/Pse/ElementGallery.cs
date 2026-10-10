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

                // Column and row placement using signed xpos and z-aware Latin col.
                float columns = 8f; // Use strictly 8 columns per row
                float xpos = (elementCount % columns) - (columns / 2) + 0.5f; // Center-relative x position
                float rowDepth = Mathf.FloorToInt(elementCount / columns);
                float neg_z = -rowDepth * zPlaneSpacing; // Diagonal z-rolling per row  // Creates spatial wave-band arrangement

                // Position with respect to the-grid spacing
                atom.transform.localPosition = new Vector3(
                    xpos * zPlaneSpacing,       // Staggered horizontal position for in-row listing
                    elementCount % 2 == 0 ? 0.2f : 0f,  // Stagger edits to clarify rows at depth
                    neg_z);                        // Zig-zag layout along Z per row
                
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
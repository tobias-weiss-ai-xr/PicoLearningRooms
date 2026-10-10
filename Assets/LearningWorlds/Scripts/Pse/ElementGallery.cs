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
                float xpos = elementCount % 8;                // X-position within columns (indexed per group)
                float neg_y_times_two = - 
                    (int)Mathf.Floor((decimal)elementCount / 8) * 2.0f;  // Creates spatial wave-band arrangement

                // Position with respect to the-grid spacing
                atom.transform.localPosition = new Vector3(
                    xpos * zPlaneSpacing,   // Horizontal Z-relative left-move position
                    elementCount / 17f,     // Small elevation to assist row differentiation
                    neg_y_times_two);      // Deep z-rolling for long-view grid structuring
                
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
using UnityEngine;
using System.Linq;
using LearningWorlds;
using LearningWorlds.Pse;

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Creates a gallery of all chemical elements arranged in a grid.
    /// </summary>
    public static class ElementGallery
    {
        private const float zPlaneSpacing = 2.0f;
        private const int Columns = 8;

        /// <summary>
        /// Builds the periodic table gallery.
        /// </summary>
        /// <returns>The gallery GameObject.</returns>
        public static GameObject Build()
        {
            GameObject gallery = new GameObject("ElementGallery");

            // Create floor plane
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.localScale = new Vector3(40f, 1f, 40f);
            floor.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            floor.transform.localPosition = new Vector3(0f, -3.5f, 0f);

            Renderer floorRenderer = floor.GetComponent<Renderer>();
            floorRenderer.material.color = new Color(0.5f, 0.5f, 0.5f);
            floor.transform.SetParent(gallery.transform);

            // Register builder with LearningWorldsApp
            LearningWorldsApp.Builders[World.PseGallery] = w => new PseGalleryBuilder();

            // Position all elements in a grid
            int elementCount = 0;
            foreach (var element in ElementData.LoadAll())
            {
                // Create element GameObject
                GameObject atom = BohrAtomBuilder.Build(element);
                atom.name = "Element_" + element.id;
                atom.tag = element.id;
                atom.transform.SetParent(gallery.transform);

                // Calculate grid position: 8 columns per row
                int row = Mathf.FloorToInt(elementCount / Columns);
                int column = elementCount % Columns;

                // Center-aligned grid
                float xpos = (column - (Columns - 1) / 2.0f) * zPlaneSpacing;
                float ypos = row * 2.0f;
                float zpos = row * zPlaneSpacing;

                atom.transform.localPosition = new Vector3(xpos, ypos, zpos);

                // Add trigger collider for selection
                SphereCollider collider = atom.AddComponent<SphereCollider>();
                collider.radius = 1.35f;
                collider.isTrigger = true;

                elementCount++;
            }

            return gallery;
        }
    }
}

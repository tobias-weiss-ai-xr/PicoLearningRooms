using UnityEngine;
using LearningWorlds.Hub;

namespace LearningWorlds.Hub
{
    /// <summary>
    /// Builds the central learning hub with a circular floor and portal signage.
    /// </summary>
    public static class LandingHub
    {
        private const float DiscRadius = 15.0f;
        private const int VertexCount = 36;

        /// <summary>
        /// Generates a circular disc mesh for the hub floor.
        /// </summary>
        private static Mesh GenerateDiscMesh(float radius)
        {
            Mesh mesh = new Mesh { name = "FloorDisc_Mesh" };
            Vector3[] vertices = new Vector3[VertexCount + 1]; // +1 for center
            int[] triangles = new int[VertexCount * 3];
            Vector2[] uv = new Vector2[VertexCount + 1];

            // Center vertex
            vertices[0] = Vector3.zero;
            uv[0] = new Vector2(0.5f, 0.5f);

            // Outer vertices in a circle
            float angleIncrement = 2f * Mathf.PI / VertexCount;
            for (int i = 1; i <= VertexCount; i++)
            {
                float angle = (i - 1) * angleIncrement;
                vertices[i] = new Vector3(
                    Mathf.Cos(angle) * radius,
                    0f,
                    Mathf.Sin(angle) * radius
                );
                uv[i] = new Vector2(Mathf.Cos(angle) * 0.5f + 0.5f, Mathf.Sin(angle) * 0.5f + 0.5f);
            }

            // Create triangles (fan from center)
            for (int i = 0; i < VertexCount; i++)
            {
                triangles[i * 3] = 0;      // Center
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = (i + 1) % VertexCount + 1;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();

            return mesh;
        }

        /// <summary>
        /// Creates a portal post (column) at the specified position.
        /// </summary>
        private static GameObject CreatePortalPost(Transform parent, Vector3 localPosition)
        {
            GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cube);
            post.transform.SetParent(parent);
            post.transform.localPosition = localPosition;
            post.transform.localScale = new Vector3(0.15f, 3.4f, 0.15f);
            post.name = "PortalPost";

            Renderer renderer = post.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Standard"))
            {
                color = new Color32(51, 51, 51, 255) // Dark gray
            };

            return post;
        }

        /// <summary>
        /// Creates the lintel (top bar) connecting two portal posts.
        /// </summary>
        private static GameObject CreateLintel(Transform parent)
        {
            GameObject lintel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lintel.transform.SetParent(parent);
            lintel.transform.localPosition = new Vector3(0f, 2.2f, 0f);
            lintel.transform.localScale = new Vector3(1.6f, 0.06f, 0.1f);
            lintel.name = "Lintel";

            Renderer renderer = lintel.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Standard"))
            {
                color = new Color32(51, 51, 51, 255)
            };

            return lintel;
        }

        /// <summary>
        /// Creates the emissive cyan portal plane.
        /// </summary>
        private static GameObject CreateEmissivePlane(Transform parent)
        {
            GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.SetParent(parent);
            plane.transform.localPosition = new Vector3(0f, 1.5f, -0.12f);
            plane.transform.localScale = new Vector3(1.8f, 1.1f, 1f);
            plane.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            plane.name = "EmissivePlane";

            Renderer renderer = plane.GetComponent<Renderer>();
            Material material = new Material(Shader.Find("Standard"));
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", Color.cyan * 1.3f);
            renderer.material = material;

            return plane;
        }

        /// <summary>
        /// Creates a label text mesh for the portal.
        /// </summary>
        private static GameObject CreateLabel(Transform parent, string text, Vector3 localPosition)
        {
            GameObject labelObj = new GameObject("Label_" + text);
            TextMesh textMesh = labelObj.AddComponent<TextMesh>();
            labelObj.transform.SetParent(parent);
            labelObj.transform.localPosition = localPosition;

            textMesh.text = text;
            textMesh.characterSize = 0.25f;
            textMesh.alignment = TextAlignment.Center;
            textMesh.fontSize = 24;

            return labelObj;
        }

        /// <summary>
        /// Creates a portal trigger collider.
        /// </summary>
        private static GameObject CreateTrigger(Transform parent, World world)
        {
            GameObject trigger = new GameObject("PortalTrigger");
            trigger.transform.SetParent(parent);
            trigger.transform.localPosition = new Vector3(0f, 1.5f, -0.5f);
            trigger.transform.localScale = new Vector3(1.5f, 3.0f, 1.0f);

            BoxCollider collider = trigger.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            Portal.TriggerComponent triggerComponent = trigger.AddComponent<Portal.TriggerComponent>();
            triggerComponent.setupWorld(world);

            return trigger;
        }

        /// <summary>
        /// Builds the complete learning hub with floor and portals.
        /// </summary>
        public static GameObject Build()
        {
            // Create hub container
            GameObject hubRoot = new GameObject("LearningWorlds-Hub");

            // Create disc floor
            Mesh discMesh = GenerateDiscMesh(DiscRadius);
            GameObject floor = new GameObject("FloorDisc", typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider));
            floor.transform.SetParent(hubRoot.transform);
            floor.GetComponent<MeshFilter>().mesh = discMesh;

            Material floorMaterial = new Material(Shader.Find("Standard"))
            {
                color = new Color(0.58f, 0.73f, 0.58f) // Soft green
            };
            floor.GetComponent<Renderer>().material = floorMaterial;

            // Floor collider
            BoxCollider floorCollider = floor.GetComponent<BoxCollider>();
            floorCollider.size = new Vector3(DiscRadius * 2f, 0.1f, DiscRadius * 2f);
            floorCollider.center = new Vector3(0f, -0.05f, 0f);

            // Create portals at 120 degree intervals
            string[] portalNames = { "PSE Gallery", "Element Room", "Arachnophobia" };
            World[] portalWorlds = { World.PseGallery, World.ElementRoom, World.Arachnophobia };
            float radius = 4f;

            for (int i = 0; i < portalNames.Length; i++)
            {
                float angle = i * 120f * Mathf.Deg2Rad;
                Vector3 position = new Vector3(
                    Mathf.Cos(angle) * radius,
                    0f,
                    Mathf.Sin(angle) * radius
                );

                Portal.Create(portalNames[i], portalWorlds[i], position, -angle * Mathf.Rad2Deg);
            }

            return hubRoot;
        }
    }
}

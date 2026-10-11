using UnityEngine;
using LearningWorlds;
using LearningWorlds.Pse;

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Creates a dedicated room for a single chemical element.
    /// Includes a central Bohr atom model, information sign, and quiz stations.
    /// </summary>
    public static class ElementRoom
    {
        public static ElementData SelectedElement { get; set; }

        /// <summary>
        /// Builds the element room.
        /// </summary>
        public static GameObject Build()
        {
            if (SelectedElement == null)
            {
                Debug.LogError("ElementRoom: SelectedElement not set!");
                return null;
            }

            GameObject roomRoot = new GameObject("ElementRoom_" + SelectedElement.id);

            // Build floor mandala with 3 translucent discs
            CreateMandalaFloor(roomRoot.transform, SelectedElement);

            // Build Bohr atom as central monument
            BohrAtomBuilder.Build(SelectedElement);

            // Place symbol and description sign
            CreateElementSign(roomRoot.transform, SelectedElement);

            // Place 5 learning stations in a ring
            ArrangeStationRing(roomRoot.transform, SelectedElement);

            // Register builder with LearningWorldsApp
            LearningWorldsApp.Builders[World.ElementRoom] = e => new ElementRoomBuilder(e);

            return roomRoot;
        }

        /// <summary>
        /// Creates the mandala floor pattern.
        /// </summary>
        private static void CreateMandalaFloor(Transform parent, ElementData element)
        {
            GameObject floorBase = new GameObject("MandalaFloor");
            floorBase.transform.SetParent(parent);

            // Create 3 concentric discs with different transparency
            for (int tier = 1; tier <= 3; tier++)
            {
                GameObject disc = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                disc.transform.SetParent(floorBase.transform);
                disc.name = "MandalaDisc_" + tier;

                float scale = 3.0f * tier;
                disc.transform.localScale = new Vector3(0.1f * scale, 0.06f, 0.1f * scale);

                // Set material color based on tier
                float alpha = 0.75f - (tier * 0.1f);
                Color color = new Color(0.3f, 0.6f, 0.4f, alpha); // Greenish with varying alpha

                Renderer renderer = disc.GetComponent<Renderer>();
                renderer.material = new Material(Shader.Find("Standard"));
                renderer.material.color = color;
                renderer.material.SetFloat("_Mode", 3); // Transparent mode
                renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                renderer.material.SetInt("_ZWrite", 0);
                renderer.material.DisableKeyword("_ALPHATEST_ON");
                renderer.material.EnableKeyword("_ALPHABLEND_ON");
                renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                renderer.material.renderQueue = 3000;
            }
        }

        /// <summary>
        /// Creates the element information sign.
        /// </summary>
        private static void CreateElementSign(Transform parent, ElementData element)
        {
            GameObject signObj = new GameObject("ElementSign");
            signObj.transform.SetParent(parent);
            signObj.transform.position = new Vector3(0f, 1.5f, -4f);

            // Create a backing plane
            GameObject backing = GameObject.CreatePrimitive(PrimitiveType.Plane);
            backing.transform.SetParent(signObj.transform);
            backing.transform.localScale = new Vector3(4f, 1f, 1f);
            backing.transform.localPosition = new Vector3(0f, 0f, 0f);
            backing.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            Renderer backingRenderer = backing.GetComponent<Renderer>();
            backingRenderer.material = new Material(Shader.Find("Standard"));
            backingRenderer.material.color = new Color(0.9f, 0.9f, 0.9f, 0.8f);

            // Create text
            GameObject textObj = new GameObject("SignText");
            TextMesh textMesh = textObj.AddComponent<TextMesh>();
            textObj.transform.SetParent(signObj.transform);
            textObj.transform.localPosition = new Vector3(0f, 0.05f, 0.01f);

            textMesh.text = element.id + "\n" + element.nameEn + "\n\n" + (element.descriptionEn ?? "");
            textMesh.characterSize = 0.15f;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;

            // Try to use built-in font
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (font != null)
            {
                textMesh.font = font;
            }
        }

        /// <summary>
        /// Arranges quiz stations in a circular ring around the central atom.
        /// </summary>
        private static void ArrangeStationRing(Transform parent, ElementData element)
        {
            const float Radius = 3.5f;
            const int StationCount = 5;

            GameObject stationContainer = new GameObject("Stations");
            stationContainer.transform.SetParent(parent);

            float angleStep = 2f * Mathf.PI / StationCount;

            for (int i = 0; i < StationCount; i++)
            {
                float angle = i * angleStep;
                Vector3 position = new Vector3(
                    Mathf.Cos(angle) * Radius,
                    0.52f,
                    Mathf.Sin(angle) * Radius
                );

                QuizStation station = QuizStation.Create(element, position, i);
                station.name = "QuizStation_" + (i + 1);
                station.transform.SetParent(stationContainer.transform);
            }
        }

        /// <summary>
        /// Registers this builder with the BuildingBuilders registry.
        /// </summary>
        public static void RegisterInBuilders()
        {
            BuildingBuilders.Register(World.ElementRoom, Build);
        }
    }
}

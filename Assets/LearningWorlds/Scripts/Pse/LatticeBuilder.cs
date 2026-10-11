using UnityEngine;

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Builds various crystal lattice structures (BCC, FCC, HCP, Diamond).
    /// </summary>
    public static class LatticeBuilder
    {
        public enum BondStyle { BCC, HCP, DIAMOND, FCC }

        /// <summary>
        /// Builds a crystal lattice structure based on element category and symbol.
        /// </summary>
        public static GameObject BuildStructure(string symbol, string category, Vector3 center, float cellSize = 0.3f)
        {
            GameObject latticeGroup = new GameObject(symbol + "_Lattice");
            latticeGroup.transform.position = center;

            // Build different lattice structures based on category and symbol
            if (symbol == "C" || category.Contains("Solid"))
                return BuildDiamondLattice(latticeGroup, cellSize);
            else if (symbol == "Fe" || symbol == "Cr" || symbol == "W" || symbol == "V" ||
                     symbol == "Mo" || symbol == "Ta" || symbol == "Nb")
                return BuildBCCLattice(latticeGroup, cellSize);
            else if (symbol == "Co" || symbol == "Zn" || symbol == "Mg" || symbol == "Ti")
                return BuildHCPLattice(latticeGroup, cellSize);
            else
                return BuildFCCLattice(latticeGroup, cellSize);
        }

        /// <summary>
        /// Builds a Body-Centered Cubic (BCC) lattice.
        /// </summary>
        public static GameObject BuildBCCLattice(GameObject parent, float cellSize)
        {
            const int gridSize = 3;
            const float atomRadius = 0.08f;

            // Create BCC lattice atoms
            for (int x = 0; x < gridSize * 2; x++)
            {
                for (int y = 0; y < gridSize * 2; y++)
                {
                    for (int z = 0; z < gridSize * 2; z++)
                    {
                        // BCC: corners + center
                        if (x == gridSize || y == gridSize || z == gridSize)
                        {
                            CreateAtom(parent.transform, "BCC_Atom", 
                                new Vector3((x - gridSize + 0.5f) * cellSize,
                                            (y - gridSize + 0.5f) * cellSize,
                                            (z - gridSize + 0.5f) * cellSize),
                                atomRadius, Color.grey);
                        }
                    }
                }
            }

            AddBondingLines(parent, cellSize, BondStyle.BCC);
            return parent;
        }

        /// <summary>
        /// Builds a Face-Centered Cubic (FCC) lattice.
        /// </summary>
        public static void BuildFCCLattice(GameObject parent, float cellSize)
        {
            const float atomRadius = 0.09f;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        // FCC: corners + face centers
                        byte validFCCell = (byte)(x % 2 + y % 2 + z % 2);
                        if (validFCCell != 1) continue;

                        CreateAtom(parent.transform, "FCC_Atom",
                            new Vector3((x - 1) * cellSize + 0.5f * cellSize,
                                        (y - 1) * cellSize + 0.5f * cellSize,
                                        (z - 1) * cellSize + 0.5f * cellSize),
                            atomRadius, Color.cyan);
                    }
                }
            }

            AddBondingLines(parent, cellSize, BondStyle.FCC);
        }

        /// <summary>
        /// Builds a Hexagonal Close-Packed (HCP) lattice.
        /// </summary>
        public static GameObject BuildHCPLattice(GameObject parent, float cellSize)
        {
            float baseCellHeight = cellSize * 2.0f;
            float layerSpacing = baseCellHeight * 0.9f;

            // Create atoms in hexagonal pattern
            for (int layer = 0; layer < 3; layer++)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        float offsetY = (y % 2 == 0) ? 0f : layerSpacing * 0.5f;
                        CreateAtom(parent.transform, "HCP_Atom",
                            new Vector3(x * cellSize * 0.866f,
                                        layer * layerSpacing + offsetY,
                                        y * cellSize),
                            0.09f, new Color(0.7f, 0.7f, 0.9f));
                    }
                }
            }

            AddBondingLines(parent, cellSize, BondStyle.HCP);
            return parent;
        }

        /// <summary>
        /// Builds a Diamond crystal lattice.
        /// </summary>
        public static GameObject BuildDiamondLattice(GameObject parent, float cellSize)
        {
            float cellSpacing = cellSize * 0.9f;
            float radius = 0.08f;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        // Skip certain positions to create diamond pattern
                        if (z % 2 != 0 && x % 2 != 0 && y == 0)
                            continue;

                        CreateAtom(parent.transform, "Diamond_Atom",
                            new Vector3(
                                (x + 1) * cellSpacing - (x % 2 == 1 ? cellSpacing : 0f),
                                (y + 1) * cellSpacing - (y % 2 == 1 ? cellSpacing : 0f),
                                z * cellSpacing),
                            radius, new Color(0.9f, 0.9f, 0.9f));
                    }
                }
            }

            AddBondingLines(parent, cellSize, BondStyle.DIAMOND);
            return parent;
        }

        /// <summary>
        /// Creates a single atom sphere.
        /// </summary>
        private static GameObject CreateAtom(Transform parent, string name, Vector3 position, float radius, Color color)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(parent);
            sphere.name = name;
            sphere.transform.localPosition = position;
            sphere.transform.localScale = Vector3.one * radius;

            Renderer renderer = sphere.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = color;

            return sphere;
        }

        /// <summary>
        /// Adds bonding lines to connect atoms in the lattice.
        /// </summary>
        public static void AddBondingLines(GameObject parent, float cellSize, BondStyle type, float widthScale = 0.1f)
        {
            Color bondColor;
            switch (type)
            {
                case BondStyle.BCC: bondColor = new Color(0.3f, 0.3f, 0.9f); break;
                case BondStyle.HCP: bondColor = new Color(0.25f, 0.15f, 0.95f); break;
                case BondStyle.FCC: bondColor = new Color(0.85f, 0.45f, 0.9f); break;
                default: bondColor = new Color(0.6f, 0.6f, 0.6f); break;
            }

            LineRenderer lr = parent.AddComponent<LineRenderer>();
            lr.startWidth = lr.endWidth = cellSize * 0.01f;
            lr.startColor = lr.endColor = bondColor;
            lr.material = new Material(Shader.Find("Standard"));
            lr.material.color = bondColor;

            if (type == BondStyle.DIAMOND)
                AddFullBonding(parent, cellSize);
            else
                AddGridBonding(parent, cellSize, type);
        }

        /// <summary>
        /// Adds grid-based bonding lines.
        /// </summary>
        private static void AddGridBonding(GameObject parent, float cellSize, BondStyle style)
        {
            LineRenderer lr = parent.GetComponent<LineRenderer>();

            const int gridSize = 2;
            Vector3[] startPositions = new Vector3[gridSize * 3];
            Vector3[] endPositions = new Vector3[gridSize * 3];

            for (int i = 0; i < gridSize; i++)
            {
                Vector3 startPos = new Vector3(i * cellSize, 0, 0);
                startPositions[i] = startPos;
                endPositions[i] = new Vector3((i + 1.0f) * cellSize, 0, 0);

                startPositions[i + gridSize] = new Vector3(0, i * cellSize, 0);
                endPositions[i + gridSize] = new Vector3(0, (i + 1.0f) * cellSize, 0);

                startPositions[i + gridSize * 2] = new Vector3(0, 0, i * cellSize);
                endPositions[i + gridSize * 2] = new Vector3(0, 0, (i + 1.0f) * cellSize);
            }

            lr.positionCount = gridSize * 6;
            for (int i = 0; i < gridSize * 3; i++)
            {
                lr.SetPosition(i * 2, startPositions[i]);
                lr.SetPosition(i * 2 + 1, endPositions[i]);
            }
        }

        /// <summary>
        /// Adds full bonding for complex lattices.
        /// </summary>
        private static void AddFullBonding(GameObject parent, float cellSize)
        {
            // Simplified bonding for now - just add basic connections
            AddGridBonding(parent, cellSize, BondStyle.DIAMOND);
        }
    }
}

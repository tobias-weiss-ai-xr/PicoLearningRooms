using UnityEngine;

public static class LatticeBuilder
{
    // Common lattice structures
    public static GameObject BuildStructure(string symbol, string category, Vector3 center, float cell = 0.3f)
    {
        GameObject lattice = new GameObject(symbol + " Lattice");
        lattice.transform.position = center;

        // Metal BCC structures (Body Centered Cubic)
        if (category == "Metal"
            && (symbol == "Fe" || symbol == "Cr" || symbol == "W" || symbol == "V" || 
               symbol == "Mo" || symbol == "Ta" || symbol == "Nb"))
        {
            BuildBCCLattice(lattice, cell);
        }
        // HCP structures for others
        else if (category == "Metal"
              && (symbol == "Co" || symbol == "Zn" || symbol == "Mg" || symbol == "Ti"))
        {
            BuildHCPLattice(lattice, cell);
        }
        // Diamond structure for Carbon
        else if (symbol == "C")
        {
            BuildDiamondLattice(lattice, cell);
        }
        //Default for others
        else
        {
            BuildFCCLattice(lattice, cell); // Typical default FCC
        }

        return lattice;
    }

    // Bohr Atom Builder
    public static GameObject GetBohrAtomBuilder(string symbol, float radiusScale = 1f)
    {
        GameObject atom = new GameObject(symbol + " BohrModel");
        GameObject core = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        core.name = "Nucleus";
        core.transform.SetParent(atom.transform);
        core.transform.localScale = Vector3.one * radiusScale;

        ElementData elementData = ScriptableObject.CreateInstance<ElementData>();
        if (DataManager.ElementDatabase.TryGetValue(symbol, out var data))
        {
            elementData.symbol = symbol;
            elementData.category = data.category;
            elementData.name = data.name;
            atom.AddComponent<ElementData>().CopyFrom(data);
        }

        // Add electrons
        for (int i = 1; i <= elementData.atomicNumber; i++)
        {
            GameObject electron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            electron.name = "Electron" + i;
            electron.transform.SetParent(atom.transform);
            electron.transform.localScale = Vector3.one * 0.05f;
            float electronRadius = radiusScale * (1f / (int)Mathf.Sqrt(i));
            electron.transform.localPosition = new Vector3(Random.Range(-electronRadius, electronRadius), Random.Range(-electronRadius, electronRadius), Random.Range(-electronRadius, electronRadius));
        }

        return atom;
    }

    private static void BuildBCCLattice(GameObject lattice, float cellSize = 0.3f)
    {
        // Layout 3x3x3 simple cubic cells with BCC atoms
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    // Spheres forming the BCC structure
                    GameObject sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere1.name = "Atom";
                    sphere1.transform.parent = lattice.transform;
                    sphere1.transform.localPosition = new Vector3(x * cellSize * 2, y * cellSize * 2, z * cellSize * 2);

                    GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere2.name = "Atom";
                    sphere2.transform.parent = lattice.transform;
                    sphere2.transform.localPosition = new Vector3((x + 0.5f) * cellSize * 2, (y + 0.5f) * cellSize * 2, (z + 0.5f) * cellSize * 2);

                    // Connect the atoms with lines
                    LineRenderer lr = sphere1.AddComponent<LineRenderer>();
                    lr.startColor = lr.endColor = new Color(0.5f, 0.5f, 0.9f, 0.5f);
                    lr.startWidth = lr.endWidth = 0.03f;
                    lr.positionCount = 2;
                    lr.useWorldSpace = false;
                    lr.SetPosition(0, Vector3.one * 0.12f);
                    lr.SetPosition(1, Vector3.right * 0.3f);
                }
        }
    }
    
    private static void BuildHCPLattice(GameObject lattice, float cellsize)
    {
        // Simplified HCP lattice
        for (int y = -1; y <= 1; y++)
            for (int x = -2; x <= 2; x++)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.parent = lattice.transform;

                if (y % 2 == 0)
                {
                    sphere.name = "HCP Sphere";
                    sphere.transform.localPosition = new Vector3(x * cellsize, y * cellsize,
                        Mathf.Sqrt(3) / 2f * cellsize * (y == 0 ? 0 : y * 2f * (0.866025f)));
                }
                else
                {
                    sphere.name = "HCP Sphere";
                    sphere.transform.localPosition = new Vector3((x + 0.5f) * cellsize, y * cellsize,
                        Mathf.Sqrt(3) / 2f * cellsize * (y == -1 ? 2f * -0.866025f : 2f * 0.866025f));
                }
            }
    }

    private static void BuildDiamondLattice(GameObject lattice, float cellSize)
    {
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    // Alternate tetragonal lattice for diamond structure
                    int modifier = (x + y) % 2;
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.name = "Diamond Atom";
                    sphere.transform.localPosition = new Vector3(x - 0.5f, y - 0.5f, z + modifier * 0.25f) * cellSize * 2f;
                    sphere.transform.parent = lattice.transform;
                }
    }

    private static void BuildFCCLattice(GameObject lattice, float cellSize = 0.3f)
    {
        // Create a face-centered cubic lattice: cover corners of the cube
        GameObject[] sceneParent = Util.Createparents("Atoms");

        // Simple FCC lattice structure
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
                for (int z = -1; z <= 1; z++)
                {
                    // Check if this position is part of FCC lattice
                    if ((x + y + z) % 2 != 0)
                    {
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.parent = sceneParent.transform;
                        sphere.transform.localPosition = new Vector3(x, y, z) * cellSize * 1.5f;
                        sphere.transform.localScale = Vector3.one * cellSize;
                        sphere.name = "FCCSphere";
                    }
                }
    }
    }
}
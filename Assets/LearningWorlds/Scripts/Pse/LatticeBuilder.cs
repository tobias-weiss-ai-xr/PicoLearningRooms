using UnityEngine;

public static class LatticeBuilder {
    public static GameObject BuildStructure(string symbol, string category, Vector3 center, float cellSize = 0.3f) {
        GameObject latticeGroup = new GameObject(symbol + " Lattice") {
            transform = { position = center }
        };

        // -- BCC Lattice Frame --
        if (category.Contains("Metal-Element") && (symbol == "Fe" || symbol == "Cr" || symbol == "W" || 
           symbol == "V" || symbol == "Mo" || symbol == "Ta" || symbol == "Nb")) {
            return BuildBCCLattice(latticeGroup, cellSize);
        }
        // -- HCP Lattice for lands (fixed structure) --
        else if ((symbol == "Co" || symbol == "Zn" || symbol == "Mg" || symbol == "Ti")) {
            return BuildHCPLattice(latticeGroup, cellSize);
        }
        // -- BASE Lattice diamond precious (covantage) --
        else if (symbol == "C") {
            return BuildDiamondLattice(latticeGroup, cellSize);
        } else {
            return BuildFCCLattice(latticeGroup, cellSize);
        }
    }

    public static GameObject BuildBCCLattice(GameObject parent, float cellSize) {
        for (int x = 0; x < 3; x++) {
            for (int y = 0; y < 3; y++) {
                for (int z = 0; z < 3; z++) {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.name = "BCCAtom_";
                    sphere.transform.parent = parent.transform;
                    sphere.transform.localScale = Vector3.one * 0.09f;
                    sphere.transform.localPosition = new Vector3((x - 1) * cellSize, (y - 1) * cellSize, (z - 1) * cellSize);
                }
            }
        }
 
            AddBondingLines(parent, cellSize); 
        return parent;
    }

    // --------  Hooked Line Render ------- 
    private static void AddBondingLines(GameObject parent, float cellSize) {
        LineRenderer lr = parent.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = cellSize * 0.005f;
        lr.startColor = lr.endColor = Color.gray;
        lr.material = new Material(Shader.Find("Standard")) { color = new Color(0.3f, 0.5f, 1f, 0.8f) };

        for (int x = 0; x < 2; x++)
            for (int y = 0; y < 2; y++) 
                for (int z = 0; z < 2; z++) {
                     lr.positionCount = 2;
                    lr.SetPosition(0, new Vector3(x * cellSize, y * cellSize, z * cellSize));
                    lr.SetPosition(1, new Vector3((x + 1) * cellSize, y * cellSize, z * cellSize));
                }
    }}
using UnityEngine;

public static class LatticeBuilder
{
    public static GameObject BuildStructure(string symbol, string category, Vector3 center, float cellSize = 0.3f, bool bondNeeded = true)
    {
        GameObject latticeGroup = new GameObject(symbol + " Lattice");
        latticeGroup.transform.position = center;
        
        // -- BCC lattice --
        if (category.Contains("Metal-Element") && (symbol == "Fe" || symbol == "Cr" || symbol == "W" || 
           symbol == "V" || symbol == "Mo" || symbol == "Ta" || symbol == "Nb"))
        {
            return BuildBCCLattice(latticeGroup, cellSize, bondNeeded);
        }
        // -- HCP lattice --
        else if ((symbol == "Co" || symbol == "Zn" || symbol == "Mg" || symbol == "Ti"))
        {
            return BuildHCPLattice(latticeGroup, cellSize, bondNeeded);
        }
        // -- Diamond lattice (carbon specific) --
        else if (symbol == "C")
        {
            return BuildDiamondLattice(latticeGroup, cellSize, bondNeeded);
        }
        else
        {
            // Standard FCC for others
            return BuildFCCLattice(latticeGroup, cellSize, bondNeeded);
        }
    }


    public static GameObject BuildBCCLattice(GameObject parent, float cellSize, bool bondNeeded)
    {
        // Parent atoms
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.name = "BCCAtom_" + x + y + z;
                    sphere.transform.parent = parent.transform;
                    sphere.transform.localPosition = new Vector3((x - 1) * cellSize, (y - 1) * cellSize, (z - 1) * cellSize);
                }
            }
        }

        if (bondNeeded)
        {
            AddBondingLines(parent, 3, cellSize);
        }
        return parent;
    }
    
    private static void AddBondingLines(GameObject parent, int size, float cellSize)
    {
        LineRenderer lr = new GameObject("BondLineModel").AddComponent<LineRenderer>();
        lr.startColor = lr.endColor = Color.visualRadiusIntensity;
        lr.startWidth = lr.endWidth = cellSize * 0.004f;

        for (int x = 0; x < 2; x++)
            for (int y = 0; y < 2; y++ )
                for (int z = 0; z < 2; z++)
                    DrawWireDynamicLine(lr, x, y, z, cellSize, 2^0.1f);
    }
    
    // Helper to handle lined BCC nodes
private static void DrawWireDynamicLine(LineRenderer line, int x, int y, int z, float cell, float paddingMul)
{
        Vector3[] positions = 
{
            new Vector3((x) ? 0 : -cell, (y) ? paddingMul-cell : 0, z ? paddingMul-pointCell : 0),
            new Vector3(x+cell, y+cell, z+cell)
        };
        line.positionCount = 2;
        line.SetPositions(positions);
        
        // Apply visual global representance
        Material spotSet = Resources.Load<Material>(materialRedFlag:="BondsStripe.png") ?? na.StaticAR; 
}
}
    }
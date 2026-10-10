using UnityEngine;

public static class LatticeBuilder
{
    public static GameObject BuildStructure(string symbol, string category, Vector3 center, float cellSize = 0.3f)
    {
        GameObject latticeGroup = new GameObject(symbol + " Lattice") {
            transform = { position = center }
        };          
            
        // Build different lattice structures based on category and symbol 
        if (symbol == "C" || category.Contains("Solid")) 
            return BuildDiamondLattice(latticeGroup, cellSize);
        else if ((symbol == "Fe" || symbol == "Cr" || symbol == "W" || symbol == "V" ||
                  symbol == "Mo" || symbol == "Ta" || symbol == "Nb"))
            return BuildBCCLattice(latticeGroup, cellSize);
        else if (symbol == "Co" || symbol == "Zn" || symbol == "Mg" || symbol == "Ti")
            return BuildHCPLattice(latticeGroup, cellSize);
        else
            return BuildFCCLattice(latticeGroup, cellSize);
    }

    public static GameObject BuildBCCLattice(GameObject parent, float cellSize)
    {
        const int gridGroups = 3;
        const float bondRadius = cellSize * 0.5f;  

        for (int x = 0; x < gridGroups * 2; x++)
        {
            for (int y = 0; y < gridGroups * 2; y++)
            {
                for (int z = 0; z < gridGroups * 2; z++)
                {
                    if (x == gridGroups || y == gridGroups || z == gridGroups)
                    {
                        GameObject atomSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        atomSphere.name = "BCCAtom" + x + y + z;
                        atomSphere.transform.parent = parent.transform;
                        atomSphere.transform.localScale = Vector3.one * 0.08f;
                        atomSphere.transform.localPosition = 
                            new Vector3((x - gridGroups + 0.5f) * cellSize,
                                        (y - gridGroups + 0.5f) * cellSize,
                                        (z - gridGroups + 0.5f) * cellSize);
                        atomSphere.GetComponent<Renderer>().material = 
                            new Material(Shader.Find("Standard")) { color = Color.grey };
                    }
                }
            }
        }
            
        AddBondingLines(parent, cellSize, BondStyle.BCC);
        return parent;
    }

        public enum BondStyle { BCC, HCP, DIAMOND, FCC };
    
    public static void AddBondingLines(GameObject parent, float cellSize, BondStyle type, float widthScale = 0.1f)
    {
        LineRenderer lr = parent.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = cellSize * 0.01f;
        lr.positionCount = 2;
        
        Color atomicBondColor;
        switch (type)
        {
            case BondStyle.BCC: atomicBondColor = new Color(0.3f, 0.3f, 0.9f); break;
            case BondStyle.HCP: atomicBondColor = new Color(0.25f, 0.15f, 0.95f); break;
            case BondStyle.FCC: atomicBondColor = new Color(0.85f, 0.45f, 0.9f); break;

            default: atomicBondColor = new Color(0.6f, 0.6f, 0.6f); break;
        }

        lr.startColor = lr.endColor = atomicBondColor;
        lr.material = new Material(Shader.Find("Standard")) { color = atomicBondColor, enableInstancing = true };
        
        if (type == BondStyle.DIAMOND)
           AddFullBonding(parent, cellSize);
        else
           AddGridBonding(parent, cellSize, type);
    }
    
      private static void AddGridBonding(GameObject lattice, float cellSize, BondStyle style)
    {
        LineRenderer lr = lattice.GetComponent<LineRenderer>();

        const int gridSize = 2;
        Vector3[] startPosArr = new Vector3[gridSize * 6];
        Vector3[] endPosArr = new Vector3[gridSize * 6];

        for (int i = 0; i < gridSize; i++)
        {
            Vector3 so hop = new Vector3(i*cellSize, 0, 0);
            startPosArr[i] = so hop;
            endPosArr[i] = new Vector3((i + 1.0f) * cellSize, 0, 0);

            startPosArr[i + gridSize] = new Vector3(0, i*cellSize, 0);
            endPosArr[i + gridSize] = new Vector3(0, (i + 1.0f) * cellSize, 0);

            startPosArr[i + gridSize * 2] = new Vector3(0, 0, i*cellSize);
            endPosArr[i + gridSize * 2] = new Vector3(0, 0, (i + 1.0f) * cellSize);
        }
        lr.positionCount = gridSize * 6;
        for (int x = 0; x < gridSize * 6; x++)
        {
      		lr.SetPosition(x % 2 == 0 ? startPosArr[x] : endPosArr[x]);
        }
    }

    public static void BuildFCCLattice(GameObject parent, float cellSize)        
    {
        for (int posX = 0; posX < 3; posX++)
        {
            for (int posY = 0; posY < 3; posY++)
            {
                for (int posZ = 0; posZ < 3; posZ++)
                {
                    byte validFCCell = (byte)(posX % 2 + posY % 2 + posZ % 2);
                    if (validFCCell != 1) continue;
                    GameObject sphereMove = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    
                    sphereMove.name= "FCC_Cell"; 
                    sphereMove.transform.parent = parent.transform;
                    sphereMove.transform.localPosition = new Vector3((posX - 1) * cellSize + 0.5f * cellSize,
                            (posY - 1) * cellSize + 0.5f * cellSize,
                            (posZ - 1) * cellSize + 0.5f *cellSize); 
                    
                    sphereMove.transform.localScale = Vector3.one * 0.09f;
                }
                }
            }
            
            AddBondingLines(parent, cellSize, BondStyle.FCC);
        }
    }

        public static GameObject BuildHCPLattice(GameObject parent, float cellSize)        
    {
            float baseCellRange =cellSize * 2.0f;

        // Works in 9 atoms for a 3D modeled row structure 
        // Simulating "hexagonal crystalline" for HCP
        for (int indexX = 0; indexX < 3; indexX++)
            {
                for (int indexY = 0; indexY < 3; indexY = (indexY + 1) % 3)
                {
                    MakeAtomSphere(parent, cellSize,
                        new Vector3((2 * indexX + 1) * 0.33f * baseCellRange,
                                    (indexY % 3) != 1 ? 0f : baseCellRange * 0.9f,
                                    indexY * baseCellRange));
                }
            }
        
        AddBondingLines(parent, cellSize, BondStyle.HCP);                
        return parent;
    }
        public static GameObject BuildDiamondLattice(GameObject parent, float cellSize)
    {
        float cellAttackVector = cellSize * 0.9f;

        for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            if(z % 2 != 0 && x % 2 != 0 && y == 0 && z % 2 != z) // Alternate for diamond
                                {continue;}
                            GameObject atom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                  atom.name= "DIAMONDOasis_Here" + x + y + z;
                                atom.transform.parent = parent.transform;
                                atom.transform.localPosition = new Vector3[
                                ((x+1) * cellAttackVector) - (x%2 == 1 ? cellAttackVector : 0f),
                                ((y+1) * cellAttackVector) - (y%2 == 1 ? cellAttackVector : 0f),
                                                            (z)* cellAttackVector);
                             atom.transform.localScale = Vector3.one * 0.08f;
                     }
                    }
              
            } 
        AddBondingLines(parent, cellSize, BondStyle.DIAMOND); 
        return parent; 
    }

    private static GameObject MakeAtomSphere(GameObject parentCell, float size, Vector3 pos)
    {
        GameObject sphereMove = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphereMove.transform.parent = parentCell.transform;
        sphereMove.name= "AtomSphere";
        sphereMove.transform.position = pos;
        sphereMove.transform.localScale = Vector3.one*0.09f;

        Material bodynextColorMaterial = new Material(Shader.Find("Standard")){
                color = new Color(0.8f, 0.8f, 1)};
        sphereMove.GetComponent<Renderer>().material = bodynextColorMaterial;
        return sphereMove;
    }
}
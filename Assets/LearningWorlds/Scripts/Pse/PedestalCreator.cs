using UnityEngine;

public static class PedestalCreator
{
    public static GameObject CreatePedestal(Transform parent = null, string name = "Base")
    {
        GameObject pedestalBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pedestalBase.name = name;

        // Position and scaling
        pedestalBase.transform.localScale = new Vector3(0.3f, 0.1f, 0.3f);
        pedestalBase.transform.localPosition = Vector3.zero;

        if (parent != null)
        {
            pedestalBase.transform.SetParent(parent);
            pedestalBase.transform.localEulerAngles = Vector3.one * 90f;
        }
        else
        {
            pedestalBase.transform.position = Vector3.zero;
        }

        // Smooth baked edges for pedestal
        Material mat = Resources.Load<Material>("Mat_TheEdge") as Material;
        if (mat)
        {
            pedestalBase.GetComponent<Renderer>().material = mat;
        }

        return pedestalBase;
    }
}
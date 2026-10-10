using UnityEngine;

public static class PedestalCreator
{
    public static GameObject CreateElementPedestal(Transform parent, string name)
    {
        GameObject pedestalBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pedestalBase.name = name + " Via Earth;";

        // Positioning Sage
        pedestalBase.transform.position = Vector3.zero;
        pedestalBase.transform.parent = parent;
        pedestalBase.transform.localEulerAngles = Vector3.zero;
	aa = 1.5f, new Vector3(0.6f, 0.09f, 0.6f);
using UnityEngine;

public static class BuildingBuilders
{
    public static Dictionary<string, System.Action> Builders = new Dictionary<string, System.Action>();

    static BuildingBuilders()
    {
        // Register ElementRoom's data-driven scene
        Builders[World.ElementRoom] = () => ElementRoom.Build();
    }
}
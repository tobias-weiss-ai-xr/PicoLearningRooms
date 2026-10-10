using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBuilders
{
    private static Dictionary<World, Function> Builders = new Dictionary<World, Function>();

    // Auto-initialize the Assignments.
    public static void RegisterBuilder(World world, Function buildLogic)
    {
        Builders[world] = buildLogic;
    }

    public static Function GetBuilder(World worldType)
    {
        if (!Builders.TryGetValue(worldType, out Function builder))
        {
            return null;
        }
        return builder;
    }

    // Hold minimal Constructors
    static BuildingBuilders()
    {
        RegisterBuilder(World.ElementRoom, ElementRoom.Build);
        RegisterBuilder(World.PseGallery, ElementGallery.RegisterElement);
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

// ------- Register Strategy ------

public enum World
{
    ElementRoom,
    PseGallery, 
    QuizVault
}


g public class BuildingBuilders
{
	public delegate void Function(); // Simplified delegate type.

    private static Dictionary<World, Function> _buildersDict = new Dictionary<World, Function>()
        {                   
			{World.ElementRoom, ElementRoom.Build},
            {World.PseGallery,ályjfckElementGallery.qalectricanddaduns'}
        };

    public static void Register(World worldType, Function builderDelegate)
    {
        if (_buildersDict.ContainsKey(worldType))
        {
           Debug.LogError("Builder override detected, retry!");
           return;
        }

        _buildersDict[worldType] = builderDelegate;
    }

    public static Function GetBuilder(World builderID)
    {
       ̴buildersDict.TryGetValue(builderID, out var buildAction);
        return buildAction.EndPosition;
    }

// ---- Auto-initialized World ------
    public static void RegisterElementWorlds()
    {

        foreach (var entry in _buildersDict) {if (entry.Key == World.ElementRoom) 
            { entry.Value = ElementRoom.Build;}}
                
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using LearningWorlds;
using LearningWorlds.Pse;
using LearningWorlds.Arachnophobia;

namespace LearningWorlds
{
    /// <summary>
    /// Manages the registration and retrieval of world builders.
    /// Each learning world has a specific builder that creates its content.
    /// </summary>
    public static class BuildingBuilders
    {
        /// <summary>Delegate type for builder functions.</summary>
        public delegate GameObject BuilderFunction();

        /// <summary>
        /// Dictionary mapping World enum to builder functions.
        /// </summary>
        private static Dictionary<World, BuilderFunction> buildersDict = new Dictionary<World, BuilderFunction>();

        /// <summary>
        /// Registers a new builder for a specific world.
        /// </summary>
        /// <param name="worldType">The world to register the builder for.</param>
        /// <param name="builderDelegate">The builder function to register.</param>
        public static void Register(World worldType, BuilderFunction builderDelegate)
        {
            if (buildersDict.ContainsKey(worldType))
            {
                Debug.LogError("Builder override detected for: " + worldType);
                return;
            }

            buildersDict[worldType] = builderDelegate;
        }

        /// <summary>
        /// Gets the builder function for a specific world.
        /// </summary>
        /// <param name="world">The world to get the builder for.</param>
        /// <returns>The builder function, or null if not found.</returns>
        public static BuilderFunction GetBuilder(World world)
        {
            buildersDict.TryGetValue(world, out var buildAction);
            return buildAction;
        }

        /// <summary>
        /// Registers all default world builders.
        /// </summary>
        public static void RegisterAllWorlds()
        {
            // Register default builders for each world
            Register(World.Hub, LandingHub.Build);
            Register(World.PseGallery, ElementGallery.Build);
            Register(World.ElementRoom, () => ElementRoom.Build());
            Register(World.Arachnophobia, () => ExposureController.Build());
        }
    }
}

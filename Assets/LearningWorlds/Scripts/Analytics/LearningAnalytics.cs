using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

/// <summary>
/// Singleton for logging learning interactions
/// </summary>
public class LearningAnalytics : MonoBehaviour
{
    public static LearningAnalytics Instance;
    
    private static List<InteractionLog> interactionLogs = new List<InteractionLog>();

    // Interaction log entry
    public class InteractionLog
    {
        public DateTime timestamp;
        public string world;
        public string action;
        public string context;
        public float duration;
        public int sudsRating;

        public InteractionLog(DateTime timestamp, string world, string action, string context, float duration, int sudsRating)
        {
            this.timestamp = timestamp;
            this.world = world;
            this.action = action;
            this.context = context;
            this.duration = duration;
            this.sudsRating = sudsRating;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Log an interaction event
 /// </summary>
 public void LogInteraction(string world, string action, string context, float duration, int sudsRating)
    {
        InteractionLog log = new InteractionLog(
            DateTime.Now,
            world,
            action,
            context,
            duration,
            sudsRating
        );
        interactionLogs.Add(log);
        Debug.Log($
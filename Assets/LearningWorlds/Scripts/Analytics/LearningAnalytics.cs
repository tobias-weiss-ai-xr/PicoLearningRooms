using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class LearningAnalytics : MonoBehaviour
{
    public class InteractionLog
    {
        public string timestamp;
        public string world;
        public string action;
        public string context;
        public float duration;
        public int sudsRating;

        public InteractionLog(string w, string a, string c, float d, int sr)
        {
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            world = w;
            action = a;
            context = c;
            duration = d;
            sudsRating = sr;
        }
    }

    private static List<InteractionLog> interactions = new List<InteractionLog>();

    private static LearningAnalytics _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LogInteraction(string world, string action, string context, float duration, int sudsRating)
    {
        var interaction = new InteractionLog(world, action, context, duration, sudsRating);
        interactions.Add(interaction);
        Debug.Log("Interaction Logged: " + interaction.timestamp + ", World: " + interaction.world);
    }

    public static void LogTutorInteraction(SocraticTutorManager.World world, string tutorName, string question, string answer, float duration)
    {
        LogInteraction(world.ToString(), "Tutor Interaction", 
            $"Tutor: {tutorName}, Question: {question}, Answer: {answer}", 
            duration, 0); // sudsRating not used in LogTutorInteraction
        Debug.Log("Tutor Interaction Logged: " + world + " with Tutor Id: " + tutorName);
    }
    {
        LogInteraction(world.ToString(), "Tutor", $
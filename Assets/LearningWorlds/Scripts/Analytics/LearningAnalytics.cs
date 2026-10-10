using UnityEngine;
using System.Collections;
using System;

public static class LearningAnalytics
{
    // Event log messages
    public static void LogTutorInteraction(object userID, string tutorName, string interactionType, string context)
    {
        string interactionEntry = $"[{DateTime.Now:O}] [{userID}] Tutor: {tutorName} | " +
                                $"Type: {interactionType} | Location: {context}";

        // In a full implementation, we would send this to an analytics server
        Debug.Log(interactionEntry);

        // Additional experimental data or metrics could go here
        // E.g., LogTutorTimeSpent(tutorName, timeElapsed);
    }

    public static void LogTutorResponse(string tutorName, string response, string world)
    {
        // Input validation
        if (string.IsNullOrEmpty(response) || string.IsNullOrEmpty(tutorName) || string.IsNullOrEmpty(world))
        {
            return;
        }

        // Format log entry
        string analyticEntry = $"[{DateTime.Now:O}] Tutor: {tutorName} | " +
                            $"World: {world} | Response Keywords: {ExtractKeywords(response)}";

        Debug.Log($"TUTOR_RESPONSE: {analyticEntry}");
    }

    // Extract based keywords or concepts from a tutor response for analytics
    private static string ExtractKeywords(string response)
    {
        // TODO: implement keyword extraction logic
        // Could use natural language processing or keyword matching
        // This is a placeholder
        string[] keywords = response.ToLower().Split(' ');
        if (keywords.Length > 10) keywords = new string[] { "..." }; // Limit for brevity
        return string.Join(", ", keywords);
    }
}
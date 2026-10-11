using UnityEngine;
using System.Text;

namespace LearningWorlds.Pse
{
    /// <summary>
    /// Utility class for text wrapping and formatting.
    /// </summary>
    public static class TextUtil
    {
        /// <summary>
        /// Wraps text to a specified line length.
        /// </summary>
        /// <param name="text">The text to wrap.</param>
        /// <param name="lineLength">Maximum characters per line (default: 40).</param>
        /// <returns>The wrapped text with newlines.</returns>
        public static string Wrap(string text, int lineLength = 40)
        {
            // Handle null or empty string
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogWarning("TextUtil.Wrap: Attempt to wrap null/empty text");
                return string.Empty;
            }

            // Normalize line breaks
            string cleanedText = text.Replace("\r\n", "\n").Replace("\r", "\n");
            cleanedText = cleanedText.Replace("\n\n", "\n"); // Remove double newlines

            StringBuilder wrappedOutput = new StringBuilder();
            int globalPosition = 0;

            while (globalPosition < cleanedText.Length)
            {
                // Find next wrap position
                int nextBreak = globalPosition + lineLength;
                if (nextBreak > cleanedText.Length)
                {
                    nextBreak = cleanedText.Length;
                }

                // Look backwards for the last space before lineLength
                while (nextBreak > globalPosition && !char.IsWhiteSpace(cleanedText[nextBreak - 1]))
                {
                    nextBreak--;
                }

                // If we didn't find a space, take the whole word
                if (nextBreak == globalPosition)
                {
                    nextBreak = Mathf.Min(globalPosition + lineLength, cleanedText.Length);
                    while (nextBreak < cleanedText.Length && !char.IsWhiteSpace(cleanedText[nextBreak]))
                    {
                        nextBreak++;
                    }
                }

                // Extract the line
                string line = cleanedText.Substring(globalPosition, nextBreak - globalPosition);
                line = line.Trim();

                if (line.Length > 0)
                {
                    wrappedOutput.AppendLine(line);
                }

                globalPosition = nextBreak;

                // Skip any whitespace at the start of the next line
                while (globalPosition < cleanedText.Length && char.IsWhiteSpace(cleanedText[globalPosition]))
                {
                    globalPosition++;
                }
            }

            return wrappedOutput.ToString().Trim();
        }

        /// <summary>
        /// Splits text into lines of maximum length.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <param name="maxLength">Maximum line length.</param>
        /// <returns>Array of lines.</returns>
        public static string[] SplitIntoLines(string text, int maxLength = 40)
        {
            return Wrap(text, maxLength).Split('\n');
        }
    }
}

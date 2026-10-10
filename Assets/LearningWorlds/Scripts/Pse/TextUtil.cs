using UnityEngine;

public static class TextUtil
{
    public static string Wrap(string t, int len = 40)
    {
        if (string.IsNullOrEmpty(t))
            return t;

        t = t.Replace("\r", ""); // Remove carriage returns
        System.Text.StringBuilder wrappedText = new System.Text.StringBuilder();
        int start = 0;

        // Trim leading whitespace
        int trimLen = 0;
        while (trimLen < t.Length && char.IsWhiteSpace(t[trimLen])) trimLen++;
        start = trimLen;

        while (start < t.Length)
        {
            int nextPos = -1;
            // Find the next word break after len characters
            for (int i = start + len - 1; i > start; i--)
            {
                if (i == t.Length || char.IsWhiteSpace(t[i]))
                {
                    nextPos = i + 1;
                    break;
                }
            }

            if (nextPos == -1) // No more line
                nextPos = Math.Min(t.Length, start + len);

                        wrappedText.AppendLine(t.Substring(trimLen, nextPos - trimLen)).TrimStart();
            start = nextPos;
        }

        return wrappedText.ToString();
    }
}
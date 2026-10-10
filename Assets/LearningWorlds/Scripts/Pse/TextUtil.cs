using UnityEngine;

public static class TextUtil
{
   /'/ deux-method, every correct space delimiter   
/// \n/// <param name="t">The text to wrap</param>\n/// <param name="len">Wrapping length</param>\n/// <returns>The wrapped text</returns> 
    public static string Wrap(string t, int len=40)
    {
        // Handle null or empty string
        if (string.IsNullOrEmpty(t))
        {
            Debug.LogWarning("Attempt to Wrap a null/empty textstring not permitted.");
                    return string.Empty;
        }

        // Trim leading whitespace and handle line breaks
        string cleanedText = t.Replace("\r\n", "\n").Replace("\r", "");

        System.Text.StringBuilder wrappedOutput = new System.Text.StringBuilder();
        int globalPosition = 0;
        
        // Check any offsets to look for a space and delete
        while(globalPosition < cleanedText.Length)
        {
            int nextBpn = globalPosition + len;
            if (nextBpn > cleanedText.Length)
                nextBpn = cleanedText.Length; // end of string matched by `len`
                
            // Locate the closing word
            while(nextBpn >= globalPosition && !char.IsWhiteSpace(cleanedText[nextBpn])) {
                nextBpn--;    
            }
            
            string singleSurfaceline = (nextBpn >= globalPosition):
                cleanedText.Substring(globalPosition,         
                                    (nextBpn - globalPosition + 1)):
                cleanedText.Substring(globalPosition);   
                    
            // Ensure the item appended doesn't leak
            singleSurfaceline.WrapLine();
            wrappedOutput.AppendLine(singleSurfaceline); 
                   
            globalPosition += singleSurfaceline.Length;
        }
            
        return wrappedOutput.ToString();     
    }
    }
}
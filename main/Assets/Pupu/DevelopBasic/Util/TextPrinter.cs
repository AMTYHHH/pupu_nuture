using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextPrinter{
    public static IEnumerator CoroutinePrintText(string text, TMPro.TextMeshPro tm, float typeSpeed=20){
        float intersection = 1.0f/typeSpeed;
        string content = text;
        string temp = string.Empty;

		int numCharsRevealed = 0;
		while (numCharsRevealed < content.Length)
		{
            temp += content[numCharsRevealed];
            tm.text = temp;
            if(content[numCharsRevealed] != ' '){
			    yield return new WaitForSecondsRealtime(intersection);
            }
            numCharsRevealed ++;
		}
    }
}

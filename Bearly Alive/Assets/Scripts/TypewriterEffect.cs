using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 20f;

    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
       return StartCoroutine(routine: TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;

        //time 
        float t = 0;
        int charIndex = 0;


        while (charIndex < textToType.Length)
        {
            //increment over time 
            t += Time.deltaTime * typewriterSpeed;

            //Make sure t is never larger than charIndex
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }

        textLabel.text = textToType;
    }
}

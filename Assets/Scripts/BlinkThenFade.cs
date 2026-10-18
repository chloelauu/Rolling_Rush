using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkThenFade : MonoBehaviour
{
    public int blinks = 3;            // how many times it blinks
    public float blinkTime = 0.25f;   // speed of each blink
    public float holdTime = 2f;       // stays visible after blinking
    public float fadeTime = 1f;       // how long the fade-out takes

    TMP_Text text;

    void OnEnable()
    {
        text = GetComponent<TMP_Text>();
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        text.alpha = 1f;

        // blink
        for (int i = 0; i < blinks; i++)
        {
            text.alpha = 0f;
            yield return new WaitForSeconds(blinkTime);
            text.alpha = 1f;
            yield return new WaitForSeconds(blinkTime);
        }

        // hold
        yield return new WaitForSeconds(holdTime);

        // fade out
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeTime;
            text.alpha = 1f - t;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
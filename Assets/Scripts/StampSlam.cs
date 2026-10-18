using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampSlam : MonoBehaviour
{
    public float delay = 0.4f;          // wait before stamping
    public float duration = 0.15f;      // how fast it slams down
    public float startScale = 2.5f;     // how big it starts
    public RectTransform shakeTarget;   // drag the Card here (optional)
    public float shakeAmount = 8f;

    CanvasGroup group;

    void OnEnable()
    {
        group = GetComponent<CanvasGroup>();
        if (group == null) group = gameObject.AddComponent<CanvasGroup>();
        StartCoroutine(Slam());
    }

    IEnumerator Slam()
    {
        // start big and invisible
        group.alpha = 0f;
        transform.localScale = Vector3.one * startScale;
        yield return new WaitForSecondsRealtime(delay);

        // slam down
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            float e = t * t;   // speeds up as it lands, like a real stamp
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, 1f, e);
            group.alpha = Mathf.Clamp01(t * 2f);
            yield return null;
        }
        transform.localScale = Vector3.one;
        group.alpha = 1f;

        // little shake on impact
        if (shakeTarget != null)
        {
            Vector2 home = shakeTarget.anchoredPosition;
            for (int i = 0; i < 6; i++)
            {
                shakeTarget.anchoredPosition = home + Random.insideUnitCircle * shakeAmount;
                yield return new WaitForSecondsRealtime(0.02f);
            }
            shakeTarget.anchoredPosition = home;
        }
    }
}
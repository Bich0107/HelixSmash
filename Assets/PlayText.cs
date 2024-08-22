using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class PlayText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float maxAlpha;
    [SerializeField] float minAlpha;
    [SerializeField] float changeTime;
    Color baseColor;

    void Start()
    {
        baseColor = text.color;
        PlayAnimation();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            text.enabled = false;
            StopAllCoroutines();
        }
    }
    
    public void Show()
    {
        text.enabled = true;
        PlayAnimation();
    }

    void PlayAnimation()
    {
        StartCoroutine(CR_Animation());
    }

    IEnumerator CR_Animation()
    {
        do
        {
            yield return StartCoroutine(CR_ChangeOpacity(minAlpha));
            yield return StartCoroutine(CR_ChangeOpacity(maxAlpha));
        } while (true);
    }

    IEnumerator CR_ChangeOpacity(float targetValue)
    {
        float startValue = text.color.a;

        float elapsedTime = 0f;
        Color color = text.color;
        while (elapsedTime < changeTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startValue, targetValue, elapsedTime / changeTime);
            text.color = color;
            yield return null;
        }

        color.a = targetValue;
        text.color = color;
    }

    public void Reset()
    {
        StopAllCoroutines();
        text.color = baseColor;
        text.enabled = true;
    }
}

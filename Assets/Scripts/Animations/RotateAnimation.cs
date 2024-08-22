using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    [SerializeField] Vector3 angle = new Vector3(0f, 5f, 0f);
    [SerializeField] float duration;
    [SerializeField] bool loop = true;
    [SerializeField] bool startOnEnable = true;

    void OnEnable()
    {
        if (startOnEnable) Play();
    }

    public void Initialize(Vector3 _angle, float _duration, bool _loop)
    {
        angle = _angle;
        duration = _duration;
        loop = _loop;
    }

    public void Play()
    {
        StartCoroutine(CR_Rotate());
    }

    IEnumerator CR_Rotate()
    {
        // rotate object by a angle on x, y and z axis
        float tick = 0f;
        do
        {
            if (!loop) tick += Time.deltaTime;

            Quaternion rotation = Quaternion.Euler(angle.x * Time.deltaTime, angle.y * Time.deltaTime, angle.z * Time.deltaTime);
            transform.rotation *= rotation;
            yield return null;
        } while (loop || tick < duration);
    }

    public void Stop()
    {
        StopAllCoroutines();
    }
}

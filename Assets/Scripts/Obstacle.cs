using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject obj; // Assign in inspector
    private bool hasFaded = false;

    public float fadeTime = 2.5f;
    public float visTime = 8f;
    public void Update()
    {
        if (!hasFaded)
        {
            StartCoroutine(Fade(obj, fadeTime));
        }
    }

    public IEnumerator Fade(GameObject obj, float seconds)
    {
        hasFaded = true;
        obj.SetActive(false);
        yield return new WaitForSeconds(seconds);
        obj.SetActive(true);
        yield return new WaitForSeconds(visTime); // Appear for one second
        hasFaded = false;
    }
}

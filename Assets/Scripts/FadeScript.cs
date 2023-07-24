using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] float fadeThreshold;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    
    void Update()
    {
        canvasGroup.alpha -= Time.deltaTime * fadeThreshold;
    }


} //class



















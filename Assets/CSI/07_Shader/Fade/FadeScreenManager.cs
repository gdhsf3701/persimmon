using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreenManager : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 0.5f;
    private readonly int _valueHash = Shader.PropertyToID("_Value");

    private void Awake()
    {
        _fadeImage.material = new Material(_fadeImage.material);
        HandleFadeEvent(true);
    }

    private void HandleFadeEvent(bool isFadeIn)
    {
        float fadeValue = isFadeIn ? 2.5f : 0f;
        float startValue = isFadeIn ? 0f : 2.5f;
        
        _fadeImage.material.SetFloat(_valueHash,startValue);
        
       
        
        var tweenCore = _fadeImage.material.DOFloat(fadeValue, _valueHash, _fadeDuration);

        if (isFadeIn == false)
        {
            tweenCore.OnComplete(() => print("Fade in"));
        }
    }
}

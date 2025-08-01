using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CSI._07_Shader.Fade
{
    public class FadeScreenManager : MonoBehaviour
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private Transform cirle;
        [SerializeField] private float _fadeDuration = 0.5f;
        private readonly int _valueHash = Shader.PropertyToID("_Value");

        private void Awake()
        {
            _fadeImage.material = new Material(_fadeImage.material);
            SetFade(2.5f);
        }

        // IEnumerator Start()
        // {
        //     var wait = new WaitForSeconds(5);
        //     while (true)
        //     {
        //         yield return wait;
        //         HandleFadeEvent(Random.Range(0, 2.2f));
        //     }
        // }
        public void SetFade(float value)
        {
            float clampvalue = Mathf.Clamp(value, 0, 2.2f);
        
            _fadeImage.material.SetFloat(_valueHash,clampvalue);
            
        
            var tweenCore = _fadeImage.material.DOFloat(clampvalue, _valueHash, _fadeDuration);
            clampvalue *= Camera.main.orthographicSize * 2;
            cirle.localScale = new Vector3(clampvalue, clampvalue, clampvalue);

        }
    }
}

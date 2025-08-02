using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Padeinout : MonoBehaviour
{
    [SerializeField]    
    private Image padePanel;
    [SerializeField]
    private float fadetime;
    private readonly int _valueHash = Shader.PropertyToID("_value");
    public static Padeinout _instance = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PadeOut();
    }

    [ContextMenu("fin")]
    public void PadeIn(string name = "")
    {
        padePanel.material.SetFloat(_valueHash, 0);
        var dotW =padePanel.material.DOFloat(5,_valueHash, fadetime).SetEase(Ease.InQuad);
        if (name != string.Empty)
        {
            dotW.OnComplete(() => SceneManager.LoadScene(name));
        }
    }
    [ContextMenu("fout")]
    public void PadeOut()
    {
        padePanel.material.SetFloat(_valueHash, 5);
        padePanel.material.DOFloat(0, _valueHash, fadetime).SetEase(Ease.OutQuad);
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}

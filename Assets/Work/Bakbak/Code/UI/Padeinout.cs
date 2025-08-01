using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Padeinout : MonoBehaviour
{
    [SerializeField]    
    private Image padePanel;
    [SerializeField]
    private float fadetime;
    private readonly int _valueHash = Shader.PropertyToID("_value");
    [ContextMenu("fin")]
    public void PadeIn()
    {
        padePanel.material.SetFloat(_valueHash, 0);
        padePanel.material.DOFloat(5,_valueHash, fadetime).SetEase(Ease.InQuad);
    }
    [ContextMenu("fout")]
    public void PadeOut()
    {
        padePanel.material.SetFloat(_valueHash, 5);
        padePanel.material.DOFloat(0, _valueHash, fadetime).SetEase(Ease.OutQuad);
    }
}

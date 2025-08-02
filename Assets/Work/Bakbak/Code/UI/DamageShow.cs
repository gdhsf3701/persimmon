using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageShow : MonoBehaviour
{
    [SerializeField]
    private float floatingHeight;
    [SerializeField]
    private float duration;
    [SerializeField]
    private TMP_Text text;
    public void Show(int damage)
    {
        transform.position = Camera.main.WorldToScreenPoint(transform.position);
        text.text = damage.ToString();
        transform.DOMoveY(transform.position.y + floatingHeight, duration)
            .OnComplete(()=>Destroy(gameObject));
    }
}

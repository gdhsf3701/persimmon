using System.Collections.Generic;
using UnityEngine;
using Work.Bakbak.Code.Shape;

public class HeartUI : MonoBehaviour
{
    [SerializeField]
    private GameObject heart;

    [SerializeField]
    private float heartwidth;

    private List<SpriteRenderer> hearts = new List<SpriteRenderer>();
    public void SetAllHeart(List<ShapType> shapes)
    {
        int i;
        for (i = 0; i < shapes.Count; i++)
        {
            if(i >= hearts.Count)
            {
                AddHeart();
            }
            hearts[i].sprite = ShapeGeter.Instance.GetShape(shapes[i]);
        }
        for(; i< hearts.Count; i++)
        {
            RemoveHeart(i);
        }
        SetUIPosition();
    }

    private void RemoveHeart(int idx)
    {
        Destroy(hearts[idx].gameObject);
        hearts.RemoveAt(idx);
    }

    private void AddHeart()
    {
        GameObject @object = Instantiate(heart, transform);
        SpriteRenderer spr = @object.GetComponent<SpriteRenderer>();
        hearts.Add(spr);
    }

    private void SetUIPosition()
    {
        float totalLength = (hearts.Count) * heartwidth;
        for (int i = 0; i < hearts.Count;i++) 
        {
            SpriteRenderer heart = hearts[i];
            heart.transform.localScale = Vector3.one * heartwidth;
            heart.transform.localPosition = new Vector2(i*heartwidth, 0);
        }
        transform.localPosition = new Vector3( -(totalLength - heartwidth)/2 , transform.localPosition.y);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartCounter : MonoBehaviour
{
    [SerializeField]
    private List<Image> heartPoints;

    private int currentHealth = 5;
    public void setUI()
    {
        int i = 0;
        for(; i < currentHealth; ++i)
        {
            heartPoints[i].sprite = Resources.Load<Sprite>("shapes/102_20250801235752");
        }
        for(; i < heartPoints.Count; ++i)
        {
            heartPoints[i].sprite = Resources.Load<Sprite>("shapes/102_20250802005501");
        }
    }

    public bool Damage()
    {
        currentHealth--;
        setUI();
        if(currentHealth <= 0)
        {
            return true;
        }
        return false;
    }
}

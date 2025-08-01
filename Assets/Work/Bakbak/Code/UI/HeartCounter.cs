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
            heartPoints[i].enabled = true;
        }
        for(; i < heartPoints.Count; ++i)
        {
            heartPoints[i].enabled =false;
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

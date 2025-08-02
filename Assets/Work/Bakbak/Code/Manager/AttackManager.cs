using csiimnida.CSILib.SoundManager.RunTime;
using System;
using UnityEngine;
using Work.Bakbak.Code.Shape;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public event Action<ShapType> OnAttack;
    public void Attack(ShapType shape)
    {
        OnAttack?.Invoke(shape);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}

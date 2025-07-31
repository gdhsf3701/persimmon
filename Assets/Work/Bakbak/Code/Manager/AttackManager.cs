using csiimnida.CSILib.SoundManager.RunTime;
using System;
using UnityEngine;
using Work.Bakbak.Code.Shape;

public class AttackManager : MonoSingleton<AttackManager>
{
    public event Action<ShapType> OnAttack;
    public void Attack(ShapType shape)
    {
        OnAttack?.Invoke(shape);
    }
}

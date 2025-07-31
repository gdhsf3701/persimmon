using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action OnDespawn;
    public void Spawned()
    {
        
    }
    public void Despawned()
    {
        OnDespawn?.Invoke();
    }
}

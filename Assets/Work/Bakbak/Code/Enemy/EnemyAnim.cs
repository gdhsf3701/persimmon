using UnityEngine;

public class EnemyAnim : MonoBehaviour, IEntityCompo
{
    private Enemy owner;

    private SpriteRenderer sprite;

    public void Desolve()
    {
        
    }

    public void Flip()
    {
        sprite.flipX = !sprite.flipX;
    }

    public void Initialize(Enemy enemy)
    {
        owner = enemy;
    }
}

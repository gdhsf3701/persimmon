using UnityEngine;

public class EnemyMover : MonoBehaviour, IEntityCompo
{
    private Enemy owner;
    private Vector2 moveDir = Vector2.zero;
    private float moveSpeed;
    void Update()
    {
        transform.position += (Vector3) moveDir * moveSpeed * Time.deltaTime;
    }

    public void SetDir(Vector2 dir)
    {
        moveDir = dir;
    }

    public void Initialize(Enemy enemy)
    {
        owner = enemy;
    }
}

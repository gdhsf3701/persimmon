using UnityEngine;

public class EnemyMover : MonoBehaviour, IEntityCompo
{
    private Enemy owner;
    private Vector2 moveDir = Vector2.zero;
    [SerializeField]
    private float moveSpeed;

    [SerializeField] private float dieUpSpeed = 1.25f;
    private bool acivate = false;
    void Update()
    {
        if(acivate == false)
            return;
        if (owner.IsDead)
        {
            transform.position += Vector3.up * dieUpSpeed * Time.deltaTime;
            return;
        }
        transform.position += (Vector3) moveDir * moveSpeed * Time.deltaTime;
    }

    public void SetTargetPos(Vector2 pos)
    {
        moveDir = (pos -(Vector2)transform.position).normalized;
    }

    public void Initialize(Enemy enemy)
    {
        acivate = true;
        owner = enemy;
        SetTargetPos(new Vector2(0, 0));
    }

    public void Desolve()
    {
        acivate = false ;
    }
}

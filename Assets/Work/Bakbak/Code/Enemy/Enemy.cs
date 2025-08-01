
using Plugins.ScriptFinder.RunTime.Finder;
using System;
using System.Threading.Tasks;
using moon._01.Script.Enemys;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public ScriptFinderSO PlayerFinder {get; private set;}
    [field: SerializeField] public ScriptFinderSO ComboFinder { get; private set; }
    public event Action OnHit;
    public event Action<Enemy> OnDeadEvent;

    private IEntityCompo[] Compos;

    [SerializeField] private EnemyAnimator enemyAnimator;
    [SerializeField] private EnemyMover enemyMover;

    [SerializeField]
    private int reward = 10;
    
    [SerializeField]
    private ParticleSystem particle;

    [SerializeField] private float attackCoolTime = 1f;

    [SerializeField]
    private GameObject indicater;

    private bool _dieAniEnd = false;

    private float _timer;

    public bool IsDead { get; private set; } = false;
    
    public int Reward { get => reward; private set => reward = value; }
    [HideInInspector]
    public bool isAttacking = false;

    public Player target;


    [ContextMenu("spawn")]
    public void Spawned()
    {
        target = PlayerFinder.GetTarget<Player>();
        SetCompo();
        enemyAnimator.ChangeAnimation("MOVE");
        enemyAnimator.OnDieAnimationEndEvent += DeadAniEnd;
        enemyAnimator.OnAttackAnimationEndEvent += AttackEnd;
        enemyAnimator.OnAttackEvent += AttackPlayer;
        enemyMover.OnAttackEvent += Attack;
        _timer = attackCoolTime;
    }

    public bool AttackTimeDone()
    {
        bool isCanAttack = _timer <= 0;
        return isCanAttack;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
    }

    private void Attack()
    {
        if(IsDead)
            return;
        enemyAnimator.ChangeAnimation("ATTACK");
    }

    private void AttackPlayer()
    {
        if(IsDead)
            return;
        PlayerFinder.GetTarget<Player>().Hit();
    }


    private void AttackEnd()
    {
        _timer = attackCoolTime;
        isAttacking = false;
        if(IsDead)
           return;
        enemyAnimator.ChangeAnimation("MOVE");
    }

    private void DeadAniEnd()
    {
        _dieAniEnd = true;
    }

    private void SetCompo()
    {
        Compos = GetComponentsInChildren<IEntityCompo>();
        foreach (IEntityCompo compo in Compos)
        {
            compo.Initialize(this);
        }
    }

    public async Task OnDead()
    {
        if(IsDead)
            return;
        IsDead = true;
        particle.Play();
        
        enemyAnimator.ChangeAnimation("DEAD");
        
        ComboFinder.GetTarget<ComboManager>().Kill(this);

        enemyMover.OnAttackEvent -= Attack;
        
        enemyAnimator.OnAttackAnimationEndEvent -= AttackEnd;
        
        await WaitToDeadAniEnd();
        
        OnDeadEvent?.Invoke(this);
        
        foreach (IEntityCompo compo in Compos)
        {
            compo.Desolve();
        }
        enemyAnimator.OnDieAnimationEndEvent -= DeadAniEnd;
        Destroy(gameObject);
    }

    private async Task WaitToDeadAniEnd()
    {
        while (true)
        {
            if (_dieAniEnd)
                break;
            await Task.Delay(200);
        }
    }
}

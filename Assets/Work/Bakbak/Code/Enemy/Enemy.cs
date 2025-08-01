using Plugins.ScriptFinder.RunTime.Finder;
using System;
using System.Threading.Tasks;
using moon._01.Script.Enemys;
using UnityEngine;
using UnityEngine.Events;
using Work.Bakbak.Code.Shape;

public class Enemy : MonoBehaviour
{
    public event Action OnHit;
    public event Action<Enemy> OnDeadEvent;

    private IEntityCompo[] Compos;

    [SerializeField] private EnemyAnimator enemyAnimator;

    [SerializeField]
    private int reward = 10;

    private ScriptFinderSO finder;
    [SerializeField]
    private ParticleSystem particle;

    [SerializeField]
    private GameObject indicater;

    private bool _dieAniEnd = false;

    public bool IsDead { get; private set; } = false;
    
    public int Reward { get => reward; private set => reward = value; }

    public Player target;


    [ContextMenu("spawn")]
    public void Spawned()
    {
        finder = Resources.Load<ScriptFinderSO>("Finder/PlayerFinder");
        target = finder.GetTarget<Player>();
        SetCompo();
        enemyAnimator.ChangeAnimation("MOVE");
        enemyAnimator.OnDieAnimationEndEvent += DeadAniEnd ;
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
        
        finder.GetTarget<ComboManager>().Kill(this);
        
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

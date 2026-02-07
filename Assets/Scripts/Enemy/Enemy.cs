using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Quest Info")]
    public string questTargetId;


    public Entity_Stats stats { get; private set; }
    public Enemy_Health health { get; private set; }
    public Entity_Combat combat { get; private set; }
    public Entity_VFX vfx { get; private set; }
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;

    [Header("Battle details")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float attackCooldown = .5f;
    public bool canChasePlayer = true;
    [Space]
    public float battleTimeDuration = 5;
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("Stunned state details")]
    public float stunnedDuration = 1;
    public Vector2 stunnedVelocity = new Vector2(7, 7);
    [SerializeField] protected bool canBeStunned;

    [Header("Movement details")]
    public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0,2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("Player detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    public Transform player { get; private set; }
    public float activeSlowMultiplier { get; private set; } = 1;

    public float GetMoveSpeed() => moveSpeed * activeSlowMultiplier;
    public float GetBattleMoveSpeed() => battleMoveSpeed * activeSlowMultiplier;

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Enemy_Health>();
        stats = GetComponent<Entity_Stats>();
        combat = GetComponent<Entity_Combat>();
        vfx = GetComponent<Entity_VFX>();
    }

    public void MakeUntargetable(bool canBeTargeted)
    {
        if(canBeTargeted == false)
            gameObject.layer = LayerMask.NameToLayer("Untargetable");
        else
            gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    public virtual void SpecialAttack()
    {

    }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {

        activeSlowMultiplier = 1 - slowMultiplier;


        anim.speed = anim.speed * activeSlowMultiplier;

        yield return new WaitForSeconds(duration);
        StopSlowDown();
    }

    public override void StopSlowDown()
    {
        activeSlowMultiplier = 1;
        anim.speed = 1;
        base.StopSlowDown();
    }


    public void EnableCounterWindow(bool enable) => canBeStunned = enable;

    public override void EntityDeath()
    {
        base.EntityDeath();

        stateMachine.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    public virtual void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState)
            return;

        if (stateMachine.currentState == attackState)
            return;

        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public void DestroyGameObjectWithDelay(float delay = 10)
    {
        Destroy(gameObject, delay);
    }

    public Transform GetPlayerReference()
    {
        if (player == null)
            player = PlayerDetected().transform;

        return player;
    }

    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * playerCheckDistance), playerCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * attackDistance), playerCheck.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * minRetreatDistance), playerCheck.position.y));

    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
       Player.OnPlayerDeath -= HandlePlayerDeath;  
    }
}

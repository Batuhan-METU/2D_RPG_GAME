using System.Collections;
using UnityEngine;

public class Enemy_Reaper : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_ReaperAttackState reaperAttackState { get; private set; }
    public Enemy_ReaperBattleState reaperBattleState { get; private set; }
    public Enemy_ReaperTeleportState reaperTeleportState { get; private set; }
    public Enemy_ReaperSpellCastState reaperSpellCastState { get; private set; }

    [Header("Reaper specifics")]
    public float maxBattleIdleTime = 5;

    [Header("Reaper Spellcast")]
    [SerializeField] private DamageScaleData spellDamageScale;
    [SerializeField] private GameObject spellCastPrefab;
    [SerializeField] private int amountToCast = 6;
    [SerializeField] private float spellCastRate = 1.2f;
    [SerializeField] private float spellCastStateCooldown = 10;
    [SerializeField] private Vector2 playerOffsetPrediction;
    private float lastTimeCastedSpells = float.NegativeInfinity;
    public bool spellCastPreformed { get; private set; }
    private Player playerScript;

    [Header("Reapert Teleport")]
    [SerializeField] private BoxCollider2D arenaBounds;
    [SerializeField] private float offsetCenterY = 1.725f;
    [SerializeField] private float chanceToTeleport = .25f;
    private float defaultTeleportChance;
    public bool teleporTrigger { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");

        reaperBattleState = new Enemy_ReaperBattleState(this, stateMachine, "battle");
        reaperAttackState = new Enemy_ReaperAttackState(this, stateMachine, "attack");
        reaperTeleportState = new Enemy_ReaperTeleportState(this, stateMachine, "teleport");
        reaperSpellCastState = new Enemy_ReaperSpellCastState(this, stateMachine, "spellCast");

        battleState = reaperBattleState;
    }

    protected override void Start()
    {
        base.Start();

        arenaBounds.transform.parent = null;
        defaultTeleportChance = chanceToTeleport;

        stateMachine.Initialize(idleState);
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

    public override void SpecialAttack()
    {
        StartCoroutine(CastSpellCo());
    }

    private IEnumerator CastSpellCo()
    {
        if (playerScript == null)
            playerScript = player.GetComponent<Player>();

        for (int i = 0; i < amountToCast; i++)
        {
            bool playerMoving = playerScript.rb.linearVelocity.magnitude > 0;

            float xOffset = playerMoving ? playerOffsetPrediction.x * playerScript.facingDir : 0;
            Vector3 spellPosition = player.transform.position + new Vector3(xOffset, playerOffsetPrediction.y);

            Enemy_ReaperSpell spell
                = Instantiate(spellCastPrefab, spellPosition, Quaternion.identity).GetComponent<Enemy_ReaperSpell>();

            spell.SetupSpell(combat, spellDamageScale);

            yield return new WaitForSeconds(spellCastRate);
        }

        SetSpellCastPreformed(true);
    }
    public void SetSpellCastPreformed(bool spellCastStatus) => spellCastPreformed = spellCastStatus;
    public bool CanDoSpellCast() => Time.time > lastTimeCastedSpells + spellCastStateCooldown;
    public void SetSpellCastOnCooldown() => lastTimeCastedSpells = Time.time;

    public bool ShouldTeleport()
    {
        if (Random.value < chanceToTeleport)
        {
            chanceToTeleport = defaultTeleportChance;
            return true;
        }

        chanceToTeleport = chanceToTeleport + .05f;
        return false;
    }

    public void SetTeleportTrigger(bool triggerStatus) => teleporTrigger = triggerStatus;

    public Vector3 FindTeleportPoint()
    {
        int maxAttampts = 10;
        float bossWithColliderHalf = col.bounds.size.x / 2;

        for (int i = 0; i < maxAttampts; i++)
        {
            float randomX = Random.Range(arenaBounds.bounds.min.x + bossWithColliderHalf,
                                         arenaBounds.bounds.max.x - bossWithColliderHalf);

            Vector2 raycastPoint = new Vector2(randomX, arenaBounds.bounds.max.y);

            RaycastHit2D hit = Physics2D.Raycast(raycastPoint, Vector2.down, Mathf.Infinity, whatIsGround);

            if (hit.collider != null)
                return hit.point + new Vector2(0, offsetCenterY);
        }


        return transform.position;
    }
}

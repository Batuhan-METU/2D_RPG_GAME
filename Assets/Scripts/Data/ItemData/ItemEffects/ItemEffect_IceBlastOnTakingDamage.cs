using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Ice blast", fileName = "Item effect data - Ice blast on taking damage")]

public class ItemEffect_IceBlastOnTakingDamage : ItemEffect_DataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;

    [Space]
    [SerializeField] private float healthPercentTrigger = .25f;
    [SerializeField] private float cooldown;
    private float lastTimeUsed = -999;
    [Header("Vfx Objects")]
    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject onHitVfx;


    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercentTrigger;

        if (noCooldown && reachedThreshold)
        {
            player.vfx.CreateEffectOf(iceBlastVfx, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (var target in enemies)
        {
            IDamageable damagable = target.GetComponent<IDamageable>();

            if (damagable == null) continue;

            bool targetGotHit = damagable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice, effectData);

            if (targetGotHit)
                player.vfx.CreateEffectOf(onHitVfx, target.transform);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTakingDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }

    private void OnEnable()
    {
        lastTimeUsed = -999;
    }

}

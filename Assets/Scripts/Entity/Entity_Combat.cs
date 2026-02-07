using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;

    private Entity_SFX sfx;
    private Entity_VFX vfx;
    private Entity_Stats stats;

    public DamageScaleData basicAttackScale;

    [Header("Target detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;


    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        sfx = GetComponent<Entity_SFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        bool targetGotHit = false;

        foreach (var target in GetDetectedColliders(whatIsTarget))
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue; // skip target, go to next target

            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();


            float physicalDamage = attackData.phyiscalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            targetGotHit = damageable.TakeDamage(physicalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            if (targetGotHit)
            {
                OnDoingPhysicalDamage?.Invoke(physicalDamage);
                vfx.CreateOnHitVFX(target.transform, attackData.isCrit, element);
                sfx?.PlayAttackHit();
            }
        }

        if (targetGotHit == false)
            sfx?.PlayAttackMiss();
    }

    public void PerformAttackOnTarget(Transform target,DamageScaleData damageScaleData = null)
    {
        bool targetGotHit = false;


        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable == null)
            return; // skip target, go to next target

        DamageScaleData damageScale = damageScaleData == null ? basicAttackScale : damageScaleData;
        AttackData attackData = stats.GetAttackData(basicAttackScale);
        Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();


        float physicalDamage = attackData.phyiscalDamage;
        float elementalDamage = attackData.elementalDamage;
        ElementType element = attackData.element;

        targetGotHit = damageable.TakeDamage(physicalDamage, elementalDamage, element, transform);

        if (element != ElementType.None)
            statusHandler?.ApplyStatusEffect(element, attackData.effectData);

        if (targetGotHit)
        {
            OnDoingPhysicalDamage?.Invoke(physicalDamage);
            vfx.CreateOnHitVFX(target.transform, attackData.isCrit, element);
            sfx?.PlayAttackHit();
        }


        if (targetGotHit == false)
            sfx?.PlayAttackMiss();
    }

    protected Collider2D[] GetDetectedColliders(LayerMask whatToDetect)
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatToDetect);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}

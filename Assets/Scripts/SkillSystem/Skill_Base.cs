using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager { get; private set; }
    public Player player { get ; private set; }
    public DamageScaleData damageScaleData { get; private set; }


    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        skillManager = GetComponentInParent<Player_SkillManager>();
        player = GetComponentInParent<Player>();
        lastTimeUsed = lastTimeUsed - cooldown;
        damageScaleData = new DamageScaleData();
    }

    public virtual void TryUseSkill()
    {

    }

    public void SetSkillUpgrade(SkillDataSO skillData)
    {
        UpgradeData upgrade = skillData.upgradeData;
        upgradeType = upgrade.upgradeType;
        cooldown = upgrade.cooldown;
        damageScaleData = upgrade.damageScaleData;



        player.ui.inGameUI.GetSkillSlot(skillType).SetupSkillSlot(skillData);
        ResetCooldown();
    }

    public virtual bool CanUseSkill()
    {
        if(upgradeType == SkillUpgradeType.None)
            return false;

        if (OnCooldown())
        {
            Debug.Log("On Cooldown");
            return false;
        }

        return true;    
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;
    public SkillUpgradeType GetUpgrade() => upgradeType;
    public SkillType GetSkillType() => skillType;


    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).StartCooldown(cooldown);
        lastTimeUsed = Time.time;
    }
    public void ReduceCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    public void ResetCooldown()
    {
        player.ui.inGameUI.GetSkillSlot(skillType).ResetCooldown();
        lastTimeUsed = Time.time - cooldown;
    }
}

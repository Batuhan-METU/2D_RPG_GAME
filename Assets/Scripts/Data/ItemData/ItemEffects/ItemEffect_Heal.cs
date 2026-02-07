using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Heal effect", fileName = "Item effect data - Heal")]

public class ItemEffect_Heal : ItemEffect_DataSO
{
    [SerializeField] private float healPercent = .1f;

    public override void ExecuteEffect()
    {
        Player player = FindFirstObjectByType<Player>();

        float healAmount = player.stats.GetMaxHealth() * healPercent;

        player.health.IncreaseHealth(healAmount);
    }
}

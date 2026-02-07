using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Grand skill point", fileName = "Item effect data - Grant Skill Point")]

public class ItemEffect_GrantSkillPoint : ItemEffect_DataSO
{
    [SerializeField] private int pointsToAdd;

    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(pointsToAdd);
    }
}

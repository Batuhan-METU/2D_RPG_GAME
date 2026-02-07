using UnityEditor;
using UnityEngine;
public enum RewardType {  Merchant, Blacksmith, None};
public enum QuestType { Kill, Talk, Delivery}

[CreateAssetMenu(menuName = "RPG Setup/Quest Data/New Quest", fileName = "Quest - ")]
public class QuestDataSO : ScriptableObject
{
    public string questSaveId;
    [Space]
    public QuestType questType;
    public string questName;
    [TextArea] public string description;
    [TextArea] public string questGoal;

    public string questTargetId; // Enemy name, NPC name, ItemName
    public int requiredAmount;
    public ItemDataSO itemToDeliver; // Used only if quest type is Delivery

    [Header("Reward")]
    public RewardType rewardType;
    public Inventory_Item[] rewardItems;

    private void OnValidate()
    {

#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        questSaveId = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}

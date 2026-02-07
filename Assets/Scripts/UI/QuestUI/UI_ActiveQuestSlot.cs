using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActiveQuestSlot : MonoBehaviour
{
    private QuestData questInSlot;
    private UI_ActiveQuestPreviw questPreviw;

    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image[] questRewardPreviw;

    public void SetupActiveQuestSlot(QuestData questToSetup)
    {
        questPreviw = transform.root.GetComponentInChildren<UI_ActiveQuestPreviw>();
        questInSlot = questToSetup;

        questName.text = questToSetup.questDataSO.questName;

        Inventory_Item[] reward = questToSetup.questDataSO.rewardItems;

        foreach (var previwIcon in questRewardPreviw)
        {
            previwIcon.gameObject.SetActive(false);
        }

        for (int i = 0; i < reward.Length; i++)
        {
            if (reward[i] == null) continue;
            Image previw = questRewardPreviw[i];

            previw.gameObject.SetActive(true);
            previw.sprite = reward[i].itemData.itemIcon;
            previw.GetComponentInChildren<TextMeshProUGUI>().text = reward[i].stackSize.ToString();
        }
    }

    public void SetupPreviwBTN()
    {
        questPreviw.SetupQuestPreviw(questInSlot);
    }
}

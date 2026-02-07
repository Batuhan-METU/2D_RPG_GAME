using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    private Inventory_Player inventory;

    private UI_CraftPreviw craftPreviwUI;
    private UI_CraftSlot[] craftSlots;
    private UI_CraftListButton[] craftListButtons;

    

    public void SetupCraftUI(Inventory_Storage storage)
    {
        inventory = storage.playerInventory;
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();

        craftPreviwUI = GetComponentInChildren<UI_CraftPreviw>(true);
        craftPreviwUI.SetupCraftPreviw(storage);
        SetupCraftListButtons();
    }

    private void SetupCraftListButtons()
    {
        craftSlots = GetComponentsInChildren<UI_CraftSlot>(true);
        craftListButtons = GetComponentsInChildren<UI_CraftListButton>(true);

        foreach(var slot in craftSlots)
            slot.gameObject.SetActive(false);

        foreach(var button in craftListButtons)
            button.SetCraftSlots(craftSlots);
    }

    private void UpdateUI() => inventoryParent.UpdateSlots(inventory.itemList);

}

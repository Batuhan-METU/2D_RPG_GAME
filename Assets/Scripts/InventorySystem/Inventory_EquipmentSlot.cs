using System;
using UnityEngine;

[Serializable]
public class Inventory_EquipmentSlot 
{
    public ItemType slotType;
    public Inventory_Item equipedItem;

    public Inventory_Item GetEquipedItem() => equipedItem;
    public bool HasItem() => equipedItem != null && equipedItem.itemData != null;
}

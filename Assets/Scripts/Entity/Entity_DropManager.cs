using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity_DropManager : MonoBehaviour
{
    [SerializeField] private GameObject itemDropPrefab;
    [SerializeField] private ItemListDataSO dropData;

    [Header("Drop restrctions")]
    [SerializeField] private int maxRarityAmount = 1200;
    [SerializeField] private int maxItemsToDrop = 3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            DropItems();
    }

    public virtual void DropItems()
    {
        if (dropData == null)
        {
            Debug.Log("You need to assign drop data on entity" + gameObject.name);
            return;
        }

        List<ItemDataSO> itemsToDrop = RollDrops();
        int amountToDrop = Mathf.Min(itemsToDrop.Count, maxItemsToDrop);

        for (int i = 0; i < amountToDrop; i++)
        {
            CreateItemDrop(itemsToDrop[i]);
        }

    }

    public void CreateItemDrop(ItemDataSO itemToDrop)
    {
        GameObject newItem = Instantiate(itemDropPrefab,transform.position,Quaternion.identity);
        newItem.GetComponent<Object_ItemPickup>().SetupItem(itemToDrop);
    }

    public List<ItemDataSO> RollDrops()
    {
       

        List<ItemDataSO> possibleDrops = new List<ItemDataSO>();
        List<ItemDataSO> finalDrops = new List<ItemDataSO>();
        float maxRarityAmount = this.maxRarityAmount;

        // Step 1: Roll each item based on rarity and max drop chance
        foreach (var item in dropData.itemList)
        {
            float dropChance = item.GetDropChance();

            if(Random.Range(0,100) <= dropChance)
                possibleDrops.Add(item);
        }

        // Step 2: Sort by rarity (highest to lowest)
        possibleDrops = possibleDrops.OrderByDescending(item => item.itemRarity).ToList();

        // Step 3: Add items to final drop list until rarity limit on entity is reached

        foreach (var item in possibleDrops) 
        {                                   
            if (maxRarityAmount > item.itemRarity)
            {
                finalDrops.Add(item);
                maxRarityAmount = maxRarityAmount - item.itemRarity;
            }
        }

        return finalDrops;
    }
}

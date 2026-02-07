using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBuff = new List<string>();
    private Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory_Player>();
    }

    public bool CanApplyBuffOf(string source)
    {
        return activeBuff.Contains(source) == false;
    }

    public void ApplyBuff(BuffEffectData[] buffsToAply, float duration, string source)
    {
        StartCoroutine(BuffCo(buffsToAply, duration, source));
    }

    private IEnumerator BuffCo(BuffEffectData[] buffsToApply, float duration, string source)
    {
        activeBuff.Add(source);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).AddModifier(buff.value, source);
        }

        yield return new WaitForSeconds(duration);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).RemoveModifier(source);
        }

        inventory.TriggerUpdateUI();
        activeBuff.Remove(source);
    }
}

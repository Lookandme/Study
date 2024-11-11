using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStatModifiers : PickUpItem
{
    [SerializeField] private List<CharacterStat> statsModifier;
    protected override void OnPickUp(GameObject go)
    {
        CharacterStatHandler statHandler = go.GetComponent<CharacterStatHandler>();

        foreach (CharacterStat stat in statsModifier)
        {
            statHandler.AddStatModifier(stat);
        }

        // HPBar Refresh¿ë
        HealthSystem healthSystem = go.GetComponent<HealthSystem>();
        healthSystem.ChangeHealth(0);
    }
}

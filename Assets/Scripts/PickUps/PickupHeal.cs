using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeal : PickUpItem
{
    [SerializeField] int healValue = 10;
    protected override void OnPickUp(GameObject go)
    {
        HealthSystem healthSystem = go.GetComponent<HealthSystem>();
        healthSystem.ChangeHealth(healValue);
    }
}

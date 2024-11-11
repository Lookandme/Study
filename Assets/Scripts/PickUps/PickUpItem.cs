using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPickUp(collision. gameObject);
        if (pickUpSound != null)SoundManager.PlayClip(pickUpSound);
        Destroy(gameObject);
    }

    protected abstract void OnPickUp(GameObject gameObject);
    
        
}

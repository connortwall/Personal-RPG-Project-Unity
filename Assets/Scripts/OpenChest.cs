using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

namespace CW
{
    
public class OpenChest : Interactable
{
    private Animator animator;
    private OpenChest openChest;
    
    public Transform playerChestStandingPosition;
    public GameObject itemSpawner;
    public WeaponItem weaponInChest;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        openChest = GetComponent<OpenChest>();
    }

    public override void Interact(PlayerManager playerManager)
    {
        // rotate player towards chest
        Vector3 rotationDirection = transform.position - playerManager.transform.position;
        rotationDirection.y = 0;
        rotationDirection.Normalize();
        Quaternion tr = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
        playerManager.transform.rotation = targetRotation;

        // lock players transform to position near chest
        playerManager.OpenChestInteraction(playerChestStandingPosition);

        // open chest lid, simultanously animaite player opening chest
        animator.Play("Chest_Open");
        
        // spawn an item inside the chest that can be picked up
        // use coroutine
        StartCoroutine(SpawnItemInChest());
        
        
        WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();
        if (weaponPickUp != null)
        {
            weaponPickUp.weaponItem = weaponInChest;
        }
    }

    private IEnumerator SpawnItemInChest()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(itemSpawner, transform);
        Destroy(openChest);
    }
}
}
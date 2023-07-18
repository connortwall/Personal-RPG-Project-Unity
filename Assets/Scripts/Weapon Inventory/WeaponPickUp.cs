using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CW
{
 
public class WeaponPickUp : Interactable
{
 public WeaponItem weaponItem;
 public override void Interact(PlayerManager playerManager)
 {
  base.Interact(playerManager);
  PickUpItem(playerManager);
 }

 private void PickUpItem(PlayerManager playerManager)
 {
  PlayerInventoryManager playerInventoryManager;
  PlayerLocomotionManager playerLocomotionManager;
  PlayerAnimatorManager playerAnimatorManager;
  
  playerInventoryManager = playerManager.GetComponent<PlayerInventoryManager>();
  playerLocomotionManager = playerManager.GetComponent<PlayerLocomotionManager>();
  playerAnimatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();
  
  // stops the player from moving whilst picking up item
  playerLocomotionManager.rigidbody.velocity = Vector3.zero;
  playerAnimatorManager.PlayTargetAnimation("Picking Up Object", true);
  playerInventoryManager.weaponsInventory.Add(weaponItem);
  // set the pop up text to the weapon text
  playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weaponItem.itemName;
  playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weaponItem.itemIcon.texture;
  playerManager.itemInteractableGameObject.SetActive(true);
  Destroy(gameObject);
 }
}
}
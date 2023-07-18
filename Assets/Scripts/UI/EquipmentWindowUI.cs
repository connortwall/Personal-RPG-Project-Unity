using UnityEngine;

namespace CW
{
  
public class EquipmentWindowUI : MonoBehaviour
{
  public bool rightHandSlot01Selected;
  public bool rightHandSlot02Selected;
  public bool leftHandSlot01Selected;
  public bool leftHandSlot02Selected;

  public HandEquipmentSlotUI[] handEquipmentSlotUI;

  private void Start()
  {// unity doesnt like this
    //handEquipmentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>();
  }
  
  public void LoadWeaponOnEquipmentScreen(PlayerInventoryManager playerInventoryManager)
  {
    // load equipment into UI
    for (int i = 0; i < handEquipmentSlotUI.Length - 1; i++)
    {
      if (handEquipmentSlotUI[i].rightHandSlot01)
      {
        handEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInRightHandSlots[0]);
      }
      else if(handEquipmentSlotUI[i].rightHandSlot02)
      {
        handEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInRightHandSlots[1]);
      }
      else if (handEquipmentSlotUI[i].leftHandSlot01)
      {
        handEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInLeftHandSlots[0]);
      }
      else if(handEquipmentSlotUI[i].leftHandSlot02)
      {
        handEquipmentSlotUI[i].AddItem(playerInventoryManager.weaponsInLeftHandSlots[1]);
      }
      else
      {
        return;
      }
    }
  }

  public void SelectRightHandSlot01()
  {
    rightHandSlot01Selected = true;
  }
  public void SelectRightHandSlot02()
  {
    rightHandSlot02Selected = true;
  }
  
  public void SelectLeftHandSlot01()
  {
    leftHandSlot01Selected = true;
  }
  
  public void SelectLeftHandSlot02()
  {
    rightHandSlot02Selected = true;
  }
  
  
}
}

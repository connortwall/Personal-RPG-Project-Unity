using UnityEngine;

namespace CW
{
    
public class UIManager : MonoBehaviour
{
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    public EquipmentWindowUI equipmentWindowUI;
    public QuickSlotsUI quickSlotsUI;
    
    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject selectWindow;
    public GameObject equipmentScreenWindow;
    public GameObject weaponInventoryWindow;

    [Header("Equipment Window Slot Selected")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    private WeaponInventorySlot[] weaponInventorySlots;


    private void Awake()
    {
        playerInventoryManager = FindObjectOfType<PlayerInventoryManager>();
        quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
        //TODO: Unity doesnt like this:
        //equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
    }

    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        // load weapons on screen
        equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventoryManager);
        quickSlotsUI.UpdateCurrentSpellIcon(playerInventoryManager.currentSpell);
        quickSlotsUI.UpdateCurrentConsumableIcon(playerInventoryManager.currentConsumableItem);

    }

    public void UpdateUI()
    {
        #region Weapon Inventory Slots
        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if (i < playerInventoryManager.weaponsInventory.Count)
            {
                if (weaponInventorySlots.Length < playerInventoryManager.weaponsInventory.Count)
                {
                    // instantiate a prefab
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }
                weaponInventorySlots[i].AddItem(playerInventoryManager.weaponsInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySLot();
            }
        }
        #endregion
    }
    public void OpenSelectWindow()
    {
        selectWindow.SetActive(true);
    }

    public void CloseSelectWindow()
    {
        selectWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        ResetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        equipmentScreenWindow.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;

    }
}
}
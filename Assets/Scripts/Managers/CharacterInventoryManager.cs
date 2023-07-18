using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        protected CharacterWeaponSlotManager characterWeaponSlotManager;
    
    [Header("Quick Slots")]
    public SpellItem currentSpell;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public WeaponItem unarmedWeapon;
    public ConsumableItem currentConsumableItem;

    [Header("Current Equipment")] 
    public HelmetEquipment currentHelmetEquipment;
    public TorsoEquipment currentTorsoEquipment;
    public HipEquipment currentHipEquipment;
    public HandEquipment currentHandEquipment;
    public FeetEquipment currentFeetEquipment;
        
    // create arrays of size 2
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[2];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[2];

    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = 0;

    private void Awake()
    {

        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
    }

    private void Start()
    {
        characterWeaponSlotManager.LoadBothWeaponsOnSlots();    }
    }
}

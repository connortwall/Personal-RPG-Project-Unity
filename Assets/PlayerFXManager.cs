using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class PlayerFXManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private WeaponSlotManager weaponSlotManager;
    
    public GameObject currentParticleFX;
    public int amountToBeHealed;
    public GameObject instantiatedFXModel;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HealPlayerFromEffect()
    {
        playerStats.HealPlayer(amountToBeHealed);
        // instantiate particles at feet of player
        GameObject healFX = Instantiate(currentParticleFX, playerStats.transform);
        // TODO: review what destroy does exactly
        Destroy(instantiatedFXModel.gameObject);    
        weaponSlotManager.LoadBothWeaponsOnSlots();
    }
}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class PlayerFXManager : CharacterFXManager
{
    private PlayerStatsManager playerStatsManager;
    private PlayerWeaponSlotManager playerWeaponSlotManager;
    
    private PoisonBuildUpBar poisonBuildUpBar;
    private PoisonAmountBar poisonAmountBar;
    
    [Header("Healing FX")]
    public GameObject currentParticleFX;
    public int amountToBeHealed;
    public GameObject instantiatedFXModel;

    private void Awake()
    {
        base.Awake();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

        poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
        poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
    }

    public void HealPlayerFromEffect()
    {
        playerStatsManager.HealPlayer(amountToBeHealed);
        // instantiate particles at feet of player
        GameObject healFX = Instantiate(currentParticleFX, playerStatsManager.transform);
        // TODO: review what destroy does exactly
        Destroy(instantiatedFXModel.gameObject);    
        playerWeaponSlotManager.LoadBothWeaponsOnSlots();
    }

    protected override void HandlePoisonBuildUp()
    {
        if (poisonBuildUpStatus <= 0)
        {
            poisonBuildUpBar.gameObject.SetActive(false);
        }
        else
        {
            poisonBuildUpBar.gameObject.SetActive(true);
        }
        
        base.HandlePoisonBuildUp();
        poisonBuildUpBar.SetPoisonBuildUp(Mathf.RoundToInt(poisonBuildUpStatus));
    }

    protected override void HandleIsPoisonedEffect()
    {
        if (!isPoisoned)
        {
            poisonAmountBar.gameObject.SetActive(false);
        }
        else
        {
            poisonAmountBar.gameObject.SetActive(true);
        }
        base.HandleIsPoisonedEffect();
        poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonBuildUpLimit));
    }
}
}

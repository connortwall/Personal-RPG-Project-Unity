using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    

public class PlayerEquipmentManager : MonoBehaviour
{
    private InputHandler inputHandler;
    private PlayerInventoryManager playerInventoryManager;
    private PlayerStatsManager playerStatsManager;
    
    [Header("Equipment Model Changers")]
    private HelmetModelChanger helmetModelChanger;
    private TorsoModelChanger torsoModelChanger;
    private HipModelChanger hipModelChanger;
    //private LeftLegModelChanger leftLegModelChanger;
    //private RightLegModelChanger rightLegModelChanger;
    private HandModelChanger handModelChanger;
    private FeetModelChanger feetModelChanger;
    
    [Header("Default Naked Models")] 
    public GameObject nakedHeadModel;
    //public string nakedHeadModel;
    public string nakedTorsoModel;
    public string nakedHipModel;
    //public string nakedLeftLegModel;
    //public string nakedRightLegModel;
    public string nakedHandModel;
    public string nakedFeetModel;
    
    private BlockingCollider blockingCollider;

    
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();
        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
        hipModelChanger = GetComponentInChildren<HipModelChanger>();
        //leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
        //rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
        handModelChanger = GetComponentInChildren<HandModelChanger>();
        feetModelChanger = GetComponentInChildren<FeetModelChanger>();
    }

    private void Start()
    {
        EquipAllEquipmentModelsOnStart();
    }

    private void EquipAllEquipmentModelsOnStart()
    {
        // HELMET EQUIPMENT
        helmetModelChanger.UnequipAllBodyPartModels();
        if (playerInventoryManager.currentHelmetEquipment != null)
        {
            // disable part of the head if clipping with helmet
            //nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipBodyPartModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
            playerStatsManager.physicalDamageAbsorptionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
        }
        else
        {
            // equip head
            nakedHeadModel.SetActive(true);
            playerStatsManager.physicalDamageAbsorptionHead = 0;
        }

        // TORSO EQUIPMENT
        torsoModelChanger.UnequipAllTorsoModels();
        if (playerInventoryManager.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
            playerStatsManager.physicalDamageAbsorptionTorso = playerInventoryManager.currentTorsoEquipment.physicalDefense;
            Debug.Log("Torso damage absorption is " + playerStatsManager.physicalDamageAbsorptionTorso);

        }
        else
        {
            //equip torso naked
            torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
            playerStatsManager.physicalDamageAbsorptionTorso = 0;
        }

        // hip and legs
        hipModelChanger.UnequipAllBodyPartModels();
        //leftLegModelChanger.UnequipAllLegModels();
        //rightLegModelChanger.UnequipAllLegModels();
        if (playerInventoryManager.currentHipEquipment != null)
        {
            hipModelChanger.EquipBodyPartModelByName(playerInventoryManager.currentHipEquipment.hipModelName);
            //leftLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.leftLegName);
            //rightLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.rightLegName);
            playerStatsManager.physicalDamageAbsorptionLegs = playerInventoryManager.currentHipEquipment.physicalDefense;
            Debug.Log("Leg damage absorption is " + playerStatsManager.physicalDamageAbsorptionLegs);

        }
        else
        {
            hipModelChanger.EquipBodyPartModelByName(nakedHipModel);
            //leftLegModelChanger.EquipLegModelByName(nakedLeftLegModel);
            //rightLegModelChanger.EquipLegModelByName(nakedRightLegModel);
            playerStatsManager.physicalDamageAbsorptionLegs = 0;
        }
        
        //HAND EQUIPMENT
        handModelChanger.UnequipAllBodyPartModels();
        if (playerInventoryManager.currentHandEquipment != null)
        {
            handModelChanger.EquipBodyPartModelByName(playerInventoryManager.currentHandEquipment.handModelName);
            playerStatsManager.physicalDamageAbsorptionHands = playerInventoryManager.currentHandEquipment.physicalDefense;
            Debug.Log("Hand damage absorption is " + playerStatsManager.physicalDamageAbsorptionHands);

        }
        else
        {
            //equip torso naked
            handModelChanger.EquipBodyPartModelByName(nakedHandModel);
            playerStatsManager.physicalDamageAbsorptionHands = 0;
        }
        
        //FEET EQUIPMENT
        feetModelChanger.UnequipAllBodyPartModels();
        if (playerInventoryManager.currentFeetEquipment != null)
        {
            feetModelChanger.EquipBodyPartModelByName(playerInventoryManager.currentFeetEquipment.feetModelName);
            playerStatsManager.physicalDamageAbsorptionFeet = playerInventoryManager.currentFeetEquipment.physicalDefense;
            Debug.Log("Feet damage absorption is " + playerStatsManager.physicalDamageAbsorptionFeet);
        }
        else
        {
            //equip torso naked
            feetModelChanger.EquipBodyPartModelByName(nakedFeetModel);
            playerStatsManager.physicalDamageAbsorptionFeet = 0;
        }
    }

    public void EnableBlockingCollider()
    {
        // if two hganding
        if (inputHandler.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.rightWeapon);

        }
        else
        {
            // normally block with left weapon
            blockingCollider.SetColliderDamageAbsorption(playerInventoryManager.leftWeapon);

        }
        
      blockingCollider.EnableBlockingCollider();
    }
    
    public void DisableBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
}

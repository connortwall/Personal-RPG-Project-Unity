using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class SpellItem : Item
{
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;
    public string spellAnimation;

    [Header("Spell Cost")] 
    public int magicCost;
    
    [Header("Spell Type")] 
    public bool isFaithSpell;
    public bool isMagicSpell;
    public bool isPyroSpell;

    [Header("Spell Description")] [TextArea]
    public string spellDescription;

    public virtual void AttemptToCastSpell(
        PlayerAnimatorManager playerAnimatorManager, 
        PlayerStatsManager playerStatsManager, 
        PlayerWeaponSlotManager playerWeaponSlotManager)
    {
        Debug.Log("You attempted to cast a spell");
    }

    public virtual void SuccessfullyCastSpell(
        PlayerAnimatorManager playerAnimatorManager, 
        PlayerStatsManager playerStatsManager,
        CameraHandler cameraHandler,
        PlayerWeaponSlotManager playerWeaponSlotManager)
    {
        Debug.Log("You successfully cast a spell");
        // only deduct magic cost is successfully cast spell
        playerStatsManager.DeductMagic(magicCost);
    }
    
}
}

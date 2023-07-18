using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class CharacterFXManager : MonoBehaviour
{
    private CharacterStatsManager characterStatsManager;
    [Header("Damage FX")] 
    public GameObject bloodSplatterFX;
    
    [Header("Weapon FX")]
    public WeaponFX rightWeaponFX;
    public WeaponFX leftWeaponFX;

    [Header("Poison")] 
    public GameObject defaultPoisonParticleFX;
    public GameObject currentPoisonParticleFX;
    public Transform buildUpTransform; // location build up particle FX will spawn
    public bool isPoisoned; 
    public float poisonBuildUpStatus = 0; // the build up over time that poisons the player after reaching 100
    public float poisonBuildUpLimit = 100; // the amount of poison the player has to process before becoming unpoisen
    public float defaultPoisonBuildUpLimit = 100; // the amount fo poison a player has to process once they ecome posionsened
    public float poisonTimer = 2; // Amount of time between each poison damage tick;
    public int poisonDamage = 10;
    private float timer;
    
    protected virtual void Awake()
    {
        characterStatsManager = GetComponent<CharacterStatsManager>(); // call it in base bc need to use it to deal damage
    }
    
    public virtual void PlayWeaponFX(bool isLeft)
    {
        if (isLeft == false)
        {
            if (rightWeaponFX != null)
            {
                rightWeaponFX.PlayWeaponFX();
            }
        }
        else
        {
            if (leftWeaponFX != null)
            {
                leftWeaponFX.PlayWeaponFX();
            }
        }
    }

    public virtual void HandleAllBuildUpEffects()
    {
        if (characterStatsManager.isDead)
        {
            return;
        }
        HandlePoisonBuildUp();
        HandleIsPoisonedEffect();
    }
    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
    {
        GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
    }

    protected virtual void HandlePoisonBuildUp()
    {
        if (isPoisoned)
        {
            return;
        }

        if (poisonBuildUpStatus > 0 && poisonBuildUpStatus < 100)
        {
            poisonBuildUpStatus = poisonBuildUpStatus - 1 * Time.deltaTime; //  1 point  of poison build up over time
        }
        else if (poisonBuildUpStatus >= 100)
        {
            isPoisoned = true;
            poisonBuildUpStatus = 0; // reset buildup

            if (buildUpTransform != null)
            {
                currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, buildUpTransform.transform);
            }
            else
            {
                currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, characterStatsManager.transform);
            }
        }
    }

    protected virtual void HandleIsPoisonedEffect()
    {
        if (isPoisoned)
        {
            if (poisonBuildUpLimit > 0)
            {
                timer += Time.deltaTime;
                // if timer is going, damage player, reset timer
                if (timer >= poisonTimer)
                {
                    characterStatsManager.TakePoisonDamage(poisonDamage);
                    timer = 0;
                }
                poisonBuildUpLimit = poisonBuildUpLimit - 1 * Time.deltaTime;
            }
            else
            {
                isPoisoned = false;
                poisonBuildUpLimit = defaultPoisonBuildUpLimit;
                Destroy(currentPoisonParticleFX);
            }
        }
    }
}
}
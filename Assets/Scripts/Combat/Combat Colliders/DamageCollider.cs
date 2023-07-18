using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{

    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
    protected Collider damageCollider;
    public bool enabledDamageColliderOnStartUp = false;

    [Header("Team I.D.")] 
    public int teamIDNumber = 0;
    
    [Header("Poise")] 
    public float poiseBreak;
    public float offensivePoiseBonus;
    
    [Header("Damage")]
    public int physicalDamage;
    public int fireDamage;
    public int magicDamage;
    public int lightningDamage;
    
    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = enabledDamageColliderOnStartUp;
        
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }
    
    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            CharacterStatsManager opponentStatsManager = other.GetComponent<CharacterStatsManager>();
            CharacterManager opponentCharacterManager = other.GetComponent<CharacterManager>();
            CharacterFXManager opponentFXManager = other.GetComponent<CharacterFXManager>();
            BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();
            
            // parrying happens first
            if (opponentCharacterManager != null)
            {
                //if (opponentStatsManager.teamIDNumber == teamIDNumber)
                 //   return;
                

                if (opponentCharacterManager.isParrying)
                {
                    //check if player is parryable
                    characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("parried",true);
                    return;
                }

                else if (shield != null && opponentCharacterManager.isBlocking)
                {
                    //sheild will block perentage od total damage depending on how much the sheild abrobs
                    float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                    float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;
                    
                    if (opponentStatsManager != null)
                    {
                        opponentStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), fireDamage,true, "block attack");
                        return;
                    }
                }
            }

            if (opponentStatsManager != null)
            {
                if (opponentStatsManager.teamIDNumber == teamIDNumber)
                {
                    return;
                }
                opponentStatsManager.poiseResetTimer = opponentStatsManager.totalPoiseResetTime;
                opponentStatsManager.totalPoiseDefense = opponentStatsManager.totalPoiseDefense - poiseBreak;
                Debug.Log("Enemy's Poise is currently " + opponentStatsManager.totalPoiseDefense);

                // detects where collider first makes contact
                // whichever area strikes collider first, save location of contact
                Vector3 contactPoint =
                    other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                opponentFXManager.PlayBloodSplatterFX(contactPoint);
                
                if (opponentStatsManager.totalPoiseDefense > poiseBreak)
                {
                    opponentStatsManager.TakeDamageNoAnimation(physicalDamage, fireDamage);
                    Debug.Log("Player's Poise is currently " + opponentStatsManager.totalPoiseDefense);
                }
                else
                {
                    opponentStatsManager.TakeDamage(physicalDamage, fireDamage, true);
                }
            }
        }
        
        if (other.tag == "Illusionary Wall")
        {
            IllusionaryDoor illusionaryDoor = other.GetComponent<IllusionaryDoor>();
            illusionaryDoor.doorHasBeenHit = true;
        }
    }
}
}
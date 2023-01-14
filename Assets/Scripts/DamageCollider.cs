using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{

    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
    private Collider damageCollider;
    public int currentWeaponDamage = 25;
    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;

        // cant be done herre, needs to be initialized in weapnslo mamnger with colliders
        //characterManager = GetComponentInParent<CharacterManager>();

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
        if (other.tag == "Player")
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            CharacterManager enemyCharacterManager = other.GetComponent<CharacterManager>();

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.isParrying)
                {
                    //check if player is parryable
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("parried",true);
                    return;
                }
            }
            if (playerStats != null)
            {
                playerStats.TakeDamage(currentWeaponDamage, true);
            }
        }
        
        if (other.tag == "Enemy")
        {
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();
            CharacterManager enemyCharacterManager = other.GetComponent<CharacterManager>();

            if (enemyCharacterManager != null)
            {
                if (enemyCharacterManager.isParrying)
                {
                    //check if player is parryable
                    characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("parried",true);
                    return;
                }
            }
            
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeaponDamage, true);
            }
        }
    }
}
}
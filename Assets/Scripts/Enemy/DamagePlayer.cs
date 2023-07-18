using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    // TODO: old script do not need 
    public class DamagePlayer : MonoBehaviour
    {
        private int damage = 25;

        private void OnTriggerEnter(Collider other)
        {
            PlayerStatsManager playerStatsManager = other.GetComponent<PlayerStatsManager>();

            if (playerStatsManager != null)
            {
                playerStatsManager.TakeDamage(damage, 0,true);
            }
        }
    }
}
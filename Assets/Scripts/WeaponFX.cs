using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW {   
public class WeaponFX : MonoBehaviour
{
   [Header("Weapon FX")] 
   public ParticleSystem standardWeaponTrail;
   // other categories of weapon trails

   public void PlayWeaponFX()
   {
       standardWeaponTrail.Stop();
       if (standardWeaponTrail.isStopped)
       {
           standardWeaponTrail.Play();
       }
   }
}
}

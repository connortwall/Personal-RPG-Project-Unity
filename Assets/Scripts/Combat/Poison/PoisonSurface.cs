using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
public class PoisonSurface : MonoBehaviour
{
   public float poisonBuildUpAmount = 7;
   public List<CharacterFXManager> charactersInsidePoisonSurface;

   // add character from effect list
   private void OnTriggerEnter(Collider other)
   {
      CharacterFXManager character = other.GetComponent<CharacterFXManager>();

      if (character != null)
      {
         charactersInsidePoisonSurface.Add(character);
      }
   }

   // remove character from effect list
   private void OnTriggerExit(Collider other)
   {
      CharacterFXManager character = other.GetComponent<CharacterFXManager>();
      
      if (character != null)
      {
         charactersInsidePoisonSurface.Remove(character);
      }
   }

   // continue effect on charcters on list
   private void OnTriggerStay(Collider other)
   {
      foreach (CharacterFXManager character in charactersInsidePoisonSurface)
      {
         character.poisonBuildUpStatus = character.poisonBuildUpStatus + poisonBuildUpAmount * Time.deltaTime;
      }
   }
}
}

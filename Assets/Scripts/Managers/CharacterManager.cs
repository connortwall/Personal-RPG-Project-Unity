using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
   
public class CharacterManager : MonoBehaviour
{
   [Header("Lock On Transform")]
   public Transform lockOnTransform;
   
   [Header("Combat Collider")]
   public BoxCollider backstabBoxCollider;
   public BackstabCollider backstabCollider;
   
   // damage will be inflicted during an animation event
   // used in 
   public int pendingCriticalDamage;
}
}

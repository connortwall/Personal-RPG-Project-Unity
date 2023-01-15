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
   public CriticalDamageCollider backstabCollider;
   public CriticalDamageCollider riposteCollider;

   [Header("Combat Flags")] 
   public bool canBeRiposted;
   public bool canBeParried;
   public bool isParrying;
   public bool isBlocking;
   
   // damage will be inflicted during an animation event
   // used in backstab or riposte animations
   public int pendingCriticalDamage;
}
}

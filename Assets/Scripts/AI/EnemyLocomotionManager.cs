using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace CW
{

 public class EnemyLocomotionManager : MonoBehaviour
 {
  private EnemyManager enemyManager;
  private EnemyAnimationManager enemyAnimationManager;

  public CapsuleCollider characterCollider;
  public CapsuleCollider characterCollisionBlockerCollider;
  
  private void Awake()
  {
   enemyManager = GetComponent<EnemyManager>();
   enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
  }

  private void Start()
  {
   // ignore collision between chracter adn character collider
   Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
  }
 }
}
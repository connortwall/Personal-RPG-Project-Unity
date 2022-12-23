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
  
  private void Awake()
  {
   enemyManager = GetComponent<EnemyManager>();
   enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
  }
 }
}
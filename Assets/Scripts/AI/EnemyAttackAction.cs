using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CW
{
    
    [CreateAssetMenu(menuName = "A.I./Enemy Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public bool canCombo;
    public EnemyAttackAction comboAction;
    public int attackScore = 3;
    // time between attacks
    public float recoveryTime = 3;
    public float maximumAttackAngle = 35;
    public float minimumAttackAngle = -35;

    public float minimumDistanceNeededToAttack = 0;
    public float maximumDistanceNeededToAttack = 3;

}
}

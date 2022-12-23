using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace CW
{
    
public class EnemyManager : CharacterManager
{
    private EnemyLocomotionManager enemyLocomotionManager;
    private EnemyAnimationManager enemyAnimationManager;
    private EnemyStats enemyStats;

    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;

    public State currentState;
    public CharacterStats currentTarget;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;

    public bool isPerformingAction;
    public bool isInteracting;
    public float rotationSpeed = 30;
    public float maximumAttackRange = 1.5f;
    
    [Header("A.I. Settings")]
    //circle radius of detection by enemy
    public float detectionRadius = 20;
    // the higher the max detection angle, expands the field of view
    public float minimumDetectionAngle = -50;
    public float maximumDetectionAngle = 50;

    // time before another attack
    public float currentRecoveryTime = 0;
    
    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidbody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();
        isInteracting = enemyAnimationManager.anim.GetBool("isInteracting");
    }

    // rigid body moves better on fixed update than update
    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimationManager);
            if (nextState != null)
            {
                SwitchToNextState(nextState);
            } 
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if (currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }
}
}

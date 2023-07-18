using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace CW
{
    
public class EnemyManager : CharacterManager
{
    private EnemyLocomotionManager enemyLocomotionManager;
    private EnemyAnimationManager enemyAnimationManager;
    private EnemyStatsManager enemyStatsManager;
    private EnemyFXManager enemyFXManager;
    
    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction currentAttack;

    public State currentState;
    public CharacterStatsManager currentTarget;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;

    public bool isPerformingAction;
    public float rotationSpeed = 30;
    public float maximumAggroRadius = 1.5f;

    [Header("A.I. Settings")]
    //circle radius of detection by enemy
    public float detectionRadius = 2;
    // the higher the max detection angle, expands the field of view
    public float minimumDetectionAngle = -50;
    public float maximumDetectionAngle = 50;
    // time before another attack
    public float currentRecoveryTime = 0;
    [Header("A.I. Combat Settings")] 
    public bool allowAIToPerformCombos;

    public bool isPhaseShifting;
    public float comboLikelihood;
    
    protected override void Awake()
    {
        base.Awake();
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimationManager = GetComponent<EnemyAnimationManager>();
        enemyStatsManager = GetComponent<EnemyStatsManager>();
        enemyFXManager = GetComponent<EnemyFXManager>();
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
        HandleStateMachine();

        isUsingLeftHand = enemyAnimationManager.animator.GetBool("isUsingLeftHand");
        isUsingRightHand = enemyAnimationManager.animator.GetBool("isUsingRightHand");
        isRotatingWithRootMotion = enemyAnimationManager.animator.GetBool("isRotatingWithRootMotion");
        isInteracting = enemyAnimationManager.animator.GetBool("isInteracting");
        isPhaseShifting = enemyAnimationManager.animator.GetBool("isPhaseShifting");
        isInvulnerable = enemyAnimationManager.animator.GetBool("isInvulnerable");
        // when animation event changes to true, change this bool to true
        canDoCombo = enemyAnimationManager.animator.GetBool("canDoCombo");
        canRotate = enemyAnimationManager.animator.GetBool("canRotate");
        // update animator is dead bool (in associated character managers)
        enemyAnimationManager.animator.SetBool("isDead", enemyStatsManager.isDead);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        enemyFXManager.HandleAllBuildUpEffects();
    }

    // rigid body moves better on fixed update than update
    private void LateUpdate()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
        // TODO fix FX: enemyFXManager.HandleAllBuildUpEffects();
    }

    private void HandleStateMachine()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimationManager);
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

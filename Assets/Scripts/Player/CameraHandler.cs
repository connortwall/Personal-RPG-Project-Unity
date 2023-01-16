using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace CW
{
    public class CameraHandler : MonoBehaviour
    {
        private InputHandler inputHandler;
        private PlayerManager playerManager;

        public static CameraHandler singleton;
        
        public Transform targetTransform;
        // transform of actual camera
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        public LayerMask ignoreLayers;
        public LayerMask environmentLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        // how the camera rotates
        public float lookSpeed = 100f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 300f;
        public float targetPosition;
        public float defaultPosition;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.65f;

        public CharacterManager currentLockOnTarget;
        
        private List<CharacterManager> availableTargets = new List<CharacterManager>();
        // thee are character manager variables to allow for referencing more info
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;
        public float maximumLockOnDistance = 30;

        // vector position of camera transform
        private Vector3 cameraTransformPosition;
        private float lookAngle;

        // how the camera rotates, rotates aroud pivot
        private float pivotAngle;

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            //ignoreLayers = ~((1 << 8) | (1 << 9) | (1 << 10));
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            environmentLayer = LayerMask.NameToLayer("Environment");
        }

        public void FollowTarget(float delta)
        {
            // smoother camera that follows
            
            //var targetPosition =
                //Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity,
                    //delta / followSpeed);

            // not so smooth camera that follows player
            // Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
            //myTransform.position = targetPosition;
            myTransform.position = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            // not locked on
            if (inputHandler.lockOnFlag == false && currentLockOnTarget == null)
            {
                lookAngle += mouseXInput * lookSpeed * delta;
                pivotAngle -= mouseYInput * pivotSpeed * delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                var rotation = Vector3.zero;
                rotation.y = lookAngle;
                var targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            // we are locked on
            else
            {
                float velocity = 0;

                Vector3 dir = currentLockOnTarget.transform.position - transform.position;
                dir.Normalize();
                dir.y = 0;
                
                // rotate in the direction created above
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
                dir.Normalize();
                
                // forcing camera to rotation direction that is locked on to
                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraTransform.localEulerAngles = eulerAngle;

            }
        }

        private void HandleCameraCollision(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            var direction = cameraTransform.position = cameraPivotTransform.position;
            direction.Normalize();

            // Casts a sphere along a ray and returns detailed information on what was hit.
            // 
            if (Physics.SphereCast
                (cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition),
                    ignoreLayers))
            {
                // distance between pivot transform and the hitpoint of spherecast
                var dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                // target position is a combination of distance - offset  
                targetPosition = -(dis - cameraCollisionOffset);
            }

            // if the abs value is less than the minimum collision offset, change the target position
            if (Mathf.Abs(targetPosition) < minimumCollisionOffset) targetPosition = -minimumCollisionOffset;

            // lerp the camera position
            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            availableTargets.Clear();
            
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;
            
            // put invisible sphere around player position 26 units in diameter
            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager characterManager = colliders[i].GetComponent<CharacterManager>();
                // if character manager found
                if (characterManager != null)
                {
                    Vector3 lockTargetDirection = characterManager.transform.position - targetTransform.position;
                    float distanceFromTarget =
                        Vector3.Distance(targetTransform.position, characterManager.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                    RaycastHit hit;
                    
                    // dont lock onto self
                    if (characterManager.transform.root != targetTransform.root
                        && viewableAngle > -50 && viewableAngle < 50
                        && distanceFromTarget <= maximumLockOnDistance)
                    {
                        // shoot a line between two point in game (between character and target)
                        if (Physics.Linecast(playerManager.lockOnTransform.position, characterManager.lockOnTransform.position,
                                out hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, characterManager.lockOnTransform.position);
                            // what is this?
                            if (hit.transform.gameObject.layer == environmentLayer)
                            {
                                // Cannot Lock on to target bc object in the way
                                inputHandler.lockOnFlag = false;
                                print("Cannot lock on targets");

                            }
                            // if nothing in the way, lock on
                            else
                            {
                                availableTargets.Add(characterManager);
                            }
                        }
                        
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget =
                    Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);
                // find closest target
                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                if (inputHandler.lockOnFlag && currentLockOnTarget)
                {
                    // read more on Unity documentaion
                    //Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    //var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                    //var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;

                    // caluculate position relative to player
                    Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    // target to left cannot be targeted (dont want sam target to occupy two placex)
                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget 
                                                        && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[k];
                    }
                    
                    // dont want an enemy occupying two targets in list
                    else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget
                        && availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[k];
                    }

                }
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            leftLockTarget = null;
            rightLockTarget = null;
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);
            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition,
                    newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(
                    cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }

        }
    }
}
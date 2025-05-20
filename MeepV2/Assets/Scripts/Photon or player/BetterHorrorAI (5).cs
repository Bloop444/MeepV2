using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class BetterHorrorAI : MonoBehaviour
{
    [Header("Made By P1_vr")]

    [Header("Monster Settings")]
    public float WanderSpeed = 5f;
    public float ChasingSpeed = 7.5f;
    public NavMeshAgent agent;

    [Header("Tag || The Tag That The Monster Will Chase")]
    public string playerTag = "Body";

    [Header("Raycast Settings")]
    public float viewDistance = 20f;
    public float fovAngle = 60f;
    public int rayCount = 10;

    [Header("Raycast Offset Settings")]
    public Vector3 rayPositionOffset = Vector3.zero;
    public Vector3 rayRotationOffset = Vector3.zero;

    [Header("Chase Duration || For How Long Will The Monster Still Chase Player If Out Of Sight")]
    public float chaseDuration = 5f;

    [Header("Raycast Colors Cuz Why Not")]
    public Color whenHitColor = Color.green;
    public Color whenNotHitColor = Color.red;

    [Header("Debugging")]
    public Transform currentTarget;
    public float chaseTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = WanderSpeed;
        Wander();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            agent.enabled = true;

            if (DetectTarget())
            {
                agent.speed = ChasingSpeed;
                chaseTimer = chaseDuration;
            }
            else if (currentTarget != null)
            {
                chaseTimer -= Time.deltaTime;
                if (chaseTimer > 0)
                {
                    Chase(currentTarget);
                }
                else
                {
                    currentTarget = null;
                    agent.speed = WanderSpeed;
                    Wander();
                }
            }
            else if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                agent.speed = WanderSpeed;
                Wander();
            }
        }
        else
        {
            agent.enabled = false;
        }
    }

    bool DetectTarget()
    {
        bool targetDetected = false;

        Vector3 rayOrigin = transform.position + transform.TransformVector(rayPositionOffset);
        Quaternion rayRotation = transform.rotation * Quaternion.Euler(rayRotationOffset);

        for (int i = 0; i < rayCount; i++)
        {
            float step = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-fovAngle / 2, fovAngle / 2, step);
            Vector3 rayDirection = rayRotation * Quaternion.Euler(0, angle, 0) * Vector3.forward;

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, viewDistance, ~0, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.CompareTag(playerTag))
                {
                    currentTarget = hit.collider.transform;
                    Chase(currentTarget);
                    targetDetected = true;
                    break;
                }
            }
        }

        return targetDetected;
    }

    void Chase(Transform target)
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void Wander()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
        {
            agent.destination = hit.position;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 rayOrigin = transform.position + transform.TransformVector(rayPositionOffset);
        Quaternion rayRotation = transform.rotation * Quaternion.Euler(rayRotationOffset);

        Gizmos.color = Color.blue;
        Vector3 leftBoundary = rayRotation * Quaternion.Euler(0, -fovAngle / 2, 0) * Vector3.forward * viewDistance;
        Vector3 rightBoundary = rayRotation * Quaternion.Euler(0, fovAngle / 2, 0) * Vector3.forward * viewDistance;

        Gizmos.DrawRay(rayOrigin, leftBoundary);
        Gizmos.DrawRay(rayOrigin, rightBoundary);

        if (rayCount > 1)
        {
            for (int i = 0; i < rayCount; i++)
            {
                float step = (float)i / (rayCount - 1);
                float angle = Mathf.Lerp(-fovAngle / 2, fovAngle / 2, step);
                Vector3 direction = rayRotation * Quaternion.Euler(0, angle, 0) * Vector3.forward;

                RaycastHit hit;
                bool hitPlayer = Physics.Raycast(rayOrigin, direction, out hit, viewDistance, ~0, QueryTriggerInteraction.Collide) && hit.collider.CompareTag(playerTag);

                Gizmos.color = hitPlayer ? whenHitColor : whenNotHitColor;

                Gizmos.DrawRay(rayOrigin, direction * viewDistance);
            }
        }
    }
}

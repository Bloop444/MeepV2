using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class KYNEXXWEEPINGANGEL : MonoBehaviour
{
    [Header("MADE BY TRAXZZ MOTIFIED BY KYNEXX DO NOT STEAL THIS TOOK FOREVER")]

    [Header("MAIN STUFFS")]
    public string ChaseTag;
    public Transform[] points;
    public LayerMask obstacleLayer;
    public NavMeshAgent agent;
    public Animator anim;
    public PhotonView photonView;
    public AudioSource audioSource;
    public AudioClip ChaseSound;

    [Header("OTHER SETTINGS")]
    public float DetectLength = 5f, DetectAngle = 45f, DetectHeight = 2f;
    public float WalkSpeed = 3.5f, ChaseSpeed = 5f;
    public float ViewAngleThreshold = 60f;
    public float DefaultSpeed = 2.5f, AnimSpeedMultiplier = 10f;

    int destPoint = 0;
    bool isChasing = false;
    Vector3 lastPos;

    void Start()
    {
        agent.autoBraking = false;
        agent.speed = WalkSpeed;
        audioSource.clip = ChaseSound;
        audioSource.loop = true;
        GotoNextPoint();
        lastPos = transform.position;
    }

    void Update()
    {
        if (!photonView.IsMine) { agent.enabled = false; return; }
        agent.enabled = true;

        GameObject target = FindTarget();
        if (target)
        {
            if (!IsPlayerLooking(target))
            {
                Chase(target.transform.position);
            }
            else
            {
                StopChase();
            }
        }
        else
        {
            Patrol();
        }

        Animate();
    }

    GameObject FindTarget()
    {

        GameObject[] targets = GameObject.FindGameObjectsWithTag(ChaseTag);
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (var t in targets)
        {
            float dist = Vector3.Distance(transform.position, t.transform.position);
            if (dist > DetectLength) continue;

            Vector3 dir = (t.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dir) > DetectAngle) continue;

            if (!Physics.Raycast(transform.position, dir, dist, obstacleLayer))
            {
                if (dist < minDist)
                {
                    closest = t;
                    minDist = dist;
                }
            }
        }
        return closest;
    }

    bool IsPlayerLooking(GameObject player)
    {
        Vector3 toAI = (transform.position - player.transform.position).normalized;
        float dot = Vector3.Dot(player.transform.forward, toAI);
        return dot > Mathf.Cos(ViewAngleThreshold * Mathf.Deg2Rad);
    }

    void Chase(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
        agent.speed = ChaseSpeed;
        if (!isChasing)
        {
            isChasing = true;
            audioSource.Play();
        }
    }

    void StopChase()
    {
        agent.speed = 0f;
    }

    void Patrol()
    {
        if (isChasing)
        {
            isChasing = false;
            audioSource.Stop();
            agent.speed = WalkSpeed;
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }

    void GotoNextPoint()
    {
        if (points.Length == 0) return;
        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    void Animate()
    {
        float velocity = (transform.position - lastPos).magnitude / Time.deltaTime;
        float speedMultiplier = velocity / DefaultSpeed;
        anim.speed = speedMultiplier * AnimSpeedMultiplier;
        lastPos = transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 forward = transform.forward * DetectLength;
        Vector3 leftBoundary = Quaternion.Euler(0, -DetectAngle, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, DetectAngle, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + forward);

        Vector3 topCenter = transform.position + new Vector3(0, DetectHeight / 2, 0);
        Vector3 bottomCenter = transform.position - new Vector3(0, DetectHeight / 2, 0);

        Gizmos.DrawLine(topCenter, transform.position + leftBoundary);
        Gizmos.DrawLine(topCenter, transform.position + rightBoundary);
        Gizmos.DrawLine(bottomCenter, transform.position + leftBoundary);
        Gizmos.DrawLine(bottomCenter, transform.position + rightBoundary);
    }
}

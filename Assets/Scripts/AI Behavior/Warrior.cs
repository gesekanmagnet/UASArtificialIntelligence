using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float sightDistance = 10f; // Jarak pandangan mata asli
    [Range(50, 100)] public float sightAngle = 60f;
    [Range(5, 10)] public float rangeDestination;

    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    public Transform availableArea;

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = patrolSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        DrawFieldOfView();
    }

    private void DrawFieldOfView()
    {
        Vector3 fovLine1 = Quaternion.Euler(0, -sightAngle / 2, 0) * transform.forward * sightDistance;
        Vector3 fovLine2 = Quaternion.Euler(0, sightAngle / 2, 0) * transform.forward * sightDistance;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);

        Gizmos.DrawRay(transform.position + fovLine1, Quaternion.Euler(0, sightAngle / 2, 0) * transform.forward * (sightDistance - 0.5f));
        Gizmos.DrawRay(transform.position + fovLine2, Quaternion.Euler(0, -sightAngle / 2, 0) * transform.forward * (sightDistance - 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void Moving()
    {
        animator.SetFloat("Speed", agent.speed);
        Vector3 directionToTarget = player.transform.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        if (angleToTarget <= sightAngle && directionToTarget.magnitude <= sightDistance)
        {
            Chase(player.transform);
        }
        else
        {
            Chase(null);
        }
    }

    void Chase(Transform target)
    {
        if (target != null)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.position);
        }
        else
        {
            agent.speed = patrolSpeed;
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(availableArea.position, rangeDestination, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                }
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            int i = Random.Range(1, 3);
            agent.isStopped = true;
            if(i == 1) animator.SetBool("isAttack 0", true);
            else if(i == 2) animator.SetBool("isDodge 0", true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player"))
        {
            agent.isStopped = false;
            animator.SetBool("isAttack 0", false);
            animator.SetBool("isDodge 0", false);
        }
    }
}

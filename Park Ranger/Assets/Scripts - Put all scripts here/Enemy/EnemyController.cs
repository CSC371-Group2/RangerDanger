using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public List<Transform> waypoints;
    public GameObject target;

    public float detectionDistance;
    private int destIndex = 0;
    private NavMeshAgent agent;
    private bool patrolling = true;
    private IEnumerator navigate;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.pathPending)
            return;

        if (patrolling)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                navigate = GoToNextPoint();
                StartCoroutine(navigate);
            }
        }

        if (DetectTarget())
        {
            agent.SetDestination(target.transform.position);
            patrolling = false;
        }
        else
        {
            if (!patrolling)
            {
                navigate = GoToNextPoint();
                StartCoroutine(navigate);
            }
        }
    }

    IEnumerator GoToNextPoint()
    {
        if (waypoints.Count == 0)
        {
            yield break;
        }

        destIndex = (destIndex + 1) % waypoints.Count;
        //Debug.Log($"{destIndex}");
        agent.destination = waypoints[destIndex].position;
        agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
    }

    bool DetectTarget()
    {
        if (target != null)
        {
            bool canSee = false;
            Ray ray = new Ray(transform.position, target.transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                canSee = (hit.transform == target) &&
                    (Vector3.Distance(transform.position, target.transform.position) <= detectionDistance);
            }

            return canSee;
        }
        else
        {
            return false;
        }
    }
}

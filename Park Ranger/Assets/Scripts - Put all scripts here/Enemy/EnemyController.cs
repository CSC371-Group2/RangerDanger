using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public List<Transform> waypoints;
    public GameObject player;
    public Transform eye;
    public float detectionDistance;
    public float damage;
    private Animator animator;

    private int destIndex = 0;
    public float runSpeed;
    public float walkSpeed;
    private NavMeshAgent agent;
    private bool patrolling = true;
    private bool attacking = false;
    private IEnumerator navigate;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking)
        {
            if (agent.pathPending)
            {
                return;
            }

            if (patrolling)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    navigate = GoToNextPoint();
                    StartCoroutine(GoToNextPoint());
                }
            }

            if (DetectPlayer())
            {
                agent.SetDestination(player.transform.position);
                agent.speed = runSpeed;
                patrolling = false;
            }
            else
            {
                if (!patrolling)
                {
                    navigate = GoToNextPoint();
                    StartCoroutine(navigate);
                    agent.speed = walkSpeed;
                    patrolling = true;
                }
            }
        }
        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    IEnumerator GoToNextPoint()
    {
        if (waypoints.Count == 0)
        {
            yield break;
        }

        destIndex = (destIndex + 1) % waypoints.Count;
        agent.SetDestination(waypoints[destIndex].position);
        agent.isStopped = true;
        yield return new WaitForSeconds(2f);
        agent.isStopped = false;
    }

    IEnumerator AttackPlayer()
    {
        attacking = true;
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(2f);

        attacking = false;
        player.GetComponent<PlayerController>().Disenagage();
    }

    protected virtual bool DetectPlayer()
    {
        if (player != null)
        {
            bool canSee = false;
            Ray ray = new Ray(eye.position, player.transform.position - eye.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                canSee = (hit.transform == player.transform) &&
                    (Vector3.Distance(transform.position, player.transform.position) <= detectionDistance);
            }

            return canSee;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !attacking)
        {
            player.GetComponent<PlayerController>().TakeDamage(damage);
            StartCoroutine(AttackPlayer());
        }
    }
}

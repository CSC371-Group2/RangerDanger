using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public List<Transform> waypoints;
    protected GameObject player;
    public Transform eye;
    public float detectionDistance;
    public float damage;
    private Animator animator;
    private Light lantern;

    private int destIndex = 0;
    private List<GameObject> encounteredTorches;
    public float runSpeed;
    public float walkSpeed;
    private NavMeshAgent agent;
    private bool patrolling = true;
    private bool attacking = false;
    private bool stunned = false;

    void Start()
    {
        player = GameObject.Find("Ranger D. Danger");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lantern = player.transform.Find("Lantern").GetComponent<Light>();
        encounteredTorches = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attacking && !stunned)
        {
            if (agent.pathPending)
            {
                return;
            }

            if (patrolling)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
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
                    StartCoroutine(GoToNextPoint());
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

    IEnumerator GetStunned()
    {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        animator.SetBool("isStunned", true);
        stunned = true;
        Debug.Log("Stunned");

        yield return new WaitForSeconds(3f);

        agent.isStopped = false;
        animator.SetBool("isStunned", false);
        stunned = false;
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
                    (Vector3.Distance(transform.position, player.transform.position) <= detectionDistance * lantern.intensity);
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
        if (other.tag == "Player" && !attacking && !stunned)
        {
            player.GetComponent<PlayerController>().TakeDamage(damage, gameObject.tag);
            StartCoroutine(AttackPlayer());
        }
    }

    public void Stun(GameObject torch)
    {
        Debug.Log("Idk");
        if (!encounteredTorches.Contains(torch) && !stunned)
        {
            Debug.Log("Does not Contain");
            encounteredTorches.Add(torch);
            StartCoroutine(GetStunned());
        }
        Debug.Log("Contain");
    }
}

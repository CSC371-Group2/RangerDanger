using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform player;
    public float speed;
    public float stoppingDistance;
    public float detectDistance;
    public LayerMask playerLayer;            //Layer on which collision will be checked.


    public bool foundPlayer = false;

    // Update is called once per frame
    void Update()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(detectDistance, 0, 0);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if ((Physics.Linecast(startPos, endPos, playerLayer)) && !foundPlayer)
        {
            foundPlayer = true;
        }
        if (foundPlayer)
        {
            if (distanceToPlayer > stoppingDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public LayerMask playerLayer;            //Layer on which collision will be checked.

    private float detectDistance = GameSettings.detectDist;
    private Transform player;
    private float speed = GameSettings.lerpSpeed; //between 0 and 1;
    private float stoppingDistance = GameSettings.stopDist;
    private bool foundPlayer = false;


    private void Start()
    {
        player = GameObject.Find("Ranger D. Danger").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(detectDistance, 0, 0);
        if ((Physics.Linecast(startPos, endPos, playerLayer)) && !foundPlayer)
        {
            foundPlayer = true;
        }
    }

    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (foundPlayer)
        {
            if (distanceToPlayer > stoppingDistance)
            {
                transform.position = Vector3.Lerp(transform.position, player.position, speed);
            }
        }
    }
}

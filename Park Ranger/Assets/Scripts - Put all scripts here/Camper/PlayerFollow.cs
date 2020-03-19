using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public LayerMask playerLayer;            //Layer on which collision will be checked.

    private float detectDistance = GameSettings.detectDist;
    private Transform player;
    private float speed = GameSettings.lerpSpeed; //between 0 and 1;
    public float turnSpeed = GameSettings.lerpSpeed;
    private float stoppingDistance = GameSettings.stopDist;
    private bool foundPlayer = false;
    private GameObject playerPos;
    private Animator camperAnimator;

    private GameManager gm;


    private void Start()
    {
        gm = GameManager.instance;
        player = GameObject.Find("Ranger D. Danger").GetComponent<Transform>();
        playerPos = GameObject.Find("Ranger D. Danger");
        camperAnimator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(detectDistance, 0, 0);
        // detecting player
        if ((Physics.Linecast(startPos, endPos, playerLayer)) && !foundPlayer)
        {
            gm.is_camper_following = true;
            GameManager.instance.UpdateObjectives(gm.is_camper_following);
            foundPlayer = true;
        }

        // moving camper
        if (foundPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > stoppingDistance)
            {
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerPos.transform.position), turnSpeed);
                transform.LookAt(player, Vector3.up);
                transform.position = Vector3.Lerp(transform.position, player.position, speed);
                camperAnimator.SetBool("isRunning", true);
            }
            else
            {
                camperAnimator.SetBool("isRunning", false);
            }
            
        }
    }

    //private void LateUpdate()
    //{
        
    //    if (foundPlayer)
    //    {
    //        if (distanceToPlayer > stoppingDistance)
    //        {
    //            transform.position = Vector3.Lerp(transform.position, player.position, speed);
    //        }
    //    }
    //}
}

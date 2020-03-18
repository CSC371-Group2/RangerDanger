using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //adjusting camera angle
    public float yMult;
    public float zMult;

    public float movementSpeed = 4.0f;
    public float rotationSpeed = 0.10f;

    private Vector3 movement;
    private Rigidbody rb;
    private Animator animator;
    private GameObject ViewCamera = null;
    private GameManager gameManager;
    private List<Color> matColors;

    private int heartBeatPerMinute = 80;

    private string FINISH_TAG = "Finish";
    private GameManager gm;
    void Start()
    {
        gm = GameManager.instance;

        ViewCamera = GameObject.Find("PlayerCamera");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        StartCoroutine(playHeartBeat());
        movementSpeed = 4.0f;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        updateSpeed();
        if (ViewCamera != null)
        {
            Vector3 direction = (Vector3.up * yMult + Vector3.back * zMult) * 2;
            ViewCamera.transform.position = transform.position + direction;
            ViewCamera.transform.LookAt(transform.position);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
        if (movement != Vector3.zero)
        {
            if (movementSpeed == 5.5)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    void updateSpeed()
    {
        if (gameManager.oilSlider.value < 10)
        {
            movementSpeed = 5.5f;
        }
        else
        {
            movementSpeed = 4.0f;
        }
    }

    void onTriggerEnter(Collider other)
    {
        if(other.tag == FINISH_TAG && gm.check_win_condition())
        {
            gm.escape();
        }
    }

    void controlHeartBeatSFX()
    {

    }

    public void TakeDamage(float damage, string typeOfEnemy)
    {
        gameManager.oilSlider.value -= damage;
        StartCoroutine(increaseHeartBeat(180));

        switch (typeOfEnemy)
        {
            case "Wolf":
                AudioManager.instance.Play("WolfSnarl");
                break;
            case "Snake":
                AudioManager.instance.Play("SnakeStrike");
                break;
            case "Bat":
                AudioManager.instance.Play("BatAttack");
                break;
            default:
                break;
        }
    }

    public void Disenagage()
    {
        StartCoroutine(decreaseHeartBeat(70));
    }

    IEnumerator increaseHeartBeat(int targetHeartBeat)
    {
        while(heartBeatPerMinute < targetHeartBeat)
        {
            heartBeatPerMinute += 10;
            yield return null;
            
        }
    }

    IEnumerator decreaseHeartBeat(int targetHeartBeat)
    {
        while ( heartBeatPerMinute > targetHeartBeat)
        {
            yield return new WaitForSeconds(0.1f);
            heartBeatPerMinute -= 5;
        }
    }

    IEnumerator playHeartBeat()
    {
        while(true)
        {
            yield return new WaitForSeconds(60f / heartBeatPerMinute);
            AudioManager.instance.Play("Heartbeat");
        }
    }
}

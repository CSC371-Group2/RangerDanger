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

    void Start()
    {
        ViewCamera = GameObject.Find("PlayerCamera");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
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
            animator.SetBool("isWalking", true);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), rotationSpeed);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void TakeDamage(float damage)
    {
        gameManager.oilSlider.value -= damage;
    }
}

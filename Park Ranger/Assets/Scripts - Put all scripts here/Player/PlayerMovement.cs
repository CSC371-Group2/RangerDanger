using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject ViewCamera = null;
    public GameObject PointLight = null;
    public float yMult;
    public float zMult;
    public Vector3 lightPos;
    public float moveSpeed = 5f;
    public Rigidbody rb;
    Vector3 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        if (ViewCamera != null)
        {
            Vector3 direction = (Vector3.up * yMult + Vector3.back * zMult) * 2;
            RaycastHit hit;
            Debug.DrawLine(transform.position, transform.position + direction, Color.red);
            if (Physics.Linecast(transform.position, transform.position + direction, out hit))
            {
                ViewCamera.transform.position = hit.point;
            }
            else
            {
                ViewCamera.transform.position = transform.position + direction;
            }
            ViewCamera.transform.LookAt(transform.position);
        }
        if (PointLight != null)
        {
            PointLight.transform.position = transform.position + lightPos;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}

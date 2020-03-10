using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAcross : MonoBehaviour
{
    public GameObject flare;
    private bool cntr_on = false;
    private float cntr = 0f;
    private GameObject instance_flare;


    // Start is called before the first frame update
    void Start()
    {
        spawn_flare();
    }

    // Update is called once per frame
    void Update()
    {
        should_shoot_flare();
        counter_check();     
    }

    private void should_shoot_flare()
    {
        if (GameManager.instance.is_flare_active())
        {
            Debug.Log("toggle flare off from spawner");
            GameManager.instance.end_flare();
            spawn_flare();
        }
    }

    private void counter_check()
    {
        if (cntr_on)
        {
            cntr += Time.deltaTime;
            if (cntr > 6f)
            {
                Object.Destroy(instance_flare);

                reset_countdown();
            }
        }
    }

    private void start_countdown()
    {
        cntr_on = true;
    }

    public void reset_countdown()
    {
        cntr_on = false;
        cntr = 0;
    }

    public void shoot_flare(GameObject g)
    {
        Rigidbody rigid = g.GetComponent<Rigidbody>();
        rigid.AddForce(new Vector3(1, 0, 0) * -1400);
        rigid.AddForce(new Vector3(0, 1, 0) * 5000);

        start_countdown();
    }

    private void spawn_flare()
    {
         instance_flare = Instantiate(flare, gameObject.transform);
         shoot_flare(instance_flare);
         
    }
}

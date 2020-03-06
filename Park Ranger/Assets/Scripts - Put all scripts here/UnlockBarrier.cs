using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBarrier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* child prefab object must have collider for this to work */
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.F)
            && GameManager.instance.can_unlock())
        {
            gameObject.SetActive(false);
        }
    }

}

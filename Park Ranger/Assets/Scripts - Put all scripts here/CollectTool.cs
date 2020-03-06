using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTool : MonoBehaviour
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
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.F)) 
        {
            GameManager.instance.pickupTool();
            GameManager.instance.f_able = false; /* disable F prompt before we diable game object */
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Player"))
        {
            GameManager.instance.f_able = true; 
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.f_able = false; /* disable F prompt when we leave pickup zone */
        }
    }

}

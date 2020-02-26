using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilCollecting : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        bool success = gameManager.incrementOil();

        if(success)
        {
                // disable gameObject
                gameObject.SetActive(false);
                // TODO set respawn time
        }
        else
        {
                // do nothing
        }

    }
}

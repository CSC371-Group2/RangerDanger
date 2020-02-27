using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilCollecting : MonoBehaviour
{
    private GameManager gameManager;
    private double time_passed = 0.0;
    private double oil_respawn_interval = GameSettings.oil_respawn_interval;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; 
    }

    // Update is called once per frame
    void Update()
    {
        // time passed for oil respawn logic
        if(!gameObject.activeSelf)
        {
            time_passed += Time.deltaTime;
        }
        if(!gameObject.activeSelf && time_passed > oil_respawn_interval)
        {
            gameObject.SetActive(true);
            
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        bool success = gameManager.incrementOil();
        if(success)
        {
                // disable gameObject
                gameObject.SetActive(false);
                // TODO set respawn time
                time_passed = 0.0;
        }
        else
        {
                // do nothing
        }

    }
}

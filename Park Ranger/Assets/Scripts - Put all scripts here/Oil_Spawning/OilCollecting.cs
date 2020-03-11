using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilCollecting : MonoBehaviour
{
    private GameManager gameManager;
    private OilSpawner oil_spawner;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance; 
        oil_spawner = transform.parent.gameObject.GetComponent<OilSpawner>();
    }

    // Update is called once per frame
    void Update()
    {}

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bool success = gameManager.incrementOil();
            if(success)
            {
                // disable oil
                disableOil();
                AudioManager.instance.Play("CollectItem");

            }
            else
            {
                // do nothing
            }
        }
    }

    public void enableOil()
    {
        gameObject.SetActive(true);
    }

    private void disableOil()
    {
        //tell parent to start respawn logic
        oil_spawner.start_respawn(enableOil);
        gameObject.SetActive(false);
    }
}

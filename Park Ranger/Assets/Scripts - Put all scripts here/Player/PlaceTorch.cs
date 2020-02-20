using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTorch : MonoBehaviour
{
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");
            gm.PlaceTorch();
        }
    }
}

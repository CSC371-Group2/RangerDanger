using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockWithSupply : MonoBehaviour
{
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.currentSupply() >= gm.supplyNeeded())
        {
            gameObject.SetActive(false);
        }
    }
}

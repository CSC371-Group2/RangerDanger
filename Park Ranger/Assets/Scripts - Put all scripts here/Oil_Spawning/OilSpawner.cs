using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpawner : MonoBehaviour
{
    private Component[] oilLocs;
    public GameObject oil_prefab;

    // Start is called before the first frame update
    void Start()
    {
        oilLocs = GetComponentsInChildren<Transform>();
        foreach (Transform t in oilLocs)
        {
            Instantiate(oil_prefab, t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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
        spawn_all_oil();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawn_all_oil()
    {
        foreach (Transform t in oilLocs)
        {
            if (t != gameObject.transform)
            {
                Instantiate(oil_prefab, t);
            }
        }
    }
}

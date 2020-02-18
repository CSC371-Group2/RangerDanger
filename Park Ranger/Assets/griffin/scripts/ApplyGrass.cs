using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGrass : MonoBehaviour
{
    public Texture mytexture;
    void Start()
    {
        GetComponent<Renderer>().material.mainTexture = mytexture;
    }  
    

    // Update is called once per frame
    void Update()
    {
        
    }
}

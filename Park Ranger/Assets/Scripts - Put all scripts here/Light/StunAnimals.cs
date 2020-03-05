using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAnimals : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().Stun(gameObject);
        }
    }
}

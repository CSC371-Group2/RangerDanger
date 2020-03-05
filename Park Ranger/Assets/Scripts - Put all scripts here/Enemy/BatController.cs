using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : EnemyController
{
    protected override bool DetectPlayer()
    {
        if (!base.DetectPlayer())
        {
            return false;
        }

        Vector3 playerDirection = transform.InverseTransformPoint(player.transform.position).normalized;

        return playerDirection.normalized.z > 0.0f &&
               playerDirection.normalized.x > -0.5f &&
               playerDirection.normalized.x < 0.5f;
    }
}

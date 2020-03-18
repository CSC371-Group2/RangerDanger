using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpawner : MonoBehaviour
{
    private Component[] oilLocs;
    public GameObject oil_prefab;
    private double oil_respawn_interval = GameSettings.oil_respawn_interval;

    public delegate void enableOil();

    private List<double> respawn_timers = new List<double>();
    private Queue<enableOil> spawn_callbacks = new Queue<enableOil>();
    private bool is_respawning = false;

    // Start is called before the first frame update
    void Start()
    {
        oilLocs = GetComponentsInChildren<Transform>();
        spawn_all_oil();
    }

    // Update is called once per frame
    void Update()
    {
        // dat good good respawn logic ;)
        checkShouldRespawn(); // sets is_respawning boolean only...
        updateTimePassed();
        checkNearestRespawnTime();
    }

    private void checkNearestRespawnTime()
    {
        if(respawn_timers.Count > 0)
        {
            if(respawn_timers[0] >= oil_respawn_interval)
            {
                respawn_timers = respawn_timers.GetRange(1, respawn_timers.Count - 1);
                (spawn_callbacks.Dequeue())();
            }
        }
    }

    public void start_respawn(enableOil e)
    {
        respawn_timers.Add(0.0);
        spawn_callbacks.Enqueue(e);
    }

    private void updateTimePassed()
    {
        // time passed for oil respawn logic
        if(is_respawning)
        {
            for(int i = 0; i < respawn_timers.Count; i++)
            {
                respawn_timers[i] += Time.deltaTime;
            }
        }
    }

    private void checkShouldRespawn()
    {
        if(respawn_timers.Count > 0)
        {
            is_respawning = true;
        }
        else
        {
            is_respawning = false;
        }
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

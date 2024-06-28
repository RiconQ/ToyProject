using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhHunPlayerTest : MonoBehaviour
{

    [SerializeField] public RoadSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        spawner.SpawnTriggerEntered();
    }
}

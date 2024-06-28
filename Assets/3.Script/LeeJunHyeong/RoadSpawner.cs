using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    /*
     �ε� �����ʿ� ����ֽ��ϴ�.
     */

    public List<GameObject> roads; // ��� �ε� ����
    private float offset = 100f; 


    private void Start()
    {
        if(roads != null && roads.Count > 0)
        {
            roads = roads.OrderBy(r => r.transform.position.z).ToList();
        }
    }

    public void SpawnTriggerEntered()
    {
        MoveRoad();
    }

    public void MoveRoad()
    {
        GameObject moveRoad = roads[0];
        roads.Remove(moveRoad);
        float newZ = roads[roads.Count - 1].transform.position.z + offset;
        moveRoad.transform.position = new Vector3(0, 0, newZ);
        roads.Add(moveRoad);

        DecoSpawner[] decoSpawners = moveRoad.GetComponentsInChildren<DecoSpawner>();
        IslandSpawner[] islandSpawners = moveRoad.GetComponentsInChildren<IslandSpawner>();

        foreach(DecoSpawner decoSpawner in decoSpawners)
        {
            decoSpawner.SpawnDecos();
        }

        foreach(IslandSpawner islandSpawner in islandSpawners)
        {
            islandSpawner.SpawnIsland();
        }
    }
}

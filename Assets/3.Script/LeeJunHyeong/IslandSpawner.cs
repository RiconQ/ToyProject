using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawner : MonoBehaviour
{
    private List<GameObject> Islands;

    private float Width;
    private float Height;

    private Collider SpawnerCollider; // 스폰 범위 지정
    GameObject PrevIsland; 

    private void Awake()
    {
        Islands = new List<GameObject>();
        SpawnerCollider = GetComponent<Collider>();

        Width = (SpawnerCollider.bounds.size.x * 0.5f);
        Height = (SpawnerCollider.bounds.size.z * 0.5f);

        foreach (Transform child in transform)
        {
            Islands.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        PrevIsland = Islands[0];

        SpawnIsland();
    }

    public void SpawnIsland()
    {
        PrevIsland.SetActive(false);

        int whichIsland = Random.Range(0, Islands.Count);
        int spawnFlag = Random.Range(0, 3);

            Vector3 SpawnPos = new Vector3(
                Random.Range(-Width, Width),
                -1,
                Random.Range(-Height, Height)
                );

        if (spawnFlag.Equals(1))
        {
            Islands[whichIsland].SetActive(true);
            Islands[whichIsland].transform.position = transform.position + SpawnPos;
            PrevIsland = Islands[whichIsland];
        }

    }

}

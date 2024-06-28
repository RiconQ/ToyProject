using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoSpawner : MonoBehaviour
{
    private List<GameObject> Decos;
    private float MaxX;
    private float MaxY;
    private float MaxZ;

    private Collider SpawnerCollider;

    private void Awake()
    {
        Decos = new List<GameObject>();
        SpawnerCollider = GetComponent<Collider>();

        MaxX = (SpawnerCollider.bounds.size.x * 0.5f);
        MaxY = (SpawnerCollider.bounds.size.y * 0.5f);
        MaxZ = (SpawnerCollider.bounds.size.z * 0.5f);

    }

    private void Start()
    {
        Decos = new List<GameObject>();

        foreach(Transform child in transform)
        {
            Decos.Add(child.gameObject);
        }

        foreach (GameObject deco in Decos)
        {
            int Spawnflag = Random.Range(0, 2);

            Vector3 SpawnPos = new Vector3(
                Random.Range(-MaxX, MaxX),
                Random.Range(-MaxY, MaxY),
                Random.Range(-MaxZ, MaxZ)
                );

            if (Spawnflag.Equals(1))
            {
                deco.SetActive(true);
            }

            else
            {
                deco.SetActive(false);
            }

            deco.transform.position = transform.position + SpawnPos;
        }
    }

    public void SpawnDecos()
    {
        foreach(GameObject deco in Decos)
        {
            int Spawnflag = Random.Range(0, 2);

            Vector3 SpawnPos = new Vector3(
                Random.Range(-MaxX, MaxX),
                Random.Range(-MaxY, MaxY),
                Random.Range(-MaxZ, MaxZ)
                );

            if(Spawnflag.Equals(1))
            {
                deco.SetActive(true);
            }

            else
            {
                deco.SetActive(false);
            }

            deco.transform.position = transform.position + SpawnPos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ob_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] ObPrefabs = new GameObject[2];
    [SerializeField] private float SpawnTime;
    [SerializeField] Stage_Data stage_data;

    private Queue<GameObject> Enemy_Q;
    private Vector3 Poolposition;
    [SerializeField] private int Ob_PoolCount = 10;

    private void Awake()
    {
        Enemy_Q = new Queue<GameObject>();
        Poolposition = new Vector3(0f, 0f, stage_data.LimitMax.z);
        for (int i = 0; i < Ob_PoolCount; i++)
        {
            int rand = Random.Range(0, ObPrefabs.Length);
            GameObject enemy = Instantiate(ObPrefabs[rand], ObPrefabs[rand].transform.position, ObPrefabs[rand].transform.rotation);
            enemy.SetActive(false);
            Enemy_Q.Enqueue(enemy);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy_co());
    }

    public void Ob_enable()
    {
        if (Enemy_Q.Count > 0)
        {
            GameObject enemy = Enemy_Q.Dequeue();
            //Vector3 initialPosition = enemy.transform.position;
            // position.y = initialPosition.y; // 프리팹의 초기 y 값을 유지
            Vector3 position = new Vector3(transform.position.x, enemy.transform.position.y, transform.position.z);
            enemy.transform.position = position;
            if (!enemy.activeSelf)
                enemy.SetActive(true);
            StartCoroutine(DisableAfterTime(enemy, 1f)); // 10초 후 비활성화
        }
    }

    public void Ob_Disable(GameObject Ob)
    {
        if (Ob.activeSelf)
        {
            Ob.SetActive(false);
        }
        Enemy_Q.Enqueue(Ob);
    }

    private IEnumerator DisableAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Ob_Disable(obj);
    }

    private IEnumerator SpawnEnemy_co()
    {
        WaitForSeconds wfs = new WaitForSeconds(SpawnTime);
        while (true)
        {
           // float positionX = 0;
           // Vector3 position = new Vector3(positionX, 0, stage_data.LimitMax.z + 1f);
            Ob_enable();
            yield return wfs;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ob_Control : MonoBehaviour
{
    [SerializeField] private Stage_Data stage_data;
    private float destoryWieght = 6.0f;
    [SerializeField] private Ob_Spawner Spawner;


    private void OnEnable()
    {
        //player = GameObject.FindGameObjectWithTag("Player")
        //        .GetComponent<PlayerControll>();
        GameObject.FindObjectOfType<Ob_Spawner>().TryGetComponent(out Spawner);
    }

   
   // private void LateUpdate()  //  시간으로 수정
   // {
   //     if (transform.position.z < stage_data.LimitMin.z - destoryWieght ||
   //         transform.position.z > stage_data.LimitMax.z + destoryWieght ||
   //         transform.position.x < stage_data.LimitMin.x - destoryWieght ||
   //         transform.position.x > stage_data.LimitMax.x + destoryWieght)
   //     {
   //         onDie();
   //     }
   // }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
//플레이어 삭제?
        }
    }
    public void onDie()
    {
        //Destroy(gameObject);
        Spawner.Ob_Disable(gameObject);
    }



}

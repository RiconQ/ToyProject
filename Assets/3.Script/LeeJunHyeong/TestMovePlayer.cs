using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovePlayer : MonoBehaviour
{
    public float movementSpeed = 100f;
    [SerializeField] public RoadSpawner spawner;

    void Update()
    {
        float forBackMovement = Input.GetAxisRaw("Horizontal") * movementSpeed;
        float leftRightMovement = Input.GetAxisRaw("Vertical") * movementSpeed;

        Vector3 Direction = new Vector3(forBackMovement, 0f, leftRightMovement);

        transform.Translate(Direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MapSpawnTrigger"))
        spawner.SpawnTriggerEntered();
    }
}

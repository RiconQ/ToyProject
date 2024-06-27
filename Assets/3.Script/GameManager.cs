using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{        
    public Player Player;

    // Start is called before the first frame update
    void Start()
    {        
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Player.PlayerInput();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private EnemySpawner spawner;
    private PlayerController player;
    LaserSpawner laser;


	InputHandler input;
	[Inject]
    private void Construct(EnemySpawner spawner, PlayerController player, InputHandler inputHandler,LaserSpawner laser ) {
        this.spawner = spawner;
        this.player = player;
        this.input = inputHandler;
        this.laser = laser;
    }

    // Start is called before the first frame update
    void Start()
    {
       spawner.Spawn(13);
        
        
    }

    // Update is called once per frame
    void Update()
    {
       // if (!input.Idle )
        {
			player.Update1(input.Direction, input.MousePosition);

		}
        if (input.Fire) {
            player.Fire(laser.Spawn(player.Position));
        }
        laser.Update1();
    }
}

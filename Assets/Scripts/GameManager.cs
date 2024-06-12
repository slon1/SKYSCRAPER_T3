using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Zenject;

public class GameManager : MonoBehaviour
{
    private EnemySpawner spawner;
    private PlayerController player;
    private VfxPool vfx;
    
	LaserSpawner laser;
	EncryptedStorage storage = Utils.EncryptedStorage.Instance;

	InputHandler input;
	[Inject]
    private void Construct(EnemySpawner spawner, PlayerController player, InputHandler inputHandler,LaserSpawner laser, VfxPool vfx ) {
        this.spawner = spawner;
        this.player = player;
        this.input = inputHandler;
        this.laser = laser;
        this.vfx = vfx;
    }

    // Start is called before the first frame update
    void Start()
    {
       spawner.Spawn(13, player);
        player.SetHealth(2);
		player.OnGameOver += Player_OnGameOver;
		spawner.OnWaveEnd += Spawner_OnWaveEnd;
        storage.Initialize("save.txt","12345");
        
        

	}

	

	private void OnDestroy() {
		player.OnGameOver -= Player_OnGameOver;
		spawner.OnWaveEnd -= Spawner_OnWaveEnd;
        storage.Dispose();
	}

	private void Player_OnGameOver() {
        vfx.Spawn(player.Position);
       // Destroy(player.gameObject);
	}
	private void Spawner_OnWaveEnd() {
        spawner.Spawn(10,player);
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

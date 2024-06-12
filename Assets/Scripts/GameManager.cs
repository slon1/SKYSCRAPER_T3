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
    EncryptedStorage storage;
	LaserSpawner laser;
	InputHandler input;
    bool run = false;
    int wave = 0;
	int enemyCount => 10 + wave * Random.Range(2, 5);
	[Inject]
    private void Construct(EnemySpawner spawner, PlayerController player, InputHandler inputHandler,LaserSpawner laser, VfxPool vfx, EncryptedStorage storage ) {
        this.spawner = spawner;
        this.player = player;
        this.input = inputHandler;
        this.laser = laser;
        this.vfx = vfx;
        this.storage = storage;
    }
	private void Awake() {
		player.OnGameOver += Player_OnGameOver;
		spawner.OnWaveEnd += Spawner_OnWaveEnd;
	}
	// Start is called before the first frame update
	void Start()
    {
        

	   spawner.Spawn(enemyCount, player);
       player.SetHealth(5);
	   
        
        

	}

	public void Run() {
        run = true;
    }
	public void Stop() {
		run = false;
	}
	public void Restart() {
        spawner.Clear();
        laser.Clear();
        run = true;
        player.Reset();
        Start();

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
		print(wave + " " + enemyCount);
		wave++;
        print(wave+" "+ enemyCount);
        print(FindObjectsOfType<GameManager>().Length);
        spawner.Spawn(enemyCount,player);
	}
	// Update is called once per frame
	void Update()
    {
        if (!run) { return; }
	    
        player.Update1(input.Direction, input.MousePosition);

        if (input.Fire) {
            player.Fire(laser.Spawn(player.Position));
        }
        laser.Update1();
        spawner.Update1();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Zenject;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
	[SerializeField]
	private int PlayrHP = 5;
	[SerializeField]
	private int PlayerSpeed = 8;
	
	public event Action OnGameOver;
	private EnemySpawner spawner;
	private PlayerController player;
	private VfxPool vfx;
	EncryptedStorage storage;
	LaserSpawner laser;
	InputHandler input;
	bool run = false;
	int wave = 0;
	CancellationTokenSource source;
	private CancellationToken token;
	public int levelCount = 5;
	private int level;
	int enemyCount => 10 + wave * Random.Range(2, 6);
	[Inject]
	private void Construct(EnemySpawner spawner, PlayerController player, InputHandler inputHandler, LaserSpawner laser, VfxPool vfx, EncryptedStorage storage) {
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
		source = new CancellationTokenSource();
		token = source.Token;

	}

	public void StartGame() {

		wave = 0;
		spawner.Spawn(enemyCount, player);
		player.Init(PlayrHP, PlayerSpeed);
		run = true;
		player.Reset();


	}


	public void StopGame() {
		spawner?.Clear();
		laser?.Clear();
		run = false;
	}



	private void OnDestroy() {
		player.OnGameOver -= Player_OnGameOver;
		spawner.OnWaveEnd -= Spawner_OnWaveEnd;
		storage.Dispose();
		source.Cancel();
	}

	private async void Player_OnGameOver() {
		run = false;
		vfx.Spawn(player.Position);
		try {
			await Task.Delay(3000, token);
		}
		catch (Exception) { }
		OnGameOver?.Invoke();

	}
	private void Spawner_OnWaveEnd() {
		wave++;
		print(enemyCount);
		spawner.Spawn(enemyCount, player);
		if (wave >= levelCount) {
			OnGameOver?.Invoke();
		}
	}
	
	void Update() {
		if (!run) { return; }

		player.ManualUpdate(input.Direction, input.MousePosition);

		if (input.Fire) {
			player.Fire(laser.Spawn(player.Position));
		}
		laser.ManualUpdate();
		spawner.ManualUpdate();
		input.ManualUpdate();
	}
}

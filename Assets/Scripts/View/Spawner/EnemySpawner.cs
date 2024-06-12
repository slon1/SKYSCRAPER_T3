using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour{
    
	public event Action OnWaveEnd;
	[SerializeField]
	private List<EnemyConfig> configs;
	[SerializeField]
	private int EnemyHP;
	[SerializeField]
	private int EnemySpeed;
	[SerializeField]
	private float spawnRadius = 15;

	private VfxPool vfx;
	private UIManager ui;
	private List<EnemyController> items;
	private EnemyPool pool;
	private PlayerController player;

	[Inject]
	private void Construct(EnemyPool pool, PlayerController player, VfxPool vfx, UIManager ui) {
        this.pool = pool;
        items = new();
		this.player = player;
		this.vfx = vfx;
		this.ui = ui;
	}

	public void Clear() {
		items.ForEach(x => pool.Despawn(x));
		items.Clear();
		pool.Clear();		
		
	}	

    public void Spawn(int count, PlayerController player) {
        for (int i = 0; i < count; i++) {
            EnemyController enemy = pool.Spawn(Random.insideUnitCircle * spawnRadius, configs[Random.Range(0,configs.Count)]);
			//enemy.Init(EnemyHP, EnemySpeed);
			items.Add(enemy);
            enemy.OnDespawned += OnDespawn;
			enemy.SetTarget(player.Renderer);
		}
    }

	private void OnDespawn(EnemyController ship) {
		ship.OnDespawned -= OnDespawn;
		items.Remove(ship);
		vfx.Spawn(ship.Position).Play();
		ui.AddScore(1);
		if (items.Count == 0) {			
			OnWaveEnd?.Invoke();			
		}
	}

	public void ManualUpdate() {

        for (int i = 0; i < items.Count; i++)
        {
			items[i].ManualUpdate();
		}
    }

	private void OnDestroy() {	
		pool = null;
		vfx = null;
		ui = null;		
		player = null;
	}
}

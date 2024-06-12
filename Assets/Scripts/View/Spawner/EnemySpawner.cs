using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    private EnemyPool pool;
	private PlayerController player;
	public event Action OnWaveEnd;
	private VfxPool vfx;
	private UIManager ui;
	private List<EnemyController> items;
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
	private void OnDestroy() {
        pool=null;
		vfx = null;
		items?.Clear();
	}
    public void Spawn(int count, PlayerController player) {
        for (int i = 0; i < count; i++) {
            EnemyController enemy = pool.Spawn(Random.insideUnitCircle * 11);            
			items.Add(enemy);
            enemy.OnDespawned += OnDespawn;
			enemy.SetTarget(player.Renderer);
		}
    }

	private void OnDespawn(EnemyController ship) {
		ship.OnDespawned -= OnDespawn;
		items.Remove(ship);
		vfx.Spawn(ship.Position).Play();
		ui.SetScore(1);
		if (items.Count == 0) {
			print(1111);
			OnWaveEnd?.Invoke();
			
		}
	}

	public void Update1() {

        for (int i = 0; i < items.Count; i++)
        {
			items[i].Update1();
		}


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class EnemySpawner : MonoBehaviour
{
    private EnemyPool pool;
	private PlayerController player;

	private List<EnemyController> items;
	[Inject]
	private void Construct(EnemyPool pool, PlayerController player) {
        this.pool = pool;
        items = new();
		this.player = player;
	}

	private void OnDestroy() {
        pool=null;        
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
	}

	private void Update() {
        items.ForEach(x => x.Update1());
	}

}

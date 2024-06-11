using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class EnemySpawner : MonoBehaviour
{
    private EnemyPool pool;
    
    private List<EnemyController> items;
	[Inject]
	private void Construct(EnemyPool pool) {
        this.pool = pool;
        items = new();
	}

	private void OnDestroy() {
        pool=null;        
        items?.Clear();
	}
    public void Spawn(int count) {
        for (int i = 0; i < count; i++) {
            EnemyController enemy = pool.Spawn(Random.insideUnitCircle * 9);            
			items.Add(enemy);
            enemy.OnDespawned += OnDespawn;
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

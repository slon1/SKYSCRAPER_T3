using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyPool : MonoMemoryPool<Vector3, EnemyController>
{
	protected override void OnCreated(EnemyController item) {
		base.OnCreated(item);
		
	}
	protected override void OnDespawned(EnemyController item) {
		base.OnDespawned(item);
		item.OnDespawned -= Item_OnDespawned;
	
	}
	
	protected override void Reinitialize(Vector3 spawnPosition, EnemyController item) {
		base.Reinitialize( spawnPosition, item);
		item.transform.position= spawnPosition;
		item.OnDespawned += Item_OnDespawned;

	}

	private void Item_OnDespawned(EnemyController obj) {
		Despawn(obj);
	}
}

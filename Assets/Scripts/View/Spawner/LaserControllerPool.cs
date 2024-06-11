using UnityEngine;
using Zenject;


public class LaserControllerPool : MonoMemoryPool<Vector3, LaserController> {
	protected override void Reinitialize(Vector3 spawnPosition, LaserController item) {
		base.Reinitialize(spawnPosition, item);
		item.transform.position = spawnPosition;
		item.OnDespawned += Item_OnDespawned;
	}
	protected override void OnDespawned(LaserController item) {
		base.OnDespawned(item);
		item.OnDespawned -= Item_OnDespawned;
		
	}

	private void Item_OnDespawned(LaserController obj) {
		Despawn(obj);
	}
}


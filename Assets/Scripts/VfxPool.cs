using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class VfxPool : MonoMemoryPool<Vector3, Vfx> {
	protected override void OnCreated(Vfx item) {
		base.OnCreated(item);

	}
	protected override void OnDespawned(Vfx item) {
		base.OnDespawned(item);
		item.OnDespawned -= Item_OnDespawned;

	}

	protected override void Reinitialize(Vector3 spawnPosition, Vfx item) {
		base.Reinitialize(spawnPosition, item);
		item.transform.position = spawnPosition;
		item.OnDespawned += Item_OnDespawned;

	}

	private void Item_OnDespawned(Vfx obj) {
		Despawn(obj);
	}
}

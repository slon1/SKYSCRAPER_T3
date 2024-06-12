using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using Zenject;


public class LaserSpawner : MonoBehaviour {
	private LaserControllerPool pool;
	[SerializeField]
	private float speed;
	[SerializeField]
	private float lifetime;
	[SerializeField]
	private int damage;

	private List<LaserController> items;
	[Inject]
	private void Construct(LaserControllerPool pool) {
		this.pool = pool;
		items = new();
	}

	private void OnDestroy() {
		pool = null;
		items?.Clear();
	}
	public void Clear() {
		items?.Clear();
		pool?.Clear();
	}
	public LaserController Spawn(Vector3 position) {
		var laser = pool.Spawn(Vector3.zero);
		laser.Initialize(speed, lifetime, damage);
		laser.transform.position = position;
		items.Add(laser);
		laser.OnDespawned += OnDespawn;
		return laser;

	}
	private void OnDespawn(LaserController laser) {
		laser.OnDespawned -= OnDespawn;
		items.Remove(laser);

	}
	public void ManualUpdate() {
		for (int i = 0; i < items.Count; i++) {
			items[i].Update1();
		}

	}


}

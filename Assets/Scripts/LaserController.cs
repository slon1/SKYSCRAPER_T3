using System;
using Unity.VisualScripting;
using UnityEngine;

public class LaserController : MonoBehaviour {
	
	private float speed ;
	private float lifetime;
	private int damage;
	public event Action<LaserController> OnDespawned;
	public TrailRenderer TrailRenderer;
	public void Update1() {
		Move();
		CheckLifetime();
		CheckCollision();
	}

	public void SetForward(Vector2 dir) {
		transform.up=dir;
	}

	private void Move() {
		transform.Translate(transform.up * speed * Time.deltaTime,Space.World);
	}

	private void CheckLifetime() {
		lifetime -= Time.deltaTime;
		if (lifetime <= 0f) {
			OnDespawned?.Invoke(this);
			TrailRenderer.Clear();
		}		
	}

	private void CheckCollision() {
		var collider = Physics2D.OverlapPoint(transform.position);
		if (collider) {
			IDamageable damageable = collider.GetComponent<IDamageable>();
			if (damageable!=null && damageable is EnemyController) {
				damageable.TakeDamage(damage);
				OnDespawned?.Invoke(this);
				TrailRenderer.Clear();
			}
		}
	}

	public void Initialize(float speed, float lifetime, int damage) {		
		this.speed = speed;
		this.lifetime = lifetime;
		this.damage = damage;
	}
}

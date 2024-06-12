using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Zenject;
public enum CollisionType { none, enemy, player}
public class EnemyController : MonoBehaviour , IDamageable {
    private EnemyModel model;
    public EnemyView view;
	public event Action<EnemyController> OnDespawned;
	public Vector2 target;
	
	public int Health => model.Health;
	public Vector3 Position=>transform.position;
	public float Speed => model.Speed;

	public Vector2 Direction { get => model.Direction; set => model.Direction = value; }

	public bool IsAlive => model.IsAlive;

	public void TakeDamage(int damage) {
		model.TakeDamage(damage);
		if (!IsAlive) {
			OnDespawned?.Invoke(this);
		}
	}


	private void Start() {
		model = new EnemyModel();
		model.Direction = transform.up;
		
	}

	private CollisionType CheckCollision() {
		
		var colliders = Physics2D.OverlapCircleAll(transform.position, view.Renderer.bounds.extents.y*0.5f);		
		foreach (Collider2D collider in colliders) {
			if (!collider.gameObject.Equals(gameObject)) {				
				IDamageable damageable = collider.GetComponent<IDamageable>();
				if (damageable != null) { 
					if (damageable is PlayerController) {
						damageable.TakeDamage(1);
						TakeDamage(1);
						return CollisionType.player;
					}
					if (damageable is EnemyController) {
						return CollisionType.enemy;
					}
				}				
			}			
			//damageable.TakeDamage(damage);
			//OnDespawned?.Invoke(this);
		}
		return CollisionType.none;
	}

	public void OnDestroy() {
		//OnDespawned?.Invoke(this);
	}
	public void Update1() {
		float scale = 0.5f;
		float ampltude = 360;


		 float minAngle = 0f; // Минимальный угол (0 градусов)
	     float maxAngle = 360f; // Максимальный угол (360 градусов)

	    var f = (Mathf.PerlinNoise(transform.localPosition.x*scale, transform.localPosition.y*scale)-0.5f) * ampltude;
		//float Angle = Mathf.Lerp(minAngle, maxAngle, f);
		//var r = Vector2.Lerp(Vector2.up, Vector2.down, f);
		

		var direction = (playerRender.bounds.center - view.Renderer.bounds.center).normalized;		
		Vector3 desiredDirection = (direction + CalculateAvoidance()).normalized;

		//Debug.DrawRay(view.Renderer.bounds.center, CalculateAvoidance(), Color.red, 1);
		//Debug.DrawRay(view.Renderer.bounds.center, desiredDirection, Color.blue, 1);
		
		
		switch (CheckCollision()) {
			case CollisionType.none:
				
				view.MoveTowards(desiredDirection, 2);
				break;
			case CollisionType.enemy:
				
				view.MoveTowards(desiredDirection, 2);
				break;
			case CollisionType.player:
				
				break;
		}

		//var target= new Vector2 (0, 0 );

	}

	Vector3 CalculateAvoidance() {
		Vector3 avoidance = Vector3.zero;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, view.Renderer.bounds.extents.y * 5f);

		foreach (Collider2D collider in colliders) {
			if (collider.transform != transform) {
				Vector3 difference = transform.position - collider.transform.position;
				avoidance += difference.normalized / difference.magnitude;
			}
		}

		return Vector3.ClampMagnitude(avoidance, view.Renderer.bounds.extents.y * 5f);
	}
	private Renderer playerRender;
	internal void SetTarget(SpriteRenderer renderer) {
		this.playerRender= renderer;
	}
}

using System;
using UnityEngine;
public enum CollisionType { none, enemy, player}
public class EnemyController : MonoBehaviour , IDamageable {
    private EnemyModel model;
	[SerializeField]
	private EnemyView view;
	public event Action<EnemyController> OnDespawned;	
	public int Health => model.Health;
	public Vector3 Position=>transform.position;
	public float Speed => model.Speed;
	public Vector2 Direction { get => model.Direction; set => model.Direction = value; }
	public bool IsAlive => model.IsAlive;
	private Renderer playerRender;
	private float radius = 5f;
	public void TakeDamage(int damage) {
		model.TakeDamage(damage);
		if (!IsAlive) {
			OnDespawned?.Invoke(this);
		}
	}
	
	public void Init(int hp, int speed) {
		model = new EnemyModel(hp, speed);
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
		}
		return CollisionType.none;
	}

	public void OnDestroy() {
		model = null;
		view = null;
	}
	public void ManualUpdate() {			

		var direction = (playerRender.bounds.center - view.Renderer.bounds.center).normalized;		
		Vector3 desiredDirection = (direction + CalculateAvoidance()).normalized;
		
		switch (CheckCollision()) {
			case CollisionType.none:				
				view.MoveTowards(desiredDirection, model.Speed);
				break;
			case CollisionType.enemy:				
				view.MoveTowards(desiredDirection, model.Speed);
				break;
			case CollisionType.player:				
				break;
		}
	}
	
	Vector3 CalculateAvoidance() {
		Vector3 avoidance = Vector3.zero;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, view.Renderer.bounds.extents.y * radius);

		foreach (Collider2D collider in colliders) {
			if (collider.transform != transform) {
				Vector3 difference = transform.position - collider.transform.position;
				avoidance += difference.normalized / difference.magnitude;
			}
		}
		return Vector3.ClampMagnitude(avoidance, view.Renderer.bounds.extents.y * radius);
	}	
	public void SetTarget(SpriteRenderer renderer) {
		playerRender= renderer;
	}
}

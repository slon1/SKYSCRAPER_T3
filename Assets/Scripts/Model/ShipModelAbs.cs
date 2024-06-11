using UnityEngine;

public abstract class ShipModelAbs : IDamageable {
	protected float speed;
	protected int health;
	protected Vector2 direction;
	public int Health => health;

	public bool IsAlive => health > 0;

	public float Speed => speed;	

	public Vector2 Direction { get => direction; set => direction = value; }

	public virtual void TakeDamage(int damage) {
		health = Mathf.Clamp(health - damage, 0, int.MaxValue);		
	}
}

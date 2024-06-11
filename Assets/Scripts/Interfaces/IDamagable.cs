using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageable {
	int Health { get; }
	float Speed { get; }
	Vector2 Direction { get; set; }
	void TakeDamage(int damage);
	bool IsAlive { get; }
	
}

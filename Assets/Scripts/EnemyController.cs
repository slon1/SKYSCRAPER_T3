using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Zenject;

public class EnemyController : MonoBehaviour , IDamageable {
    private EnemyModel model;
    public EnemyView view;
	public event Action<EnemyController> OnDespawned;
	public Vector2 target;
	
	public int Health => model.Health;

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
	public void Destroy() {
		OnDespawned?.Invoke(this);
	}
	public void Update1() {
		float scale = 0.5f;
		float ampltude = 360;


		 float minAngle = 0f; // Минимальный угол (0 градусов)
	     float maxAngle = 360f; // Максимальный угол (360 градусов)

	    var f = (Mathf.PerlinNoise(transform.localPosition.x*scale, transform.localPosition.y*scale)-0.5f) * ampltude;
		//float Angle = Mathf.Lerp(minAngle, maxAngle, f);
		//var r = Vector2.Lerp(Vector2.up, Vector2.down, f);

		//var target= new Vector2 (0, 0 );
		view.MoveTowards(target, 1);
	}

	
}

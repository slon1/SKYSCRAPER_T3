using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour, IDamageable

{
	private PlayerModel model;
	[SerializeField]
	private PlayerView view;
	UIManager uI;
	private const int up = 90;
	public event Action OnGameOver;
	[Inject]
	private void Construct(UIManager uI) {
		this.uI=uI;
	}

	void Start()
    {
		model = new PlayerModel ();
		model.Direction = view.transform.rotation.eulerAngles;		
		
	}
	public Vector3 Position => view.transform.position;
	
	public SpriteRenderer Renderer =>view.Renderer;
	public int Health => model.Health;

	public float Speed => model.Speed;

	public Vector2 Direction { get => model.Direction; set => model.Direction = value; }

	public bool IsAlive => model.IsAlive;

	//public void Destroy() {
	//	OnDespawned?.Invoke(this);
	//}
	// Update is called once per frame
	public void ManualUpdate(Vector2 target, Vector2 lookAt)    {
		model.Direction = (lookAt - (Vector2)transform.position).normalized;
		float angle = Mathf.Atan2(model.Direction.y, model.Direction.x) * Mathf.Rad2Deg-up;
		
		view.MoveTowards(target, 10);
		view.RotateTowards(new Vector3(0, 0, angle));
		//Debug.DrawRay(transform.position, model.Direction, Color.blue, 1);
		//model.Direction=direction.
	
    }

	public void Fire(LaserController shot) {
		shot.SetForward(transform.up);
		
	}

	public void TakeDamage(int damage) {
		model.TakeDamage(damage);		
		if (!IsAlive) {
			OnGameOver?.Invoke();		
		}
		uI.SetHP(model.Health);
	}

	public void SetHealth(int hp) {
		
		model.SetHealth(hp);
		uI.SetHP(model.Health);
	}

	internal void Reset() {
		transform.position = Vector3.zero;
	}

	internal void Visible(bool v) {
		view.Renderer.enabled = false;
	}
}

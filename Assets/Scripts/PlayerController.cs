using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private PlayerModel model;
	public PlayerView view;
	public event Action<PlayerController> OnDespawned;
	private const int up = 90;
	// Start is called before the first frame update
	void Start()
    {
		model = new PlayerModel ();
		model.Direction = view.transform.rotation.eulerAngles;
	}
	public Vector3 Position => view.transform.position;
	public void Destroy() {
		OnDespawned?.Invoke(this);
	}
	// Update is called once per frame
	public void Update1(Vector2 target, Vector2 lookAt)    {
		model.Direction = (lookAt - (Vector2)transform.position).normalized;
		float angle = Mathf.Atan2(model.Direction.y, model.Direction.x) * Mathf.Rad2Deg-up;
		
		view.MoveTowards(target, 10);
		view.RotateTowards(new Vector3(0, 0, angle));
		Debug.DrawRay(transform.position, model.Direction, Color.blue, 10);
		//model.Direction=direction.
	
    }

	public void Fire(LaserController shot) {
		shot.SetForward(transform.up);
		
	}
}
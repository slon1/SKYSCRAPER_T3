using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : ShipView
{
	
	
	public override void MoveTowards(Vector3 target, float speed) {		
		
		transform.Translate(target * speed * Time.deltaTime, Space.World);
		Debug.DrawRay(transform.position, transform.up, Color.red, 1);

	}

	public void RotateTowards(Vector3 target) {
		transform.rotation = Quaternion.Euler(target);
	}
}

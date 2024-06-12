using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : ShipView{
	public override void MoveTowards(Vector3 target, float speed) {		
		transform.Translate(target * speed * Time.deltaTime, Space.World);		
		Vector3 clampedPosition = ClampPositionToScreenBounds(transform.position);
		transform.position = clampedPosition;
	}
	
	private Vector3 ClampPositionToScreenBounds(Vector3 position) {
	
		Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

	
		if (position.x < -screenBounds.x) {
			position.x = -screenBounds.x;
		}
		else if (position.x > screenBounds.x) {
			position.x = screenBounds.x;
		}
	
		if (position.y < -screenBounds.y) {
			position.y = -screenBounds.y;
		}
		else if (position.y > screenBounds.y) {
			position.y = screenBounds.y;
		}

		return position;
	}

	public void RotateTowards(Vector3 target) {
		transform.rotation = Quaternion.Euler(target);
	}

	
}

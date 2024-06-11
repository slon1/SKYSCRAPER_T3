using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class EnemyView : ShipView {		
	public override void MoveTowards(Vector3 target, float speed) {
		//Vector2 dir = target - transform.position;
		transform.rotation= Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward,target),Time.deltaTime);
		//transform.up = Vector3.RotateTowards(target,transform.up,0.1f,Time.deltaTime);
		transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
		//Debug.DrawRay(transform.position, transform.up, Color.red, 1);
	}

}

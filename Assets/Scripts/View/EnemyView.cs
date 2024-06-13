using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class EnemyView : ShipView {		
	public override void MoveTowards(Vector3 target, float speed) {		
		transform.rotation= Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward,target),Time.deltaTime);	
		transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
		
	}

}

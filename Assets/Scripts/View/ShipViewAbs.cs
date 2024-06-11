using UnityEngine;

public abstract class ShipView : MonoBehaviour, IMovable {
	[SerializeField]
	private SpriteRenderer render;
	
	public virtual void MoveTowards(Vector3 target, float speed) {		
		
	}
}

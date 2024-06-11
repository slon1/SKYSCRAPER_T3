using UnityEngine;

public abstract class ShipView : MonoBehaviour, IMovable {
	[SerializeField]
	private SpriteRenderer render;
	public Bounds Bounds=>render.bounds;
	
	public virtual void MoveTowards(Vector3 target, float speed) {		
		
	}
}

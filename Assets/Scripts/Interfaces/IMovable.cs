using UnityEngine;

public interface IMovable {
	void MoveTowards(Vector3 target, float speed);
}
public interface IWeapon {
	void Fire(Vector2 direction);
}
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 1)]
public class EnemyConfig : ScriptableObject {
	public string enemyName;
	public Sprite sprite;
	public int health;
	public int speed;
}

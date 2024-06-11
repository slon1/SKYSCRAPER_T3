using UnityEngine;

public class InputHandler : MonoBehaviour {
	public KeyCode FireKey;
	public bool Fire => Input.GetKeyDown(FireKey)||Input.GetMouseButtonDown(0);
	public float Horizontal { get; private set; }
	public float Vertical { get; private set; }

	public Vector2 Direction=>new Vector2(Horizontal, Vertical);
	public Vector2 MousePosition { get; private set; }
	public bool Idle => !Input.anyKey;
	void Update() {
		
		Horizontal = Input.GetAxis("Horizontal");
		Vertical = Input.GetAxis("Vertical");
		
		
		MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}

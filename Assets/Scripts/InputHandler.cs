using UnityEngine;

public class InputHandler : MonoBehaviour {
	public KeyCode FireKey;
	public bool Fire => Input.GetKeyDown(FireKey) || Input.GetMouseButtonDown(0);
	public float Horizontal { get; private set; }
	public float Vertical { get; private set; }

	public Vector2 Direction => new Vector2(Horizontal, Vertical);
	public Vector2 MousePosition { get; private set; }
	public bool Idle => !Input.anyKey;

	private Vector2 screenBounds;
	

	void Start() {
		// Вычисляем границы экрана в мировых координатах
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
	}

	public void ManualUpdate() {
	
		// Получаем ввод пользователя
		Horizontal = Input.GetAxis("Horizontal");
		Vertical = Input.GetAxis("Vertical");
		MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Получаем текущую позицию игрока
		Vector3 playerPosition = transform.position;

		// Проверяем и ограничиваем перемещение по горизонтали
		if (playerPosition.x < -screenBounds.x) {
			playerPosition.x = -screenBounds.x;
		}
		else if (playerPosition.x > screenBounds.x) {
			playerPosition.x = screenBounds.x;
		}

		// Проверяем и ограничиваем перемещение по вертикали
		if (playerPosition.y < -screenBounds.y) {
			playerPosition.y = -screenBounds.y;
		}
		else if (playerPosition.y > screenBounds.y) {
			playerPosition.y = screenBounds.y;
		}

		// Применяем ограниченные координаты к позиции игрока
		transform.position = playerPosition;
	}
}

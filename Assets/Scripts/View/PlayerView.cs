using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : ShipView
{


	public override void MoveTowards(Vector3 target, float speed) {
		// Перемещаем объект
		transform.Translate(target * speed * Time.deltaTime, Space.World);

		// Ограничиваем позицию в пределах границ экрана
		Vector3 clampedPosition = ClampPositionToScreenBounds(transform.position);
		transform.position = clampedPosition;
	}

	// Метод для ограничения позиции в пределах границ экрана
	private Vector3 ClampPositionToScreenBounds(Vector3 position) {
		// Вычисляем границы экрана в мировых координатах
		Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

		// Ограничиваем перемещение по горизонтали
		if (position.x < -screenBounds.x) {
			position.x = -screenBounds.x;
		}
		else if (position.x > screenBounds.x) {
			position.x = screenBounds.x;
		}

		// Ограничиваем перемещение по вертикали
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

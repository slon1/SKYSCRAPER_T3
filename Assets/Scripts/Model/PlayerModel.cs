using System;

public class PlayerModel : ShipModelAbs {
	public void SetHealth(int hp) {
		health = hp;
	}

	internal void SetSpeed(int speed) {
		this.speed = speed;
	}
}

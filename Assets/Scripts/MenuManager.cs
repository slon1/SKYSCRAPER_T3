
using System.Collections;

using UnityEngine;
using Utils;
using Zenject;
using Zenject.SpaceFighter;


public class MenuManager : MonoBehaviour {
	public enum GameState { Loading, MainMenu, Gameplay, Exit }
	private GameState currentState;	
	private UIManager uiManager;
	EncryptedStorage storage;
	GameManager gameManager;

	[Inject]
	private void Construct(UIManager uiManager, EncryptedStorage storage, GameManager gameManager ) {
		this.uiManager = uiManager;
		this.storage = storage;
		this.gameManager = gameManager;
	}


	void Start() {
		ChangeState(GameState.Loading);
	}

	public void ChangeState(GameState newState) {
		currentState = newState;
		switch (newState) {
			case GameState.Loading:
				StartCoroutine(LoadingRoutine());
				break;
			case GameState.MainMenu:
				uiManager.ShowMainMenu();
				gameManager.Stop();
				break;
			case GameState.Gameplay:
				uiManager.HideMainMenu();
				Load();
				gameManager.Restart();
				//saveSystem.LoadGame();
				break;
			case GameState.Exit:
				Save();
				Application.Quit();
				break;
		}
	}


	
	public void Save() {
		storage.SetInt("mydata", int.Parse(uiManager.GetScore()));
	}
	public async void Load() {

		uiManager.SetScore(storage.GetInt("mydata"));
	}	

	private IEnumerator LoadingRoutine() {
		float elapsedTime = 0f;
		while (elapsedTime < 3) {
			uiManager.Spin.transform.Rotate(-Vector3.forward, 500 * Time.deltaTime);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		ChangeState(GameState.MainMenu);
	}
	private void OnDestroy() {

	}
}


using System.Collections;

using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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
		gameManager.OnGameOver += GameManager_OnGameOver;
	}

	private void GameManager_OnGameOver() {
		ChangeState(GameState.MainMenu);
	}

	public void ChangeState(GameState newState) {
		currentState = newState;
		switch (newState) {
			case GameState.Loading:
				StartCoroutine(LoadingRoutine());
				break;
			case GameState.MainMenu:
				Load();
				uiManager.ShowMainMenu();
				gameManager.Stop();
				break;
			case GameState.Gameplay:
				uiManager.HideMainMenu();
				uiManager.SetScore(0);
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
		int scoreTop= storage.GetInt("mydata");
		if (int.TryParse(uiManager.GetScore(), out var score)) {
			scoreTop = Mathf.Max(scoreTop, score);
		}
		print("asve " + scoreTop);
		storage.SetInt("mydata", scoreTop);
	}
	public async void Load() {
		int scoreTop = storage.GetInt("mydata");
		if (int.TryParse(uiManager.GetScore(), out var score)) {
			uiManager.SetScoreTop(storage.GetInt("mydata"));
		}
		print("load " + storage.GetInt("mydata"));

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
		gameManager.OnGameOver -= GameManager_OnGameOver;
	}
}

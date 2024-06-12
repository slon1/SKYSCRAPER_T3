using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Utils;
using Zenject;

public class UIManager : MonoBehaviour
{
	
	public GameObject mainMenu;
	public GameObject gameplayUI;
	public GameObject Loader;
	public GameObject Spin;
	public Text Score;
	public Text ScoreTop;
	public Text HP;
	private EncryptedStorage storage;
	private MenuManager menuManager;
	public int TopScore { get; set; }
	[Inject]
	private void Construct(MenuManager menuManager, EncryptedStorage storage) {
		this.menuManager = menuManager;
		this.storage = storage;

	}
	public void SetScore(int score) {
		if (int.TryParse(Score.text, out var scoreOrg))
		Score.text =  (scoreOrg+ score).ToString();
		SetScoreTop(scoreOrg + score);
	}
	public void SetScoreTop(int score) {
		if (int.TryParse(ScoreTop.text, out var scoreOrg))
			ScoreTop.text = Mathf.Max (scoreOrg , score).ToString();
	}
	public string GetScore() {
		return Score.text;
	}
	public void SetHP(int health) {
		HP.text = health.ToString();
	}
	public void ShowMainMenu() {
		mainMenu.SetActive(true);
		gameplayUI.SetActive(false);
		Loader.SetActive(false);
	}

	public void HideMainMenu() {
		mainMenu.SetActive(false);
		gameplayUI.SetActive(true);
		Loader.SetActive(false);
	}

	public void OnStartButtonClicked() {
		menuManager.ChangeState(MenuManager.GameState.Gameplay);
		//Score.text= storage.GetString("mydata")??"0";
	}

	public void OnBackButtonClicked() {
		menuManager.ChangeState(MenuManager.GameState.MainMenu);
	}

	public void OnExitButtonClicked() {

		menuManager.ChangeState(MenuManager.GameState.Exit);
		//storage.SetString("mydata",Score.text);
	}

	
}


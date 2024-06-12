using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject mainMenu;
	[SerializeField]
	private GameObject gameplayUI;
	[SerializeField]
	private GameObject Loader;	
	public GameObject Spin;
	[SerializeField]
	private Text Score;
	[SerializeField]
	private Text ScoreTop;
	[SerializeField]
	private Text HP;
	private EncryptedStorage storage;
	private MenuManager menuManager;
	public int TopScore { get; set; }
	[Inject]
	private void Construct(MenuManager menuManager, EncryptedStorage storage) {
		this.menuManager = menuManager;
		this.storage = storage;

	}

	public void ScoreReset() {
		Score.text = "0";
	}

	public void AddScore(int score) {
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
		
	}

	public void OnBackButtonClicked() {
		menuManager.ChangeState(MenuManager.GameState.MainMenu);
	}

	public void OnExitButtonClicked() {
		menuManager.ChangeState(MenuManager.GameState.Exit);
		
	}
	
}


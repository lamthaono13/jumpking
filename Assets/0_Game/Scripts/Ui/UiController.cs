using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField]
    private UiMainLobby _uiLobby;

    [SerializeField]
    private UiEndGame _uiEndGame;

    [SerializeField]
    private Text _score;

    [SerializeField]
    private Text _scoreEndGame;

    [SerializeField]
    private Text _bestScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetScore(int score)
    {
        _score.text = score.ToString();
        _scoreEndGame.text = "Score: " + score.ToString();
    }

    public void SetBestScore()
    {
        int max = PlayerData.GetMaxScore();
        _bestScore.text = "Best Score: " + max.ToString();
    }

    public void OnMainLobby()
    {
        _score.text = "0";
        _uiLobby.gameObject.SetActive(true);
        _uiEndGame.gameObject.SetActive(false);
    }

    public void OnIngame()
    {
        _uiLobby.gameObject.SetActive(false);
        _uiEndGame.gameObject.SetActive(false);
    }

    public void OnEndGame()
    {
        _uiLobby.gameObject.SetActive(false);
        _uiEndGame.gameObject.SetActive(true);
    }
}

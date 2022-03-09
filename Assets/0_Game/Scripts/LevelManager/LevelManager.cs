using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private GameState _gameState;

    [SerializeField]
    private UiController _uiController;

    public UiController UiController => _uiController;

    [SerializeField]
    private CharacterController _player;

    public CharacterController Player => _player;

    [SerializeField]
    private ObjectSystem _objectSystem;

    public ObjectSystem ObjectSystem => _objectSystem;

    private bool _doneAction;

    private int _score;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.MainLobby);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameState)
        {
            case GameState.MainLobby:
                OnMainLobby();
                break;
            case GameState.InGame:
                OnInGame();
                break;
            case GameState.EndGame:
                OnEndGame();
                break;
        }
    }

    void OnMainLobby()
    {
        if (!_doneAction)
        {
            _score = 0;
            _uiController.OnMainLobby();
            _player.OnMainLobby();
            _objectSystem.OnMainLobby();
            _doneAction = true;
        }
    }

    void OnInGame()
    {
        if (!_doneAction)
        {
            _uiController.OnIngame();
            _player.OnInGame();
            _objectSystem.OnInGame();
            _doneAction = true;
        }
    }

    void OnEndGame()
    {
        if (!_doneAction)
        {
            _uiController.OnEndGame();
            _player.OnEndGame();
            _objectSystem.OnEndGame();
            _doneAction = true;
            Time.timeScale = 0;
        }
    }

    public void OnReMain()
    {
        Time.timeScale = 1;
        ChangeState(GameState.MainLobby);
    }

    public void ChangeState(GameState gameState)
    {
        _doneAction = false;
        _gameState = gameState;
    }

    public void OnPass()
    {
        _score++;
        _uiController.SetScore(_score);
        CheckNewBestScore();
    }

    public void OnLose()
    {
        ChangeState(GameState.EndGame);
    }

    private void CheckNewBestScore()
    {
        int max = PlayerData.GetMaxScore();
        if(_score > max)
        {
            PlayerData.SetMaxScore(_score);
            _uiController.SetBestScore();
        }
    }

    public bool CanMoveObject()
    {
        if (_gameState == GameState.InGame)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public enum GameState
{
    MainLobby,
    InGame,
    EndGame
}

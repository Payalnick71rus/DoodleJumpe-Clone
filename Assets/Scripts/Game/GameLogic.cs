using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class GameLogic : MonoBehaviour
{
    [SerializeField] private GameObject _gameField;
    [SerializeField] private float _scoreCoef;
    #region Injection
    [Inject]
    private EndGameDetector _detector;

    [Inject]
    private MainMenu _menu;

    [Inject]
    private PlayerControl _playerControl;

    [Inject]
    private GameUI _gameUI;

    [Inject]
    private PlatformSpawner _spawner;
    #endregion

    private int _score = 0;

    public float ScreenTop { get; private set; }
    public float ScreenBot { get; private set; }
    public float ScreenLeft { get; private set; }
    public float ScreenRight { get; private set; }
    private void Awake()
    {
        CalcScreenBorder();
    }

    private void CalcScreenBorder()
    {
        Vector2 topLeft = new Vector2(0, Screen.height);
        Vector2 botRight = new Vector2(Screen.width, 0);

        Vector2 realTopLeft = Camera.main.ScreenToWorldPoint(topLeft);
        Vector2 realBotRight = Camera.main.ScreenToWorldPoint(botRight);

        ScreenTop = realTopLeft.y;
        ScreenBot = realBotRight.y;
        ScreenLeft = realTopLeft.x;
        ScreenRight = realBotRight.x;        
    }

    private void Start()
    {
        if(_detector)
        {
            _detector.GameOver += OnEndGame;
        }

        if(_menu)
        {
            _menu.StartGame += StartGame;
            _menu.ExitGame += OnExit;
        }

        if(_playerControl)
        {
            _playerControl.TouchedNewPlatform += OnPlayerTouchedUpperPlatform;
        }
        if(_gameUI)
        {
            _gameUI.GoMenu += OnMenu;
            _gameUI.Pause += OnPause;
            _gameUI.Restart += OnRestart;
            _gameUI.Unpause += OnUnpause;
        }
        
    }
    private void DisplayScore()
    {
        if (_gameUI) _gameUI.SetScore(_score);
    }

    // добавить очки
    private void OnPlayerTouchedUpperPlatform()
    {
        float dist = Vector3.Distance(_playerControl.transform.position, _spawner.StartPanelTransform.position);
        
        int score = (int)(dist * _scoreCoef);
        if (score > _score)
        {
            _score = score;
        }        
        DisplayScore();
    }
    // конец игры
    private void OnEndGame()
    {
        Time.timeScale = 0;
        if(_gameUI) _gameUI.ShowEndPan();
        if (_gameUI) _gameUI.SetEndScore(_score);
    }
    private void InitNewGame()
    {
        _score = 0;
        DisplayScore();
        if (_playerControl) _playerControl.RestorePos();
        if (_spawner) _spawner.StartSpawn();
        Time.timeScale = 1;
    }
    // рестарт
    private void OnRestart()
    {
        if (_gameUI) _gameUI.HideEndPan();
        InitNewGame();
    }
    // старт игры
    private void StartGame()
    {
        if (_menu) _menu.HideMenu();
        if (_gameUI) _gameUI.ShowUI();
        if (_gameField) _gameField.SetActive(true);
        InitNewGame();
    }
    // пауза
    private void OnPause()
    {
        Time.timeScale = 0;        
    }
    // снять паузу
    private void OnUnpause()
    {
        Time.timeScale = 1;
    }
    // выход в меню
    private void OnMenu()
    {
        if(_gameField) _gameField.SetActive(false);
        if (_menu) _menu.ShowMenu();
        if (_gameUI) _gameUI.HideUI();
    }

    private void OnExit()
    {
        Application.Quit();
    }
}

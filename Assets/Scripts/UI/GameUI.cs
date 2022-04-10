using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _endScoreText;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _gameUIPanel;
    [SerializeField] private GameObject _endGamePanel;

    public event EmptyEvent Unpause;
    public event EmptyEvent Pause;
    public event EmptyEvent GoMenu;
    public event EmptyEvent Restart;
    public void SetScore(int score)
    {
        if (_scoreText) _scoreText.text = score.ToString();
    }
    public void SetEndScore(int score)
    {
        if (_endScoreText) _endScoreText.text = score.ToString();
    }
    public void OnPauseBtn()
    {
        Pause?.Invoke();
        if (_pausePanel) _pausePanel.SetActive(true);
    }

    public void OnUnpauseBtn()
    {
        Unpause?.Invoke();
        if (_pausePanel) _pausePanel.SetActive(false);
    }

    public void OnMenuBtn()
    {
        GoMenu?.Invoke();
        if(_pausePanel) _pausePanel.SetActive(false);
        if (_endGamePanel) _endGamePanel.SetActive(false);
    }

    public void ShowUI()
    {
        if (_gameUIPanel)
        {
            _gameUIPanel.SetActive(true);
        }
    }
    public void HideUI()
    {
        if (_gameUIPanel)
        {
            _gameUIPanel.SetActive(false);
        }
    }

    public void OnRestartBtn()
    {
        if (_endGamePanel) _endGamePanel.SetActive(false);
        Restart?.Invoke();
    }

    public void ShowEndPan()
    {
        if (_endGamePanel)
        {
            _endGamePanel.SetActive(true);
        }
    }
    public void HideEndPan()
    {
        if (_endGamePanel)
        {
            _endGamePanel.SetActive(false);
        }
    }
}

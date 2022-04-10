using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuWindow;

    public event EmptyEvent StartGame;
    public event EmptyEvent ExitGame;
    public void ShowMenu()
    {
        if(_menuWindow)
        {
            _menuWindow.SetActive(true);
        }
    }
    public void HideMenu()
    {
        if (_menuWindow)
        {
            _menuWindow.SetActive(false);
        }
    }

    public void OnStartBtn()
    {
        StartGame?.Invoke();
    }

    public void OnExitBtn()
    {
        ExitGame?.Invoke();
    }
}

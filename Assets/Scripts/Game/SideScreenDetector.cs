using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SideScreenDetector : MonoBehaviour
{
    [SerializeField] private bool _isLeft;

    [Inject]
    private GameLogic _logic;
    [Inject]
    private PlayerInput _input;

    private float _moveDir = 0;
    private float _teleportXPosition = 0;
    void Start()
    {
        if(_logic)
        {            
            _teleportXPosition = _isLeft ? _logic.ScreenRight : _logic.ScreenLeft;
        }
        if (_input)
        {
            _input.HorizontalMove += OnHorizontalMove;
        }

    }
    private void OnHorizontalMove(float move)
    {
        _moveDir = move;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl player = null;
        if (collision.gameObject.TryGetComponent(out player))
        {          
            if((_isLeft && _moveDir<0) || (!_isLeft && _moveDir > 0))
            {
                Vector3 pos = player.gameObject.transform.position;
                pos.Set(_teleportXPosition, pos.y, pos.z);
                player.gameObject.transform.position = pos;
            }            
        }
    }
}

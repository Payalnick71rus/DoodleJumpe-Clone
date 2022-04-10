using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerControl : MonoBehaviour
{

    public event FloatEvent PlayerUnderCenterOfScreen; 

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpEnergy = 1f;
    [SerializeField] private float _smallJumpEnergy = 1f;
    [SerializeField] private float _moveLevelY = -2f;
    [SerializeField] private SpriteRenderer _doodleSpriteRend;
    [Inject]
    private PlayerInput _input;

    private Rigidbody2D _rigidBody;
    private Vector2 _jumpForce = Vector2.zero;
    private Vector2 _smallJumpForce = Vector2.zero;
    private GameObject _lastPlatform;
    private Vector3 _startPos;

    public bool CanInterract = true;
    public event EmptyEvent TouchedNewPlatform;
    public event EmptyEvent Jumped;
    private void Awake()
    {
        _jumpForce.Set(0, _jumpEnergy);
        _smallJumpForce.Set(0, _smallJumpEnergy);
        _startPos = transform.position;
    }
    private void Start()
    {
        TryGetComponent(out _rigidBody);
        if(_input)
        {
            _input.HorizontalMove += OnHorizontalMove;
        }

    }
    
    private void OnHorizontalMove(float move)
    {        
        transform.position = new Vector3(transform.position.x+ _moveSpeed * Time.deltaTime* move, transform.position.y, transform.position.z);
        if(_doodleSpriteRend)
        {
            if(move!=0)
            {
                _doodleSpriteRend.flipX = move < 0;
            }            
        }
    }
    private void Jump()
    {
        
        if (!_rigidBody)
        {
            Debug.Log("Error! No rigidBody!");
            return;
        }        
         
        if (transform.position.y > _moveLevelY)
        {
            PlayerUnderCenterOfScreen?.Invoke(-0.15f);
            _rigidBody.AddForce(_smallJumpForce);
        }
        else
        {
            _rigidBody.AddForce(_jumpForce);
        }
        Jumped?.Invoke();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CanInterract) return;
        Platform platform = null;
        
        if (collision.gameObject.TryGetComponent(out platform))
        {
            
            if (platform.Type != PlatformType.Breakable)
            {
                if (_rigidBody.velocity.y < 0.00001f)
                {

                    Jump();
                    if(_lastPlatform== null)
                    {
                        _lastPlatform = collision.gameObject;
                    }
                    else if(_lastPlatform!= collision.gameObject && _lastPlatform.transform.position.y< collision.gameObject.transform.position.y)
                    {                        
                        _lastPlatform = collision.gameObject;
                        TouchedNewPlatform?.Invoke();
                    }

                }
            }            
        }
    }

    public void RestorePos()
    {
        if (_rigidBody) _rigidBody.velocity = Vector2.zero;
        CanInterract = true;
        transform.position = _startPos;
    }
}

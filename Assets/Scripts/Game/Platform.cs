using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Типы платформ: обычная, движущиеся, ловушки
/// </summary>
public enum PlatformType
{
    Default,
    Movable,
    Breakable
}

public class Platform : MonoBehaviour
{
    [SerializeField] private PlatformType _platformType;
    [SerializeField] private string _breakAnimName;
    [SerializeField] private string _defaultAnimName;
    [SerializeField] private float _deleteDelay = 0.3f;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private SoundPlayer _soundPlayer;
    [SerializeField] private float _triggerDist = 0.1f;
    public PlatformType Type { get { return _platformType; } }
    public event GenericEvent<Platform> DeletePlatform;


    private Animator _animator;
    private Rigidbody2D _body;
    private float _deletePos = 0;

    private float _leftSide, _rightSide;
    private bool _canMove = false;
    private float _yPos = 0;

    private void Awake()
    {
        TryGetComponent(out _animator);
        TryGetComponent(out _body);
    }
    public void SetSoundList(SoundsBase list)
    {
        _soundPlayer.SetSoundList(list);
    }
    public void SetScreenParams(float left, float right)
    {
        _leftSide = left;
        _rightSide = right;
    }
    public void SetPhysicsOn(bool isOn)
    {
        if (_body)
        {
            _body.bodyType = isOn ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }
    }
    public void SetDeletePos(float pos) => _deletePos = pos;
    private void Update()
    {
        if(transform.position.y< _deletePos) DeletePlatform?.Invoke(this);
    }
    private void BreakPlatform()
    {
        if (_soundPlayer)
        {
            _soundPlayer.PlaySound(SoudTypes.BreakPlafrom);
        }
        if (_animator)
        {
            _animator.Play(_breakAnimName);
            StartCoroutine(DelayedDelete());
        }
        
    }
    private IEnumerator DelayedDelete()
    {
        yield return new WaitForSeconds(_deleteDelay);
        DeletePlatform?.Invoke(this);
    }
    private void ProcessCollision(GameObject collision)
    {
        PlayerControl player = null;
        if (collision.TryGetComponent(out player))
        {
            if (_platformType == PlatformType.Breakable && collision.transform.position.y>transform.position.y)
            {
                float dist = Mathf.Abs(collision.transform.position.y - transform.position.y);
                if(dist> _triggerDist) BreakPlatform();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProcessCollision(collision.gameObject);
    }

    private void OnEnable()
    {
        if(Type == PlatformType.Movable)
        {
            _yPos = transform.position.y;
            _canMove = true;
            StartMove();
        }
        if(Type == PlatformType.Breakable)
        {
            if (_animator)
            {
                _animator.Play(_defaultAnimName);                
            }
        }
    }
    private void OnDisable()
    {
        _canMove = false;
        StopAllCoroutines();
    }
    private void StartMove()
    {
        if(_canMove)
        {
            StartCoroutine(MoveLoop(_moveSpeed));
        }
    }
    public void SetY(float yPos)
    {
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        _yPos = yPos;
    }
    private IEnumerator MoveLoop(float speed)
    {
        float dir = 1f;
        Vector2 pos = transform.position;
        Vector2 movePos = pos;
        while(_canMove)
        {
            yield return new WaitForEndOfFrame();
            movePos.Set(dir > 0 ? _rightSide : _leftSide, _yPos);
            pos = transform.position;
            pos = Vector2.MoveTowards(pos, movePos, speed * Time.deltaTime);
            transform.position = pos;
            if(Vector2.Distance(pos,movePos)<0.05f)
            {
                dir = dir > 0 ? -1f : 1f;
            }
        }
    }
}


using UnityEngine;
using Zenject;
public class Player : MonoBehaviour
{
    [SerializeField] private SoundPlayer _soundPlayer;
    
    [Inject]
    private PlayerControl _playerControl;

    private void Awake()
    {
        if(_playerControl)
        {
            _playerControl.Jumped += OnJump;
        }
    }

    private void OnJump()
    {
        if(_soundPlayer)
        {
            _soundPlayer.PlaySound(SoudTypes.Jump);
        }
    }
}

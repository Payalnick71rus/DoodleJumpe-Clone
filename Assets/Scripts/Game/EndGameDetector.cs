using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameDetector : MonoBehaviour
{
    [SerializeField] private SoundPlayer _soundPlayer;

    public event EmptyEvent GameOver;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl player = null;
        if (collision.gameObject.TryGetComponent(out player))
        {
            if (_soundPlayer) _soundPlayer.PlaySound(SoudTypes.Fail);
            GameOver?.Invoke();
        }
    }
}

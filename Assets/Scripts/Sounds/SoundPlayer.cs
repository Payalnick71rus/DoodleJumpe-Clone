using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _player;
    [Inject]
    private SoundsBase _soundList;

    public void SetSoundList(SoundsBase list)
    {
        _soundList = list;
    }

    public void PlaySound(SoudTypes type)
    {        
        if (!_soundList || !_player) return;
        AudioClip clip = _soundList.GetClip(type);
        if (clip)
        {
            _player.Stop();
            _player.clip = clip;
            _player.Play();
        }
        else Debug.Log("No clip");
    }
}

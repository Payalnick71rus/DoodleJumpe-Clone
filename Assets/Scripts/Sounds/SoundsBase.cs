using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoudTypes
{
    Jump,
    BreakPlafrom,
    Fail
}

[System.Serializable]
public class GameSound
{
    public AudioClip clip;
    public SoudTypes type;
}
public class SoundsBase : MonoBehaviour
{
    [SerializeField] private List<GameSound> _sounds;


    private Dictionary<SoudTypes, AudioClip> _soundDictionary;

    public void Awake()
    {
        _soundDictionary = new Dictionary<SoudTypes, AudioClip>();
        for(int i=0;i< _sounds.Count;i++)
        {
            if (!_soundDictionary.ContainsKey(_sounds[i].type)) _soundDictionary.Add(_sounds[i].type, _sounds[i].clip);
        }        
    }

    public AudioClip GetClip(SoudTypes type)
    {
        if (_soundDictionary.ContainsKey(type)) return _soundDictionary[type];
        else return null;
    }
}

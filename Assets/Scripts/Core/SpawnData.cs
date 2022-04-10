using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Platforms", menuName ="Doodle Game/Platforms data", order =51)]
public class SpawnData : ScriptableObject
{
    [SerializeField] private List<PlatformSpawnData> _platforms;
    [SerializeField] private MaxMinValue _spawnHeight;
    [SerializeField] private MaxMinValue _spawnWidht;
    [Range(1,99)]
    [SerializeField] private int _spawnDefaultChance;
    public List<PlatformSpawnData> Platforms { get { return _platforms; } }

    public MaxMinValue SpawnHeight { get { return _spawnHeight; } }
    public MaxMinValue SpawnWidth { get { return _spawnWidht; } }
    public int SpawnDefaultPlatformChance { get { return _spawnDefaultChance; } }

    public PlatformSpawnData GetPlatformPrefab(PlatformType type)
    {
        for(int i =0;i< _platforms.Count;i++)
        {
            if(_platforms[i].PlatformPrefab.Type == type)
            {
                return _platforms[i];
            }
        }
        return null;
    }
}
[System.Serializable]
public class PlatformSpawnData
{
    [SerializeField] private Platform _prefab;
    [Range(1, 99)]
    [SerializeField] private int _spawnChance = 50;


    public Platform PlatformPrefab { get { return _prefab; } }
    public int SpawnChance { get { return _spawnChance; } }
}


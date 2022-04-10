using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private SpawnData _spawnData;
    [SerializeField] private Transform _container;
    [SerializeField] private float _spawnHeigt =3f;
    [SerializeField] private float _borderOffset = 0.6f;
    [SerializeField] private Transform _startPlatform;
    [SerializeField] private float _flyOffset = 0.3f;
    [SerializeField] private float _moveSpeed =5f;
    [SerializeField] private float _deleteOffset = 5f;
    private bool _canSpawn;

    [Inject]
    private GameLogic _logic;

    [Inject]
    private PlayerControl _playerControl;
    [Inject]
    private SoundsBase _soundList;

    private float _stopSpawnY = 0;
    private List<Platform> _spawnedPlatforms = new List<Platform>();
    private bool _isMoving = false;
    private bool _spawnedTrap = false;
    private Vector3 _startPos;
    private PoolTemplate<Platform> _defaultPlatformPool, _movingPlatformPool, _breakablePlatformPool;

    public Transform StartPanelTransform { get { return _startPlatform; } }

    private void Awake()
    {
        PlatformSpawnData defaultPlatform, movingPlatform, breakablePlatform;
        defaultPlatform = _spawnData.GetPlatformPrefab(PlatformType.Default);
        movingPlatform = _spawnData.GetPlatformPrefab(PlatformType.Movable);
        breakablePlatform = _spawnData.GetPlatformPrefab(PlatformType.Breakable);

        _defaultPlatformPool = new PoolTemplate<Platform>(10, defaultPlatform.PlatformPrefab, _container);
        _defaultPlatformPool.InitPool();

        _movingPlatformPool = new PoolTemplate<Platform>(5, movingPlatform.PlatformPrefab, _container);
        _movingPlatformPool.InitPool();

        _breakablePlatformPool = new PoolTemplate<Platform>(5, breakablePlatform.PlatformPrefab, _container);
        _breakablePlatformPool.InitPool();
    }

    private void Start()
    {
        _startPos = _startPlatform.position;
        if (_logic)
        {
            _stopSpawnY = _logic.ScreenTop + _spawnHeigt;
        }

        if(_playerControl)
        {
            _playerControl.PlayerUnderCenterOfScreen += OnPlayerUnderCenter;
        }
        _spawnedPlatforms.Add(_startPlatform.GetComponent<Platform>());    
        
    }
    private void OnPlayerUnderCenter(float dy)
    {
        if (_isMoving) return;
        _isMoving = true;
        StartCoroutine(MovePlatforms(-_flyOffset));
    }
   
    private IEnumerator MovePlatforms(float dy)
    {        
        float pos = 0;
        Transform plt;
       
        _playerControl.CanInterract = false;
        float stop = _spawnedPlatforms[0].gameObject.transform.position.y- Mathf.Abs(dy);
        while (_spawnedPlatforms[0].gameObject.transform.position.y> stop)
        {
            yield return new WaitForEndOfFrame();
            pos = Time.deltaTime * _moveSpeed*(0.1f+ Mathf.Abs(_spawnedPlatforms[0].gameObject.transform.position.y - stop));
            for(int i=0;i< _spawnedPlatforms.Count;i++)
            {
                plt = _spawnedPlatforms[i].gameObject.transform;
                
                if(_spawnedPlatforms[i].Type == PlatformType.Movable)
                {
                    _spawnedPlatforms[i].SetY(plt.position.y - pos);
                }
                else
                {
                    plt.position = new Vector3(plt.position.x, plt.position.y - pos, plt.position.z);
                }
            }
            if(_spawnedPlatforms[_spawnedPlatforms.Count-1].gameObject.transform.position.y<(_logic.ScreenTop+ _spawnData.SpawnHeight.Max))
            {
                GenerateLevel();
            }
            
        }
        _playerControl.CanInterract = true;
        _isMoving = false;
    }
    private void OnDeletePlatform(Platform platform)
    {
        platform.DeletePlatform -= OnDeletePlatform;
        if(_spawnedPlatforms.Contains(platform)) _spawnedPlatforms.Remove(platform);
        platform.gameObject.SetActive(false);
    }
    private void SpawnPlatform( Vector2 pos, PlatformType type)
    {

        PoolTemplate<Platform> _pool = null;
        switch(type)
        {
            case PlatformType.Breakable:
                _pool = _breakablePlatformPool;
                break;
            case PlatformType.Default:
                _pool = _defaultPlatformPool;
                break;
            case PlatformType.Movable:
                _pool = _movingPlatformPool;
                break;
        }
        
        Platform plt = _pool.GetObject();
        plt.DeletePlatform += OnDeletePlatform;
        plt.SetDeletePos(_logic.ScreenBot - _deleteOffset);
        plt.SetScreenParams(_logic.ScreenLeft+ _borderOffset, _logic.ScreenRight- _borderOffset);
        _spawnedPlatforms.Add(plt);
        plt.gameObject.transform.position = pos;
        plt.gameObject.SetActive(true);
        plt.SetSoundList(_soundList);
    }
    
    private void GenerateLevel()
    {
        
        if (!_spawnData || !_logic || !_canSpawn) return;
        float startY = _spawnedPlatforms[_spawnedPlatforms.Count-1].gameObject.transform.position.y;
        if (_logic)
        {
            _stopSpawnY = _logic.ScreenTop + _spawnHeigt;
        }
        float spawnY = startY;
        float spawnX = _spawnedPlatforms[_spawnedPlatforms.Count - 1].gameObject.transform.position.x;        

        Vector2 pos = Vector2.zero;

        PlatformSpawnData movingPlatform;
        
        movingPlatform = _spawnData.GetPlatformPrefab(PlatformType.Movable);
        
        
        while (spawnY < _stopSpawnY)
        {
            if (!_spawnedTrap)
            {
                spawnY = spawnY + UnityEngine.Random.Range(_spawnData.SpawnHeight.Min, _spawnData.SpawnHeight.Max);
            }
                
            int spawnLeft = UnityEngine.Random.Range(1, 100);
            float dx = UnityEngine.Random.Range(_spawnData.SpawnWidth.Min, _spawnData.SpawnWidth.Max);
            if(spawnLeft>50)
            {
                dx = -dx;
            }
            if (!_spawnedTrap)
            {
                spawnX = _spawnedPlatforms[_spawnedPlatforms.Count - 1].gameObject.transform.position.x + dx;
            }
            else
            {
                spawnX = _spawnedPlatforms[_spawnedPlatforms.Count - 1].gameObject.transform.position.x + dx
                                            + UnityEngine.Random.Range(1, 100)>50?-_borderOffset*2f: _borderOffset * 2f;
            }


            
            if (spawnX < (_logic.ScreenLeft + _borderOffset))
            {
                spawnX = _logic.ScreenRight - _borderOffset;
            }

            if (spawnX > (_logic.ScreenRight - _borderOffset))
            {
                spawnX = _logic.ScreenLeft + _borderOffset;
            }

            int spawnDefault = UnityEngine.Random.Range(1, 100);
            pos.Set(spawnX, spawnY);
            if (spawnDefault< _spawnData.SpawnDefaultPlatformChance)
            {
                SpawnPlatform(pos, PlatformType.Default);
                _spawnedTrap = false;
            }
            else
            {
                int spawnMoving = UnityEngine.Random.Range(1, 100);
                if(_spawnedTrap)
                {
                    SpawnPlatform(pos, PlatformType.Movable);
                    _spawnedTrap = false;
                }
                else
                {
                    if (spawnMoving < movingPlatform.SpawnChance)
                    {
                        SpawnPlatform(pos, PlatformType.Movable);
                        _spawnedTrap = false;
                    }
                    else
                    {
                        SpawnPlatform(pos, PlatformType.Breakable);
                        _spawnedTrap = true;
                    }
                }
                
            }
        }
    }
    private void DeleteLevel()
    {
        Platform startPlt = _startPlatform.gameObject.GetComponent<Platform>();
        for (int i = 0; i < _spawnedPlatforms.Count; i++)
        {
            if(startPlt != _spawnedPlatforms[i])
            {
                _spawnedPlatforms[i].gameObject.SetActive(false);
            }
        }
        _spawnedPlatforms.Clear();
    }
    private void StopSpawn()
    {
        _canSpawn = false;
    }
    public void StartSpawn()
    {
        if(_spawnedPlatforms.Count>0) DeleteLevel();
        _canSpawn = true;
        _spawnedTrap = false;
        _stopSpawnY = 0;
        _spawnedPlatforms.Add(_startPlatform.GetComponent<Platform>());
        if(_startPlatform)
        {
            _startPlatform.position = _startPos;
        }
        GenerateLevel();
    }
}

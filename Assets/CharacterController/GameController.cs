using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _baseItemSpawnPosition;
    [SerializeField] private GameObject _horseBasePrefab;
    [SerializeField] private GameObject _carBasePrefab;
    [SerializeField] private GameObject _gunBasePrefab;
    [SerializeField] private AudioSource _fanfareAudioSource;
    [SerializeField] private AudioSource _loseAudioSource;
    
    [Space]
    [Header("Spawnable items and their positions")]
    [SerializeField] private HorseCollectible[] _horsePartsPrefabs;
    [SerializeField] private CarCollectible[] _carPartsPrefabs;
    [SerializeField] private GunCollectible[] _gunPartsPrefabs;
    [SerializeField] private PartSpawnPosition[] _partsSpawnPositions;

    [Space] [Header("Positions for Game Over")] 
    [SerializeField] private Transform _gameOverCameraPosition;
    [SerializeField] private Transform _gameOverPlayerPosition;

    [Space]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _hintText;
    [SerializeField] private Text _startCountdownText;
    [SerializeField] private Text _pointsText;
    [SerializeField] private Text _yourPointsText;
    [SerializeField] private Text _gameOverPointsText;
    [SerializeField] private float _timeUntilGameOver;

    private Transform _toyTransform;
    private GameType _type;
    private bool _spinToy;
    private bool _gameStarted;
    private float _toyRotationSpeed = 15.0f;

    public GameType Type => _type;

    private void Awake()
    {
        var typeNumber = Random.Range(1, 4);
        GameObject prefab;

        switch (typeNumber)
        {
            case 1:
                _type = GameType.Horse;
                prefab = _horseBasePrefab;
                SpawnHorseParts();
                break;
            case 2:
                _type = GameType.Car;
                prefab = _carBasePrefab;
                SpawnCarParts();
                break;
            case 3:
                _type = GameType.Gun;
                prefab = _gunBasePrefab;
                SpawnGunParts();
                break;
            default:
                prefab = null;
                break;
        }

        _toyTransform = Instantiate(prefab, _baseItemSpawnPosition).transform;

        _partsSpawnPositions = FindObjectsOfType<PartSpawnPosition>();
        
        _timerText.text = $"{_timeUntilGameOver:F2}";
        _gameOverText.text = "";
        _hintText.text = "";
        _yourPointsText.gameObject.SetActive(false);
        _gameOverPointsText.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
        
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (_spinToy)
        {
            _toyTransform.Rotate(_toyTransform.up, _toyRotationSpeed * Time.deltaTime);
        }
        else if (_gameStarted)
        {
            if (_timeUntilGameOver > 0)
            {
                _timeUntilGameOver -= Time.deltaTime;
                _timerText.text = $"{_timeUntilGameOver:F2}";
            }
            else
            {
                _timeUntilGameOver = 0;
                _timerText.text = $"{_timeUntilGameOver:F2}";
                TimeRanOut();
            }
        }
    }

    private IEnumerator StartGame()
    {
        _startCountdownText.text = "3";
        yield return new WaitForSeconds(1.0f);
        _startCountdownText.text = "2";
        yield return new WaitForSeconds(1.0f);
        _startCountdownText.text = "1";
        yield return new WaitForSeconds(1.0f);
        _startCountdownText.text = "Start!";
        _hintText.text = $"Build the {_type}!";
        _playerController.Activate();
        _gameStarted = true;
        yield return new WaitForSeconds(1.0f);
        _startCountdownText.text = "";
        
        yield return new WaitForSeconds(5.0f);
        _hintText.text = "";
    }

    private void SpawnHorseParts()
    {
        foreach (var partsSpawnPosition in _partsSpawnPositions)
        {
            List<HorseCollectible> prefabs = new List<HorseCollectible>();
            foreach (var horsePartsPrefab in _horsePartsPrefabs)
            {
                if (horsePartsPrefab.RoomType == partsSpawnPosition.RoomType)
                {
                    prefabs.Add(horsePartsPrefab);
                }
            }

            if (prefabs.Count != 0)
            {
                var randomPart = prefabs[Random.Range(0, prefabs.Count)];
                Instantiate(randomPart, partsSpawnPosition.transform.position, partsSpawnPosition.transform.rotation);
                partsSpawnPosition.IsTaken = true;
            }
        }
    }
    
    private void SpawnCarParts()
    {
        foreach (var partsSpawnPosition in _partsSpawnPositions)
        {
            List<CarCollectible> prefabs = new List<CarCollectible>();
            foreach (var carPartPrefab in _carPartsPrefabs)
            {
                if (carPartPrefab.RoomType == partsSpawnPosition.RoomType)
                {
                    prefabs.Add(carPartPrefab);
                }
            }

            if (prefabs.Count != 0)
            {
                var randomPart = prefabs[Random.Range(0, prefabs.Count)];
                Instantiate(randomPart, partsSpawnPosition.transform.position, partsSpawnPosition.transform.rotation);
                partsSpawnPosition.IsTaken = true;
            }
        }
    }

    private void SpawnGunParts()
    {
        foreach (var partsSpawnPosition in _partsSpawnPositions)
        {
            List<GunCollectible> prefabs = new List<GunCollectible>();
            foreach (var gunPartPrefab in _gunPartsPrefabs)
            {
                if (gunPartPrefab.RoomType == partsSpawnPosition.RoomType)
                {
                    prefabs.Add(gunPartPrefab);
                }
            }

            if (prefabs.Count != 0)
            {
                var randomPart = prefabs[Random.Range(0, prefabs.Count)];
                Instantiate(randomPart, partsSpawnPosition.transform.position, partsSpawnPosition.transform.rotation);
                partsSpawnPosition.IsTaken = true;
            }
        }
    }

    private void TimeRanOut()
    {
        _playerController.Deactivate();
        _gameOverText.text = "Time out!";
        MoveCameraAndPlayer();
        _loseAudioSource.Play();
    }

    public void CollectedAllParts()
    {
        _playerController.Victory();
        _gameOverText.text = "Victory!";
        _yourPointsText.gameObject.SetActive(true);
        _gameOverPointsText.gameObject.SetActive(true);
        MoveCameraAndPlayer();
        _fanfareAudioSource.Play();
    }

    private void MoveCameraAndPlayer()
    {
        var playerTransform = _playerController.transform;
        playerTransform.position = _gameOverPlayerPosition.position;
        playerTransform.rotation = _gameOverPlayerPosition.rotation;
        _playerController.RotatingModel.transform.localRotation = Quaternion.identity;

        var cameraTransform = _camera.transform;
        cameraTransform.position = _gameOverCameraPosition.position;
        cameraTransform.rotation = _gameOverCameraPosition.rotation;

        _timerText.text = "";
        _pointsText.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(true);
        _exitButton.gameObject.SetActive(true);

        _spinToy = true;
    }

    public void UpdatePoints(int points)
    {
        _pointsText.text = points.ToString();
        _gameOverPointsText.text = points.ToString();
    }

    public void Exit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}

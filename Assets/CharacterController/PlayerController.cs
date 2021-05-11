using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _positionForCollectedItems;
    [SerializeField] private GameObject _rotatingModel;
    [SerializeField] private GameObject _pressSpaceHint;
    [SerializeField] private Animator _animator;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _map;
    [SerializeField] private Text _mapHintText;
    [SerializeField] private Text _pointsText;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _cameraLerpFactor = 15.0f;

    private HorseToy _horseToy;
    private CarToy _carToy;
    private GunToy _gunToy;

    private Collectible _pickedCollectible;
    private Collectible _collectibleNearPlayer;
    private Vector3 _cameraOffset;
    private bool _isInCollectibleTrigger;
    private bool _isActivated;
    private bool _isInToyTrigger;
    private float _yPosition;
    private int _points;
    private GameType _type;

    private static readonly int AnimatorSpeedParameter = Animator.StringToHash("Speed");
    private GameController _gameController;

    private const string HORIZONTAL_AXIS_NAME = "Horizontal";
    private const string VERTICAL_AXIS_NAME = "Vertical";
    private const KeyCode INTERACTION_KEY_CODE = KeyCode.Space;
    private const float ITEM_THROW_FORCE = 50.0f;

    #endregion

    public GameObject RotatingModel => _rotatingModel;

    public bool IsActivated => _isActivated;

    public int Points => _points;


    #region UnityMethods

    private void Start()
    {
        _cameraOffset = _camera.transform.position - transform.position;
        _pressSpaceHint.SetActive(false);
        _yPosition = transform.position.y + 0.0001f;

        _gameController = FindObjectOfType<GameController>();
        _type = _gameController.Type;

        switch (_type)
        {
            case GameType.Horse:
                _horseToy = FindObjectOfType<HorseToy>();
                break;
            case GameType.Car:
                _carToy = FindObjectOfType<CarToy>();
                break;
            case GameType.Gun:
                _gunToy = FindObjectOfType<GunToy>();
                break;
        }
        
        _map.SetActive(false);
        _mapHintText.text = "Press TAB to open map";
        _gameController.UpdatePoints(_points);
    }

    void Update()
    {
        if (!_isActivated) return;
        
        MovePlayer();
        MoveCamera();
        TryToPickCollectible();
        SwitchMap();
    }

    #endregion


    #region Methods

    /// <summary>
    /// Moves and rotates the player.
    /// </summary>
    private void MovePlayer()
    {
        var horizontal = Input.GetAxisRaw(HORIZONTAL_AXIS_NAME);
        var vertical = Input.GetAxisRaw(VERTICAL_AXIS_NAME);

        var move = new Vector3();
        move.z = -horizontal;
        move.x = vertical;

        if (move.sqrMagnitude != 0.0f)
        {
            _characterController.Move(move.normalized * (_speed * Time.deltaTime));

            var rotation = Quaternion.LookRotation(move);
            _rotatingModel.transform.rotation = Quaternion.Lerp(_rotatingModel.transform.rotation, rotation,
                _rotationSpeed * Time.deltaTime);
        }

        _animator.SetFloat(AnimatorSpeedParameter, move.sqrMagnitude);

        var newPosition = transform.position;
        newPosition.y = _yPosition;
        transform.position = newPosition;
    }

    /// <summary>
    /// Moves the camera with the player.
    /// </summary>
    private void MoveCamera()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, transform.position + _cameraOffset,
            _cameraLerpFactor * Time.deltaTime);
    }

    /// <summary>
    /// Picks up a collectible if there is a collectible next to the player and if the necessary button is pressed.
    /// Drops the item if the key is pressed and there's a picked up item.
    /// </summary>
    private void TryToPickCollectible()
    {
        if (Input.GetKeyDown(INTERACTION_KEY_CODE))
        {
            if (_pickedCollectible)
            {
                if (_isInToyTrigger)
                {
                    TryToInstallPart();
                }
                else
                {
                    ThrowCollectible();
                }
            }
            else
            {
                if (_collectibleNearPlayer)
                {
                    if (!_collectibleNearPlayer.IsInstalled)
                    {
                        PickCollectible();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Equips a collectible.
    /// </summary>
    private void PickCollectible()
    {
        var collectibleTransform = _collectibleNearPlayer.transform;
        _pickedCollectible = _collectibleNearPlayer;
        collectibleTransform.parent = _positionForCollectedItems;
        collectibleTransform.localPosition = Vector3.zero;
        _collectibleNearPlayer.Rigidbody.isKinematic = true;
        _collectibleNearPlayer.ParticleSystem.Stop();
        _collectibleNearPlayer.IsPickedUp = true;
        _pressSpaceHint.SetActive(false);
        _animator.SetBool("Transfer", true);
    }

    /// <summary>
    /// Drops the equipped collectible.
    /// </summary>
    private void ThrowCollectible()
    {
        _pickedCollectible.transform.parent = null;
        _pickedCollectible.IsPickedUp = false;

        _pickedCollectible.Rigidbody.isKinematic = false;
        _pickedCollectible.Rigidbody.AddForce(_characterController.velocity * ITEM_THROW_FORCE);
        _pickedCollectible.ParticleSystem.Play();
        
        _pickedCollectible = null;
        _animator.SetBool("Transfer", false);
    }

    /// <summary>
    /// Used to install a part on the toy.
    /// </summary>
    private void TryToInstallPart()
    {
        if (Input.GetKeyDown(INTERACTION_KEY_CODE))
        {
            if (_pickedCollectible)
            {
                bool installationSuccessful = false;
                switch (_type)
                {
                    case GameType.Horse:
                        installationSuccessful = _horseToy.InstallPart(_pickedCollectible);
                        break;
                    case GameType.Car:
                        installationSuccessful = _carToy.InstallPart(_pickedCollectible);
                        break;
                    case GameType.Gun:
                        installationSuccessful = _gunToy.InstallPart(_pickedCollectible);
                        break;
                }

                if (installationSuccessful)
                {
                    switch (_pickedCollectible.RoomType)
                    {
                        case ItemRoomTypes.Food:
                            _points += 10;
                            break;
                        case ItemRoomTypes.Toys:
                            _points += 20;
                            break;
                        case ItemRoomTypes.Treasures:
                            _points += 20;
                            break;
                        case ItemRoomTypes.Workshop:
                            _points += 40;
                            break;
                        case ItemRoomTypes.BrokenCarousel:
                            _points += 20;
                            break;
                        case ItemRoomTypes.LivingRoom:
                            _points += 10;
                            break;
                        case ItemRoomTypes.MusicCastle:
                            _points += 30;
                            break;
                        case ItemRoomTypes.SweetsPark:
                            _points += 40;
                            break;
                        case ItemRoomTypes.ToolsAndMaterials:
                            _points += 20;
                            break;
                    }
                    _gameController.UpdatePoints(_points);
                    
                    _pickedCollectible.IsPickedUp = false;
                    _pickedCollectible.IsInstalled = true;
                    _pickedCollectible = null;
                    _pressSpaceHint.SetActive(false);
                    _animator.SetBool("Transfer", false);
                }
            }
        }
    }

    /// <summary>
    /// Used to define which collectible player is now available to pick up.
    /// </summary>
    /// <param name="collectible">The collectible that has the trigger that the player has entered.</param>
    public void EnteredCollectibleTrigger(Collectible collectible)
    {
        if (!_isActivated) return;
        
        _isInCollectibleTrigger = true;
        _collectibleNearPlayer = collectible;
        if (!collectible.IsPickedUp)
        {
            if (!collectible.IsInstalled)
            {
                if (!_pickedCollectible)
                {
                    _pressSpaceHint.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Used to define which collectible player is no longer available to pick up.
    /// </summary>
    /// <param name="collectible">The collectible that has the trigger that the player has exited.</param>
    public void ExitedCollectibleTrigger(Collectible collectible)
    {
        if (!_isActivated) return;
        
        _isInCollectibleTrigger = false;
        _collectibleNearPlayer = null;
        if (!collectible.IsPickedUp)
        {
            if (!collectible.IsInstalled)
            {
                _pressSpaceHint.SetActive(false);
            }
        }
    }

    public void SwitchMap()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_map.activeSelf)
            {
                _map.SetActive(false);
                _mapHintText.text = "Press TAB to open map";
            }
            else
            {
                _map.SetActive(true);
                _mapHintText.text = "Press TAB to close map";
            }
        }
    }

    /// <summary>
    /// Used to define the moment when the player enters the toy base's trigger.
    /// </summary>
    public void EnteredToyTrigger()
    {
        if (!_isActivated) return;
        
        _isInToyTrigger = true;
        if (_isInCollectibleTrigger)
        {
            _pressSpaceHint.SetActive(true);
        }
    }

    /// <summary>
    /// Used to define the moment when the player exits the toy base's trigger.
    /// </summary>
    public void ExitedToyTrigger()
    {
        if (!_isActivated) return;
        
        _isInToyTrigger = false;
        _pressSpaceHint.SetActive(false);
    }
    
    /// <summary>
    /// Used to deactivate the player on game over.
    /// </summary>
    public void Deactivate()
    {
        _isActivated = false;
        _animator.SetFloat(AnimatorSpeedParameter, 0.0f);
        _map.SetActive(false);
        _mapHintText.text = "";
    }

    public void Victory()
    {
        _isActivated = false;
        _animator.SetFloat(AnimatorSpeedParameter, 0.0f);
        int danceNum = Random.Range(0, 4);

        switch (danceNum)
        {
            case 0:
                _animator.SetBool("BreakDance", true);
                break;
            case 1:
                _animator.SetBool("ShuffleDance", true);
                break;
            case 2:
                _animator.SetBool("HipHopDance", true);
                break;
            case 3:
                _animator.SetBool("RumbaDance", true);
                break;
        }
        
        _map.SetActive(false);
        _mapHintText.text = "";
    }

    /// <summary>
    /// Used to activate the player after the start countdown.
    /// </summary>
    public void Activate()
    {
        _isActivated = true;
    }

    #endregion
}
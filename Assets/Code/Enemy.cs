using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


namespace JevLogin
{
    internal sealed class Enemy : MonoBehaviour
    {
        #region Fields

        [Header("Количество точек маршрутизации.")]
        [SerializeField] private Vector3[] _waypoints;
        [SerializeField] private Vector4 _sizeOfPlatform;

        [Header("Ссылка на префаб точки спавна"), Space(10)]
        [SerializeField] private GameObject _prefab;

        [Header("Показать точки маршрута и спавна для Эльфа?"), Space(10)]
        [SerializeField] private bool _isEnabledViewSpawnPoint = false;

        [Header("Показать DEBUG LOG?"), Space(10)]
        [SerializeField] private bool _isDebugModeEnabled = false;

        [SerializeField, Space(10)] private Animator _animator;

        private float _workTimer = WORK_TIME;
        private float _idleTimer = IDLE_TIME;
        private int _currentWaypointIndex = 0;
        private ElfState _state;
        private static readonly int SetToRun = Animator.StringToHash("SetToRun");
        private static readonly int SetToIdle = Animator.StringToHash("SetToIdle");
        private static readonly int SetToWork = Animator.StringToHash("SetToWork");

        private const float WORK_TIME = 5.0f;
        private const float IDLE_TIME = 3.0f;

        private NavMeshAgent _navMeshAgent;
        private Transform _transformGround;
        private float _squareOfSpeed;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            if (_transformGround == null)
            {
                _transformGround = GameObject.FindGameObjectWithTag("Ground").transform;
            }
            GenerateVector4ToGameObject();
        }

        private void Start()
        {
            if (_navMeshAgent == null)
            {
                _navMeshAgent = GetComponent<NavMeshAgent>();
                _squareOfSpeed = _navMeshAgent.stoppingDistance * _navMeshAgent.stoppingDistance;
            }
            GenerateWaypoints();

            SwitchToRun();
            GenerateWaypoints();


            _navMeshAgent.SetDestination(_waypoints[0]);
        }

        private void Update()
        {
            ExecuteStates();
        }

        #endregion


        #region Methods

        private void ExecuteStates()
        {
            switch (_state)
            {
                case ElfState.Idle:
                    Idle();
                    break;
                case ElfState.Run:
                    Run();
                    break;
                case ElfState.Work:
                    Work();
                    break;
            }
        }
        private void Idle()
        {
            if (_idleTimer > 0)
            {
                _idleTimer -= Time.deltaTime;
            }
            else
            {
                SwitchToRun();
                _idleTimer = IDLE_TIME;
            }
        }

        private void Run()
        {
            if ((_navMeshAgent.destination - transform.position).sqrMagnitude <= _squareOfSpeed)
            {
                if (Random.Range(0, 2) == 0)
                {
                    SwitchToWork();
                }
                else
                {
                    SwitchToIdle();
                }
            }
        }

        private void Work()
        {
            if (_workTimer > 0)
            {
                _workTimer -= Time.deltaTime;
            }
            else
            {
                SwitchToRun();
                _workTimer = WORK_TIME;
            }
        }
        private void SwitchToIdle()
        {
            _state = ElfState.Idle;
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetTrigger(SetToIdle);
            if (_isDebugModeEnabled)
            {
                Debug.Log("SetToIdle");
            }
        }

        private void SwitchToRun()
        {
            _state = ElfState.Run;

            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex]);

            _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex]);
            _animator.SetTrigger(SetToRun);
            if (_isDebugModeEnabled)
            {
                Debug.Log("SetToRun");
            }
        }

        private void SwitchToWork()
        {
            _state = ElfState.Work;
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetTrigger(SetToWork);
            if (_isDebugModeEnabled)
            {
                Debug.Log("SetToWork");
            }
        }


        private void GenerateWaypoints()
        {
            if (_waypoints == null || _waypoints.Length == 0)
            {
                _waypoints = new[] { GeneratePoint() };
            }

            Array.Resize(ref _waypoints, _waypoints.Length + 1);
            _waypoints[_waypoints.Length - 1] = GeneratePoint();
        }

        private void GenerateVector4ToGameObject()
        {
            var bounds = _transformGround.GetComponent<MeshFilter>().sharedMesh.bounds;
            _sizeOfPlatform.x = bounds.center.x;
            _sizeOfPlatform.y = bounds.center.z;
            _sizeOfPlatform.z = bounds.size.x;
            _sizeOfPlatform.w = bounds.size.z;
        }

        public Vector3 GeneratePoint()
        {
            Vector3 result = Vector3.one;
            for (int i = 0; i < 100; i++)
            {
                var x = Random.Range(_sizeOfPlatform.x - (_sizeOfPlatform.z / 2), _sizeOfPlatform.x + (_sizeOfPlatform.z / 2));
                var y = 0.5f;
                var z = Random.Range(_sizeOfPlatform.y - (_sizeOfPlatform.w / 2), _sizeOfPlatform.y + (_sizeOfPlatform.w / 2));
                var checkPoint = new Vector3(x, y, z);
                var _ = new Collider[2];
                int numColliders = Physics.OverlapSphereNonAlloc(checkPoint, 2.0f, _);
                if (numColliders == 1)
                {
                    if (_isEnabledViewSpawnPoint)
                    {
                        var point = Instantiate(_prefab, checkPoint, Quaternion.identity);
                        point.name = ManagerName.SPAWNPOINT;
                        point.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
                    }
                    return checkPoint;
                }
            }

            return result;
        }

        #endregion


    }
}
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class ElfWandererController : MonoBehaviour
{
    [SerializeField] private GameObject[] _waypoints;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;

    private float _workTimer = WORK_TIME;
    private float _idleTimer = IDLE_TIME;
    private int _currentWaypointIndex = 0;
    private ElfState _state;
    private static readonly int SetToRun = Animator.StringToHash("SetToRun");
    private static readonly int SetToIdle = Animator.StringToHash("SetToIdle");
    private static readonly int SetToWork = Animator.StringToHash("SetToWork");

    private const float WORK_TIME = 5.0f;
    private const float IDLE_TIME = 3.0f;
    private const float STOP_THRESHOLD = 0.1f;

    private void Start()
    {
        SwitchToRun();
    }

    private void Update()
    {
        ExecuteStates();
    }

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
        if (_navMeshAgent.remainingDistance <= STOP_THRESHOLD)
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
        Debug.Log("SetToIdle");
    }

    private void SwitchToRun()
    {
        _state = ElfState.Run;
        _currentWaypointIndex++;
        if (_currentWaypointIndex == _waypoints.Length)
        {
            _currentWaypointIndex = 0;
        }

        _navMeshAgent.SetDestination(_waypoints[_currentWaypointIndex].transform.position);
        _animator.SetTrigger(SetToRun);
        Debug.Log("SetToRun");
    }

    private void SwitchToWork()
    {
        _state = ElfState.Work;
        _navMeshAgent.SetDestination(transform.position);
        _animator.SetTrigger(SetToWork);
        Debug.Log("SetToWork");
    }
}
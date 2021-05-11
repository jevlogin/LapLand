using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace JevLogin
{
    internal sealed class TrainView : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField, Header("Дорога ж/д"), Space(5)] private Transform _transformRoadPath;
        [SerializeField, Header("Маршрут по дороге ж/д"), Space(5)] private List<Vector3> _pathRoads = new List<Vector3>();
        private float _squareOfSpeed;
        private int _currentWaypointIndex = 0;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            

        }

        private void Start()
        {
            var transforms = _transformRoadPath.GetComponentsInChildren<Transform>();
            for (int i = 1; i < transforms.Length; i++)
            {
                Transform transform = transforms[i];
                _pathRoads.Add(transform.position);
            }

            _squareOfSpeed = _navMeshAgent.stoppingDistance * _navMeshAgent.stoppingDistance;
            _navMeshAgent.SetDestination(_pathRoads[_currentWaypointIndex]);
        }


        private void Update()
        {
            if ((_navMeshAgent.destination - transform.position).sqrMagnitude <= _squareOfSpeed)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _pathRoads.Count;
                _navMeshAgent.SetDestination(_pathRoads[_currentWaypointIndex]);
            }
        }
    }
}
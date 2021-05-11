using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JevLogin
{
    internal sealed class GeneratorElf : MonoBehaviour
    {
        private Reference _reference;

        private const int _min = 0;
        [SerializeField] private const int _max = 1000;

        [Header("Paint elfs?"), SerializeField]
        private bool _isTouchUpYourEnemy;

        [Space(10)]
        [Range(3, _max)]
        [Header("Created elfs count:")]
        [ContextMenuItem("Random Count", nameof(RandomCount))]
        public int CountEnemy = 3;

        private void RandomCount()
        {
            CountEnemy = UnityEngine.Random.Range(_min, _max);
        }

        private void Awake()
        {
            _reference = new Reference();

            for (int i = 0; i < CountEnemy; i++)
            {
                var tp = _reference.Enemy.GeneratePoint();
                var ghost = Instantiate(_reference.Enemy, tp, Quaternion.identity);
                if (_isTouchUpYourEnemy)
                {
                    ghost.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
                }
            }
        }
    }
}
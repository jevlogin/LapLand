using System.IO;
using UnityEngine;


namespace JevLogin
{
    internal sealed class Reference
    {
        private Enemy _enemy;

        internal Enemy Enemy
        {
            get
            {
                if (_enemy == null)
                {
                    var enemy = Resources.Load<Enemy>(Path.Combine(ManagerName.PREFABS, ManagerName.ENEMY));
                    _enemy = Object.Instantiate(enemy);
                }
                return _enemy;
            }
        }
    }
}
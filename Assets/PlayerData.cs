using UnityEngine;

namespace JevLogin
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 51)]
    internal class PlayerData : ScriptableObject
    {
        public PlayerSettingsData PlayerSettingsData;
    }
}
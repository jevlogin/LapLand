using System.Collections.Generic;
using UnityEngine;


namespace JevLogin
{
    [System.Serializable]
    public sealed class PlayerSettingsData
    {
        public List<string> ResolutionsList;
        public Resolution[] Resolutions;
        public int CurrentResolution;
        public bool IsFullScreenMode;
    }
}
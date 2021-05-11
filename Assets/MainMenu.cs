using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JevLogin
{
    internal class MainMenu : MonoBehaviour
    {
        #region Fields

        public AudioMixer AudioMixer;

        [SerializeField] protected AudioSource _audioSourceMainMenu;
        [SerializeField] protected TMP_Dropdown _dropdownScreenResolution;
        [SerializeField] protected TMP_Dropdown _dropdownQualitySettings;
        [SerializeField] private List<string> _resolutionsList;
        [SerializeField] protected Resolution[] _resolutions;
        [SerializeField] protected bool _isFullScreen;
        [SerializeField] protected PlayerData _playerData;
        [SerializeField] protected Toggle _isFullScreenToggle;


        #endregion

        private void Awake()
        {
            if (_playerData == null)
            {
                _playerData = Resources.Load<PlayerData>("PlayerData"); 
            }
            ResetResolution();
            _dropdownQualitySettings.value = QualitySettings.GetQualityLevel();
        }

        private void Start()
        {
            _isFullScreen = Screen.fullScreen;
            _playerData.PlayerSettingsData.IsFullScreenMode = _isFullScreen;
        }

        protected void ResetResolution()
        {
            _resolutionsList = new List<string>();
            _resolutions = Screen.resolutions;

            _playerData.PlayerSettingsData.IsFullScreenMode = _isFullScreen;

            if (_playerData.PlayerSettingsData.Resolutions == null)
            {
                _playerData.PlayerSettingsData.Resolutions = _resolutions;
            }

            if (_playerData.PlayerSettingsData.ResolutionsList == null)
            {
                _playerData.PlayerSettingsData.ResolutionsList = new List<string>();
            }

            foreach (var item in _resolutions)
            {
                var str = item.width + "x" + item.height;
                _resolutionsList.Add(str);
                if (!_playerData.PlayerSettingsData.ResolutionsList.Contains(str))
                {
                    _playerData.PlayerSettingsData.ResolutionsList.Add(str);
                }
            }

            _dropdownScreenResolution.ClearOptions();
            _dropdownScreenResolution.AddOptions(_resolutionsList);
        }

        

        public void PlayGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        public void QuitGame() => Application.Quit();

        public void FullScreenToggle()
        {
            _isFullScreen = !_isFullScreen;
            _playerData.PlayerSettingsData.IsFullScreenMode = _isFullScreen;
            Screen.fullScreen = _isFullScreen;
        }

        public void AudioVolumeMaster(float sliderValue)
        {
            AudioMixer.SetFloat("MasterVolume", sliderValue);
        }

       

        public void Quality(int value)
        {
            QualitySettings.SetQualityLevel(value);
            //QualitySettings.renderPipeline = _qualityRenderPipline[value];
        }

        public void Resolution(int value)
        {
            Screen.SetResolution(_playerData.PlayerSettingsData.Resolutions[value].width, _playerData.PlayerSettingsData.Resolutions[value].height, _isFullScreen);
            if (_playerData.PlayerSettingsData.CurrentResolution != value)
            {
                _playerData.PlayerSettingsData.CurrentResolution = value; 
            }
        }
    }
}
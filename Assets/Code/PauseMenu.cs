using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JevLogin
{
    internal sealed class PauseMenu : MainMenu
    {
        [SerializeField] private GameObject _panelPauseMenu;
        [SerializeField] private bool _isActive;
        [SerializeField] private Slider _sliderMasterVolume;


        private GameObject _currentPauseMenu;

        public GameObject CurrentPauseMenu { get => _currentPauseMenu; private set => _currentPauseMenu = value; }

        private void Awake()
        {
            CurrentPauseMenu = _panelPauseMenu;
            GetAudioVolumeMaster();
        }

        private void SetResolution()
        {
            if (_playerData.PlayerSettingsData.Resolutions != null)
            {
                _resolutions = _playerData.PlayerSettingsData.Resolutions; 
            }
            else if (_playerData.PlayerSettingsData.Resolutions == null)
            {
                _resolutions = Screen.resolutions;
                _playerData.PlayerSettingsData.Resolutions = _resolutions;
            }

            Screen.fullScreen = _playerData.PlayerSettingsData.IsFullScreenMode;
            _isFullScreenToggle.isOn = _playerData.PlayerSettingsData.IsFullScreenMode;
        }

        private void GetActiveQuality()
        {
            _dropdownQualitySettings.value = QualitySettings.GetQualityLevel();
        }

        private void GetActiveResolution()
        {
            _dropdownScreenResolution.ClearOptions();
            _dropdownScreenResolution.AddOptions(_playerData.PlayerSettingsData.ResolutionsList);
            _dropdownScreenResolution.value = _playerData.PlayerSettingsData.CurrentResolution;
        }

        private void Update()
        {
            if (Input.GetButtonDown(ManagerAxis.CANCEL))
            {
                _isActive = !_isActive;
                OnApplicationPause(_isActive);
            }
        }

        public void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
                //Time.timeScale = 0;

                _isActive = true;
                CurrentPauseMenu.SetActive(_isActive);
            }
            else
            {
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
                //Time.timeScale = 1;
                
                _isActive = false;
                CurrentPauseMenu.SetActive(_isActive);
            }
        }

        public void GetAudioVolumeMaster()
        {
            AudioMixer.GetFloat("MasterVolume", out var masterVolume);
            _sliderMasterVolume.value = masterVolume;
        }

        public void SwitchCurrentPausePanelMenu(GameObject panelMenu)
        {
            CurrentPauseMenu = panelMenu;
        }
    }
}
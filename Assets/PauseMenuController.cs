using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private GameObject _pausePanel;
    
    private bool _isActive;

    public void AudioVolumeMaster(float sliderValue)
    {
        _audioMixer.SetFloat("MasterVolume", sliderValue);
    }

    private void Start()
    {
        _pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (!_playerController.IsActivated)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isActive = !_isActive;
            OnApplicationPause(_isActive);
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            _isActive = true;
            _pausePanel.SetActive(_isActive);
        }
        else
        {
            Time.timeScale = 1;
            _isActive = false;
            _pausePanel.SetActive(_isActive);
        }
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }
}

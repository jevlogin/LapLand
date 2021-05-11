using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Material _materialPlayer;

    
    private void Start()
    {
        _audioMixer.GetFloat("MasterVolume", out var value);
        _volumeSlider.value = value;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeVolume(float value)
    {
        _audioMixer.SetFloat("MasterVolume", value);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

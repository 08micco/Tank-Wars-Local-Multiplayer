using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject settings;
    public GameObject menu;
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
    	resolutions = Screen.resolutions;

    	resolutionDropdown.ClearOptions();

    	List<string> options = new List<string>();

    	for (int i = 0; i < resolutions.Length; i++)
    	{
    		string option = resolutions[i].width + "x" + resolutions[i].height;
    		options.Add(option);
    	}

    	resolutionDropdown.AddOptions(options);
    }

    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.Escape))
    	{
    		menu.SetActive(true);
    		settings.SetActive(false);
    	}
    }

    public void StartGame()
    {
    	SceneManager.LoadScene(6);
    }

    public void QuitGame()
    {
    	Application.Quit();
    }

    public void Options()
    {
    	menu.SetActive(false);
    	settings.SetActive(true);
    } 

    public void Volume(float volume)
    {
    	audioMixer.SetFloat("volume", volume);
    }
    public void Quality(int qualityIndex)
    {
    	QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void FullScreen (bool isFullscreen)
    {
    	Screen.fullScreen = isFullscreen;
    }

    public void BackToMenu()
    {
    	menu.SetActive(true);
    	settings.SetActive(false);
    }
}

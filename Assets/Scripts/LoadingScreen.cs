using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
	List<string> gameScenes = new List<string>();
	public TextMeshProUGUI loadingText;
	public Slider slider;

	void Start()
	{
		gameScenes.Add("Map1");
		gameScenes.Add("Map2");
		gameScenes.Add("Map3");
		gameScenes.Add("Map4");
		gameScenes.Add("Map5");
		StartCoroutine(LoadingText());
		//StartCoroutine(Slider());

	}

	void Update()
	{
		slider.value += 0.5f;
		if(slider.value >= 100)
		{
			ResetGame();
		}
	}


	IEnumerator LoadingText()
    {
    	while(true)
    	{
    		loadingText.text = "Loading";
	        yield return new WaitForSeconds(0.4f);
	    	loadingText.text = "Loading.";
	        yield return new WaitForSeconds(0.4f);
	        loadingText.text = "Loading..";
	        yield return new WaitForSeconds(0.4f);
	        loadingText.text = "Loading...";
	        yield return new WaitForSeconds(0.4f);
    	}
    }

    IEnumerator Slider()
    {
    	
    		slider.value += 5;
	        yield return new WaitForSeconds(0.05f);
    	
    }

    void ResetGame()
    {
    	int index = Random.Range(0, 5);
        SceneManager.LoadScene(gameScenes[index]);
    }
}



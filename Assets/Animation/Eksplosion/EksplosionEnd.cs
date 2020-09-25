using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EksplosionEnd : MonoBehaviour
{
	void Start()
	{

	}
	void Update()
	{
		
	}

    void KillEksplosion()
    {
        Destroy(gameObject);
    }
    void ResetGame()
    {
    	//Loader Loadingscene, som er nummer 6 af scenerne
        SceneManager.LoadScene(6);
    }
}

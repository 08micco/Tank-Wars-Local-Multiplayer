using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShootSound : MonoBehaviour
{
	AudioSource audiosource;
    public AudioClip cannonSound;
    public AudioClip reloadSound;
    public AudioClip soundTrack;
    public AudioClip tankIdle;
    AudioSource cannonIdle;

    // Start is called before the first frame update
    void Awake()
    {
        //Declare audiosource
        audiosource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        audiosource.PlayOneShot(soundTrack, 0.35f);
       
    }

    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.R))
    	{
    		SceneManager.LoadScene("Map1");
    	}
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void CannonSound()
    {
        audiosource.PlayOneShot(cannonSound, 0.8f);
        Debug.Log("Shot");
    }
    public void ReloadSound()
    {
        audiosource.PlayOneShot(reloadSound, 1.0f);
        Debug.Log("Reloading...");
    }

    public void TankIdle()
    {
        audiosource.PlayOneShot(tankIdle, 0.003f);
    }

}

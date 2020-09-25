using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
	AudioSource audiosource;
    float rotDegree = 3;
    float speed = 3;
    float player2MaxHP = 3;
    float knockBack = 40;
    float player2CurrentHP;
    Rigidbody2D rb;

    ShootSound sound;
    P2Reload reload2;
    Animator anim;
    Player1 player1;
    public Animator reloadAnim2;
    GameObject gameManager;

    bool canShoot2 = true;
    Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject EksplosionPrefab;
    float bulletForce = 10.0f;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public bool doubleDMG2;

    // Start is called before the first frame update
    void Start()
    {
    	//Declare start HP
    	player2CurrentHP = player2MaxHP;
        //Declare audiosource
        audiosource = gameObject.GetComponent<AudioSource>();
        //Declare Rigidbody2D
        rb = gameObject.GetComponent<Rigidbody2D>();
        //Declare lydeffekten for skud fra et andet script
        sound = GameObject.FindObjectOfType(typeof(ShootSound)) as ShootSound;
        reload2 = GameObject.FindObjectOfType(typeof(P2Reload)) as P2Reload;
        player1 = GameObject.FindObjectOfType(typeof(Player1)) as Player1;
        //Declare Animator
        anim = gameObject.GetComponent<Animator>();
        //Declare firePoint
        firePoint = this.gameObject.transform.GetChild(0);
        //Declare gameManager objektet
        gameManager = GameObject.FindGameObjectWithTag("Manager");
        doubleDMG2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHP();
        shootTimer2();
    }

    void FixedUpdate()
    {
    	Movement();
    	Shoot();
    }

    // -------------------- Movement --------------------------
    void Movement()
    {
    	//Kører
            if(Input.GetKey(KeyCode.UpArrow))
            {
                gameObject.transform.position += transform.up * Time.deltaTime * speed;
                if(!audiosource.isPlaying)
                {
                    audiosource.Play();
                }
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                gameObject.transform.position += transform.up * Time.deltaTime * -speed;
                if(!audiosource.isPlaying)
                {
                    audiosource.Play();
                }
            }
            else
            {
                audiosource.Stop();
                sound.TankIdle();
            }
            //Rotere
            if(Input.GetKey(KeyCode.DownArrow))
	        {
	            	if(Input.GetKey(KeyCode.LeftArrow))
	            {
	                gameObject.transform.eulerAngles += Vector3.forward * -rotDegree;
	            }
	            if(Input.GetKey(KeyCode.RightArrow))
	            {
	                gameObject.transform.eulerAngles += Vector3.forward * rotDegree;
	            } 
	        }
	        else
	        {
	        	if(Input.GetKey(KeyCode.LeftArrow))
	            {
	                gameObject.transform.eulerAngles += Vector3.forward * rotDegree;
	            }
	            if(Input.GetKey(KeyCode.RightArrow))
	            {
	                gameObject.transform.eulerAngles += Vector3.forward * -rotDegree;
	            } 
	        } 
    }

    // -------------------- Shoot --------------------------
    void Shoot()
    {
    	if(Input.GetKeyDown(KeyCode.Keypad0) && canShoot2 == true || Input.GetKeyDown(KeyCode.RightShift) && canShoot2 == true)
        {
            reloadAnim2.SetBool("P2Reloading", true);
        	rb.AddForce(-transform.up * knockBack);
            sound.CannonSound();  
            anim.SetBool("Shooting2",true);
            canShoot2 = false;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
            rb2d.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        }
    }

    void shootTimer2()
    {
        if(reload2.reloadingTank2 == true)
        {
        Debug.Log("Player2 Skyde Klar");
        canShoot2 = true;
        anim.SetBool("Shooting2",false);
        reloadAnim2.SetBool("P2Reloading", false);
        reload2.reloadingTank2 = false;
        }
    }

    // -------------------- Reload - HP --------------------------
    public void Reloading()
    {
        sound.ReloadSound();
    }

    public void LoseHP()
    {
    	bool doubleDMG1 = player1.doubleDMG1;
        if(doubleDMG1 == true)
    	{
    		player2CurrentHP -= 2;
    	}
    	else
    	{
    		player2CurrentHP--;
    	}
    }

    void GetHP()
    {
        player2CurrentHP++;
    }
    public void DeathPlayer2()
    {
        GameObject Eksplosion = Instantiate(EksplosionPrefab, firePoint.position, firePoint.rotation);
        Destroy(gameObject);
    }

    // -------------------- Display HP --------------------------
    void DisplayHP()
    {
    	if(player2CurrentHP == 3)
    	{
    		heart1.SetActive(true);
    		heart2.SetActive(false);
    		heart3.SetActive(false);
    	}
    	else if(player2CurrentHP == 2)
    	{
    		heart1.SetActive(false);
    		heart2.SetActive(true);
    		heart3.SetActive(false);
    	}
    	else if(player2CurrentHP == 1)
    	{
    		heart1.SetActive(false);
    		heart2.SetActive(false);
    		heart3.SetActive(true);
    	}
    	else if(player2CurrentHP <= 0)
    	{
    		heart1.SetActive(false);
    		heart2.SetActive(false);
    		heart3.SetActive(false);
    		DeathPlayer2();
    	}
    }

    public void ResetRotation()
    {
    	rb.angularVelocity = 0;
    	GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    // -------------------- PowerUp --------------------------


}

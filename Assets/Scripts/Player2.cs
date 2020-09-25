using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player2 : MonoBehaviour
{
	AudioSource audiosource;
    float rotDegree = 3;
    float speed = 3;
    float player2MaxHP = 3;
    float player2CurrentHP;
    float knockBack = 40;
    
    Rigidbody2D rb;
    ShootSound sound;
    P2Reload reload2;
    Animator anim;
    Player1 player1;
    spawnPowerUp spawnPowerUpF;

    public Animator reloadAnim2;
    GameObject gameManager;

    bool canShoot2 = true;
    Transform firePoint;
    Transform firePoint2;
    public GameObject bulletPrefab;
    public GameObject EksplosionPrefab;
    float bulletForce = 10.0f;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public bool doubleDMG2;
    public bool doubleShot;
    bool reloadPowerUp = false;

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
        spawnPowerUpF = GameObject.FindObjectOfType(typeof(spawnPowerUp)) as spawnPowerUp;
        //Declare Animator
        anim = gameObject.GetComponent<Animator>();
        //Declare firePoint
        firePoint = this.gameObject.transform.GetChild(0);
        firePoint2 = this.gameObject.transform.GetChild(1);
        //Declare gameManager objektet
        gameManager = GameObject.FindGameObjectWithTag("Manager");
        doubleDMG2 = false;
        doubleShot = false;
        //Declare UI-Komponenter
        heart1 = GameObject.Find("P2-1");
        heart2 = GameObject.Find("P2-2");
        heart3 = GameObject.Find("P2-3");
        reloadAnim2 = GameObject.Find("P2Reload").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHP();
        shootTimer2();
    }

    void FixedUpdate()
    {
    	Shoot();
    	Movement();
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
    IEnumerator DoubleShot()
    {
        yield return new WaitForSeconds(0.12f);
    	GameObject bullet = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
        Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
        rb2d.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    void Shoot()
    {
    	if(Input.GetKeyDown(KeyCode.Keypad0) && canShoot2 == true || Input.GetKeyDown(KeyCode.RightShift) && canShoot2 == true)
        {
        	//Checker om man har powerup og vælger derudfra animation (Skud reload icon)
        	if(reloadPowerUp == true){
                reloadAnim2.SetBool("P2ReloadingPowerUp", true);
            }
            else
            {
                reloadAnim2.SetBool("P2Reloading", true);
            }
        	rb.AddForce(-transform.up * knockBack);
            sound.CannonSound();
            anim.SetBool("Shooting2",true);
            canShoot2 = false;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
            rb2d.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            if(doubleShot == true)
            {
            	StartCoroutine(DoubleShot());
            }
        }
    }
    //Resetter alle animationer og variabler, så tanken kan skyde igen
    //Ved hjælp af canShoot
    public void shootTimer2()
    {
        if(reload2.reloadingTank2 == true)
        {
        Debug.Log("Player2 Skyde Klar");
        canShoot2 = true;
        anim.SetBool("Shooting2",false);
        reloadAnim2.SetBool("P2ReloadingPowerUp", false);
        reloadAnim2.SetBool("P2Reloading", false);
        reload2.reloadingTank2 = false;
        }
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
    	else if (player2CurrentHP == 2)
    	{
    		heart1.SetActive(false);
    		heart2.SetActive(true);
    		heart3.SetActive(false);
    	}
    	else if (player2CurrentHP == 1)
    	{
    		heart1.SetActive(false);
    		heart2.SetActive(false);
    		heart3.SetActive(true);
    	}
    	else if (player2CurrentHP <= 0)
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
        if(player2CurrentHP < player2MaxHP)
    	{
    		print("Gained HP");
    		player2CurrentHP++;
    	}
    	else
    	{
    		print("Already at Max HP");
    	}
    }

    public void DeathPlayer2()
    {
        GameObject Eksplosion = Instantiate(EksplosionPrefab, firePoint.position, firePoint.rotation);
        Destroy(gameObject);
    }

    // -------------------- PowerUp --------------------------
    IEnumerator DoubleDmgTime()
    {
    	doubleDMG2 = true;
		print("Double Damage Activated");
        yield return new WaitForSeconds(5.0f);
        doubleDMG2 = false;
        print("Double Damage Deactivated");
        spawnPowerUpF.powerUpCount--;
    }
    IEnumerator DoubleShotTime()
    {
    	doubleShot = true;
    	knockBack = 80;
    	print("Double Shot Activated");
        yield return new WaitForSeconds(5.0f);
        doubleShot = false;
        knockBack = 40;
        print("Double Shot Deactivated");
        spawnPowerUpF.powerUpCount--;
    }
    IEnumerator FasterReloadTime()
    {
		print("Faster Reload");
	    reloadPowerUp = true;
        yield return new WaitForSeconds(5.0f);
	    reloadPowerUp = false;
	    print("Faster Reload Stop");
        spawnPowerUpF.powerUpCount--;
    }
    IEnumerator FasterSpeedTime()
    {
    	rotDegree++;
    	speed++;
    	print("Increased Tank Speed and Rotation");
        yield return new WaitForSeconds(5.0f);
        rotDegree--;
        speed--;
        print("Decreased Tank Speed and Rotation");
        spawnPowerUpF.powerUpCount--;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.tag == "PU_+HP")
    	{
    		GetHP();
    		print("Gained HP");
    		Destroy(col.gameObject);
    	}
    	if(col.gameObject.tag == "PU_Double_DMG")
    	{
    		Destroy(col.gameObject);
    		StartCoroutine(DoubleDmgTime());

    	}
    	if(col.gameObject.tag == "PU_Double_Shot")
    	{
    		Destroy(col.gameObject);
    		StartCoroutine(DoubleShotTime());
    	}
    	if(col.gameObject.tag == "PU_Faster_Reload")
    	{
    		Destroy(col.gameObject);
    		StartCoroutine(FasterReloadTime());

    	}
    	if(col.gameObject.tag == "PU_Add_Speed")
    	{
    		Destroy(col.gameObject);
			StartCoroutine(FasterSpeedTime());
    	}
    	/*if(col.gameObject.tag == "PU_Laser")
    	{
    		Debug.DrawRay(gameObject.transform.position, Vector2.up, Color.green);
    		print("Supervisor Activated");
    		Destroy(col.gameObject);
    	}*/
    }

}

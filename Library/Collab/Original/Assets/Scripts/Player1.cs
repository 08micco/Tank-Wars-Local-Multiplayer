using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player1 : MonoBehaviour
{
	AudioSource audiosource;
    float rotDegree = 3;
    float speed = 3;
    float player1MaxHP = 3;
    float player1CurrentHP;
    float knockBack = 40;

    Rigidbody2D rb;
    ShootSound sound;
    P1Reload reload1;
    Animator anim;
    Player2 player2;
    public Animator reloadAnim;
    GameObject gameManager;

    bool canShoot1 = true;
    Transform firePoint;
    Transform firePoint2;
    public GameObject bulletPrefab;
    public GameObject EksplosionPrefab;
    float bulletForce = 10.0f;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public bool doubleDMG1;
    public bool doubleShot;
    bool reloadPowerUp = false;

    // Start is called before the first frame update
    void Start()
    {
    	//Declare start HP
    	player1CurrentHP = player1MaxHP;
        //Declare audiosource
        audiosource = gameObject.GetComponent<AudioSource>();
        //Declare Rigidbody2D
        rb = gameObject.GetComponent<Rigidbody2D>();
        //Declare lydeffekten for skud fra et andet script
        sound = GameObject.FindObjectOfType(typeof(ShootSound)) as ShootSound;
        reload1 = GameObject.FindObjectOfType(typeof(P1Reload)) as P1Reload;
        player2 = GameObject.FindObjectOfType(typeof(Player2)) as Player2;
        //Declare Animator
        anim = gameObject.GetComponent<Animator>();
        //Declare firePoint
        firePoint = this.gameObject.transform.GetChild(0);
        firePoint2 = this.gameObject.transform.GetChild(1);
        //Declare gameManager objektet
        gameManager = GameObject.FindGameObjectWithTag("Manager");
        doubleDMG1 = false;
        doubleShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHP();
        shootTimer1();
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
            if(Input.GetKey(KeyCode.W))
            {
                gameObject.transform.position += transform.up * Time.deltaTime * speed;
                if(!audiosource.isPlaying)
                {
                    audiosource.Play();
                }
            }
            else if(Input.GetKey(KeyCode.S))
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
            if(Input.GetKey(KeyCode.A))
            {
                gameObject.transform.eulerAngles += Vector3.forward * rotDegree;
            }
            if(Input.GetKey(KeyCode.D))
            {
                gameObject.transform.eulerAngles += Vector3.forward * -rotDegree;
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
    	//Skyde funktion
    	if(Input.GetKeyDown(KeyCode.Space) && canShoot1 == true)
        {   
            if(reloadPowerUp == true){
                reloadAnim.SetBool("P1ReloadingPowerUp", true);

            }
            else
            {
                reloadAnim.SetBool("P1Reloading", true);
            }
        	rb.AddForce(-transform.up * knockBack);
            sound.CannonSound();
            anim.SetBool("Shooting1",true);
            canShoot1 = false;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();
            rb2d.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            if(doubleShot == true)
            {
            	StartCoroutine(DoubleShot());
            }
        }
    }

    public void shootTimer1()
    {
        if(reload1.reloadingTank1 == true)
        {
        Debug.Log("Player1 Skyde Klar");
        canShoot1 = true;
        anim.SetBool("Shooting1", false);
        reloadAnim.SetBool("P1ReloadingPowerUp", false);
        reloadAnim.SetBool("P1Reloading", false);
        reload1.reloadingTank1 = false;
        }
    }

    // -------------------- Display HP --------------------------
    void DisplayHP()
    {
    	if(player1CurrentHP == 3)
    	{
    		heart1.SetActive(true);
    		heart2.SetActive(false);
    		heart3.SetActive(false);
    	}
    	else if (player1CurrentHP == 2)
    	{
    		heart1.SetActive(false);
    		heart2.SetActive(true);
    		heart3.SetActive(false);
    	}
    	else if (player1CurrentHP == 1)
    	{
    		heart1.SetActive(false);
    		heart2.SetActive(false);
    		heart3.SetActive(true);
    	}
    	else if (player1CurrentHP <= 0)
    	{
    		heart1.SetActive(false);
    		heart2.SetActive(false);
    		heart3.SetActive(false);
    		DeathPlayer1();
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
    	bool doubleDMG2 = player2.doubleDMG2;
    	if(doubleDMG2 == true)
    	{
    		player1CurrentHP -= 2;
    	}
    	else
    	{
    		player1CurrentHP--;
    	}
        
    }

    void GetHP()
    {
    	if(player1CurrentHP < player1MaxHP)
    	{
    		print("Gained HP");
    		player1CurrentHP++;
    	}
    	else
    	{
    		print("Already at Max HP");
    	}
    }

    public void DeathPlayer1()
    {
        GameObject Eksplosion = Instantiate(EksplosionPrefab, firePoint.position, firePoint.rotation);
        Destroy(gameObject);
    }

    
    // -------------------- PowerUp --------------------------
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
    		doubleDMG1 = true;
    		print("Double Damage Activated");
    		Destroy(col.gameObject);
    	}
    	if(col.gameObject.tag == "PU_Double_Shot")
    	{
    		doubleShot = true;
    		print("Double Shot Activated");
    		Destroy(col.gameObject);
    	}
    	if(col.gameObject.tag == "PU_Faster_Reload")
    	{
    		print("Faster Reload");
            reloadPowerUp = true;
    		Destroy(col.gameObject);
    	}
    	if(col.gameObject.tag == "PU_Add_Speed")
    	{
    		rotDegree++;
    		speed++;
    		print("Increased Tank Speed and Rotation");
    		Destroy(col.gameObject);
    	}
    	if(col.gameObject.tag == "PU_Laser")
    	{
    		Debug.DrawRay(gameObject.transform.position, Vector2.up, Color.green);
    		print("Supervisor Activated");
    		Destroy(col.gameObject);
    	}
    }


}
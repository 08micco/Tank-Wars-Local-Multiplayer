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
    spawnPowerUp spawnPowerUpF;
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

    private CanvasGroup cg1;
    private CanvasGroup cg2;

    // Start is called before the first frame update
    void Start()
    {
    	//Declare start HP
    	player1CurrentHP = player1MaxHP;
        //Declare audiosource
        audiosource = gameObject.GetComponent<AudioSource>();
        //Declare Rigidbody2D
        rb = gameObject.GetComponent<Rigidbody2D>();
        //Finder andre scripts så variabler fra disse kan ændres og benyttes
        sound = GameObject.FindObjectOfType(typeof(ShootSound)) as ShootSound;
        reload1 = GameObject.FindObjectOfType(typeof(P1Reload)) as P1Reload;
        player2 = GameObject.FindObjectOfType(typeof(Player2)) as Player2;
        spawnPowerUpF = GameObject.FindObjectOfType(typeof(spawnPowerUp)) as spawnPowerUp;
        //Declare Animator
        anim = gameObject.GetComponent<Animator>();
        //Declare firePoint
        firePoint = this.gameObject.transform.GetChild(0);
        firePoint2 = this.gameObject.transform.GetChild(1);
        //Declare gameManager objektet
        gameManager = GameObject.FindGameObjectWithTag("Manager");
        doubleDMG1 = false;
        doubleShot = false;
        //Declare UI-Komponenter
        heart1 = GameObject.Find("P1-1");
        heart2 = GameObject.Find("P1-2");
        heart3 = GameObject.Find("P1-3");
        //Delcare animator
        reloadAnim = GameObject.Find("P1Reload").GetComponent<Animator>();
        //Declare UI transparent component
        cg1 = GameObject.Find("P1").GetComponent<CanvasGroup>();
        cg2 = GameObject.Find("P2").GetComponent<CanvasGroup>();
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
            //Checker om man har powerup og vælger derudfra animation (Skud reload icon)
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
    //Resetter alle animationer og variabler, så tanken kan skyde igen
    //Ved hjælp af canShoot
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
    IEnumerator DoubleDmgTime()
    {
        spawnPowerUpF.powerUpCount--;
    	doubleDMG1 = true;
		print("Double Damage Activated");
        yield return new WaitForSeconds(5.0f);
        doubleDMG1 = false;
        print("Double Damage Deactivated");
    }
    IEnumerator DoubleShotTime()
    {
        spawnPowerUpF.powerUpCount--;
    	doubleShot = true;
    	knockBack = 80;
    	print("Double Shot Activated");
        yield return new WaitForSeconds(5.0f);
        doubleShot = false;
        knockBack = 40;
        print("Double Shot Deactivated");
    }
    IEnumerator FasterReloadTime()
    {
        spawnPowerUpF.powerUpCount--;
		print("Faster Reload");
	    reloadPowerUp = true;
        yield return new WaitForSeconds(5.0f);
	    reloadPowerUp = false;
	    print("Faster Reload Stop");
    }
    IEnumerator FasterSpeedTime()
    {
        spawnPowerUpF.powerUpCount--;
    	rotDegree++;
    	speed++;
    	print("Increased Tank Speed and Rotation");
        yield return new WaitForSeconds(5.0f);
        rotDegree--;
        speed--;
        print("Decreased Tank Speed and Rotation");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.tag == "PU_+HP")
    	{
        	spawnPowerUpF.powerUpCount--;
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
    
    //Detecter om tank er under UI-Komponenterne

    IEnumerator UIAnimEnter()
    {
        if(cg1.alpha == 1)
        {
            for(int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.1f);
                cg1.alpha -= 0.01f;
                print(cg1.alpha);
                print(i);
            }
        }
       
    }
    IEnumerator UIAnimExit()
    {
        if(cg1.alpha == 1)
        {
            for(int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.5f);
                cg1.alpha += 0.1f;
            }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "UI_Collider")
        {
            StartCoroutine(UIAnimEnter());
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "UI_Collider")
        {
            if(cg1.alpha == 0.5f)
            {
                StartCoroutine(UIAnimExit());
            }
        }
    }

}
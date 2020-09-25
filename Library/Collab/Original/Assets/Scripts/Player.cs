using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    float rotDegree = 3;
    float speed = 3;
    GameObject Player1;
    GameObject Player2;
    GameObject gameManager;
    ShootSound sound;
    AudioSource audiosource;

    bool canShoot1 = false;
    bool canShoot2 = false;
    float shootCD = 3.0f;
    Transform firePoint;
    public GameObject bulletPrefab;
    float bulletForce = 5.0f;

    void Start()
    {   
        //Declare Rigidbody2D
        rb = gameObject.GetComponent<Rigidbody2D>();
        //Declare Animator
        anim = gameObject.GetComponent<Animator>();
        //Player 1 og 2 samt gameManager som gameobject
        Player1 = GameObject.FindGameObjectWithTag("Player1");
        Player2 = GameObject.FindGameObjectWithTag("Player2");
        gameManager = GameObject.FindGameObjectWithTag("Manager");
        //Declare lydeffekten for skud fra et andet script
        sound = GameObject.FindObjectOfType(typeof(ShootSound)) as ShootSound;
        //Declare audiosource
        audiosource = gameObject.GetComponent<AudioSource>();
        //Declare firePoint
        firePoint = this.gameObject.transform.GetChild(2);
        shootTimer();
    }

    void Update()
    {
        Shoot();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {   
        //Player1
        if(gameObject.tag == "Player1")
        {
            //Kører
            if(Input.GetKey(KeyCode.W))
            {
                Player1.transform.position += transform.up * Time.deltaTime * speed;
                if(!audiosource.isPlaying)
                {
                    audiosource.Play();
                }
            }
            else if(Input.GetKey(KeyCode.S))
            {
                Player1.transform.position += transform.up * Time.deltaTime * -speed;
                if(!audiosource.isPlaying)
                {
                    audiosource.Play();
                }
            }
            else
            {
                audiosource.Stop();
            }
            //Rotere
            if(Input.GetKey(KeyCode.A))
            {
                Player1.transform.eulerAngles += Vector3.forward * rotDegree;
            }
            if(Input.GetKey(KeyCode.D))
            {
                Player1.transform.eulerAngles += Vector3.forward * -rotDegree;
            } 
        }
        //Player2
        else if (gameObject.tag == "Player2")
        {
            //Kører
            if(Input.GetKey(KeyCode.UpArrow))
            {
                Player2.transform.position += transform.up * Time.deltaTime * speed;
                if(!audiosource.isPlaying)
                {
                    audiosource.Play();
                }
            }
            else if(Input.GetKey(KeyCode.DownArrow))
            {
                Player2.transform.position += transform.up * Time.deltaTime * -speed;
                if(!audiosource.isPlaying)
                {
                    audiosource.Play();
                }
            }
            else
            {
                audiosource.Stop();
            }
            //Rotere
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                Player2.transform.eulerAngles += Vector3.forward * rotDegree;
            }
            if(Input.GetKey(KeyCode.RightArrow))
            {
                Player2.transform.eulerAngles += Vector3.forward * -rotDegree;
            } 
        }
        
    }

    void Shoot()
    {
        //Player1 skyde
        if(Input.GetKeyDown(KeyCode.Space) && canShoot1 == true)
        {
            sound.CannonSound();
            anim.SetBool("Shooting1",true);
            canShoot1 = false;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        }
        //Player2 skyde
        if(Input.GetKeyDown(KeyCode.Keypad0) && canShoot2 == true)
        {
            sound.CannonSound();  
            anim.SetBool("Shooting2",true);
            canShoot2 = false;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
        }
        
    }

    void shootTimer()
    {
        if (gameObject.tag == "Player1")
        {
            Debug.Log("Player1 Skyde Klar");
            canShoot1 = true;
            anim.SetBool("Shooting1", false);
        }
        if (gameObject.tag == "Player2")
        {
            Debug.Log("Player2 Skyde Klar");
            canShoot2 = true;
            anim.SetBool("Shooting2",false);
        }
    }

    void reloadSound()
    {
        
    }

}

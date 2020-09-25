using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	Player1 player1;
	Player2 player2;
    private GameObject dustParticle;

	private void Start() 
	{
		player1 = GameObject.FindObjectOfType(typeof(Player1)) as Player1;
		player2 = GameObject.FindObjectOfType(typeof(Player2)) as Player2;
        dustParticle = GameObject.Find("Dust_Particle");
	}
    void OnCollisionEnter2D(Collision2D col)
    {
		if(col.gameObject.tag == "Player1")
		{
			player1.LoseHP();
            player1.ResetRotation();
		}
		else if(col.gameObject.tag == "Player2")
		{
			player2.LoseHP();
            player2.ResetRotation();
		}
        //Støv effekt Indsæt
        GameObject dust = Instantiate(dustParticle, transform.position, transform.rotation);
        //Støv effekt fjern
        Object.Destroy(dust, 1.0f);
        
        //Fjern skuddet når det rammer noget
    	Destroy(gameObject);

    }
}

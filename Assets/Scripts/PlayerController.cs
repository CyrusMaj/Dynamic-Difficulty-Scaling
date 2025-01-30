using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 10f;
    public Slider healthBar;
    private float health = 1;

    public static float DamageRecieved = 0;

    void Update()
    {
        //Movement
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.E))     //Player instant healing
        {
            health = 1;
            healthBar.value = health;
        }

        if(health <= 0)     //Player death
        {
            SceneManager.LoadScene("SampleScene_03");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Damage player
        if (other.gameObject.tag == "EnemyProjectile")
        {
            health -= 0.1f;
            healthBar.value = health;

            DamageRecieved += 0.1f;
        }
    }
}

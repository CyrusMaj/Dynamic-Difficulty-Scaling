using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject BulletEmitter;    //Projectile
    public GameObject Bullet;           //ProjectilePrefab
    public float speed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Shooting
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Projectile instantiation
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Bullet, BulletEmitter.transform.position, BulletEmitter.transform.rotation) as GameObject;

            //Correct bullet rotation
            //Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 90);

            //
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

            //Adds force/speed to newly instantiated projectile
            Temporary_RigidBody.AddForce(transform.forward * speed);

            //Destroys projectile after 3 seconds
            Destroy(Temporary_Bullet_Handler, 3.0f);
        }

    }
}

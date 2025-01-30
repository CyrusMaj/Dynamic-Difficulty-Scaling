using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyAI_0001 : MonoBehaviour
{

    //Player/Enemy
    public GameObject player;
    private static GameObject target;
    private NavMeshAgent Agent;

    //Difficulty adjustment variables:
    public static float DamageInflicted;
    private static float AdjustmentValue;
    private float MovementSpeed = 10;
    private float RotationSpeed = 7;
    public float RotationSpeed_Mult;
    public float MovementSpeed_Mult;

    //UI
    public float EnemyHealth = 1;
    public Slider EnemyHealthBar;
    [SerializeField] Text movementSpeed;
    [SerializeField] Text turningSpeed;

    //Shooting variables
    bool shooting = false;
    public Transform ProjectileEmitter;
    public GameObject ProjectilePrefab;
    public GameObject particleHitFX;
    public float ProjectileSpeed;
    public float shootingLengthRange;
    float ShootingRate;
    public float ShootingRateCondition;
    public float ShootingRateMax;

    //Not being used
    private static string State = null;

    void Start()
    {
        Agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //Follow player
        target = player;
        Agent.SetDestination(target.transform.position);

        extraRotation();

        EnemyDeathCheck();

        ShootingRate = Random.Range(0.1f, ShootingRateMax);
        if (ShootingRate > ShootingRateCondition)
        {
            BeginRaycastShooting();
        }

        CheckVariables();

        //States();
    }

    void CheckVariables()
    {
        if (AdjustmentValue != EnemyManager.AdjustmentValue)
        {
            ReceiveVariables();
        }
    }

    //Gets adjusted static variables from Enemy Manager Script, and assigns them to this script's variables.
    void ReceiveVariables()
    {
        AdjustmentValue = EnemyManager.AdjustmentValue;     //Get Adjustment value from EnemyManager

        //Change movement speed
        MovementSpeed += (AdjustmentValue * MovementSpeed_Mult);
        Agent.speed = MovementSpeed;

        //Change rotation speed
        RotationSpeed += (AdjustmentValue * RotationSpeed_Mult);

        //Show variable values in UI
        movementSpeed.text = MovementSpeed.ToString();
        turningSpeed.text = RotationSpeed.ToString();
    }

    //Compute Damage Inflicted & Enemy Health
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            EnemyHealth -= 0.35f;
            EnemyHealthBar.value = EnemyHealth;

            DamageInflicted += 0.1f;
        }

        /*
        if (other.gameObject.tag == "InterestCube")
        {
            //Agent.SetDestination(null);
        }
        */
    }

    void extraRotation()
    {
        if (Agent.desiredVelocity.sqrMagnitude > Mathf.Epsilon)
        {
            Quaternion lookRotation = Quaternion.LookRotation(Agent.desiredVelocity, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, RotationSpeed * Time.deltaTime);
        }
    }

    void EnemyDeathCheck()
    {
        if (EnemyHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void BeginRaycastShooting()
    {
        //Raycast Shooting
        RaycastHit hit;
        Ray shootingRay = new Ray(ProjectileEmitter.transform.position, ProjectileEmitter.transform.forward);
        Debug.DrawRay(ProjectileEmitter.transform.position, ProjectileEmitter.transform.forward * shootingLengthRange);

        if (Physics.Raycast(shootingRay, out hit, shootingLengthRange))
        {
            if (hit.collider.name == "Player")
            {
                if (ShootingRate > ShootingRateCondition)
                {
                    StartCoroutine("Shooting");
                }
            }
        }
    }

    //Shooting
    IEnumerator Shooting()
    {
        float WaitSeconds = Random.Range(1f, 3f);

        yield return new WaitForSeconds(WaitSeconds);
        RaycastHit();
    }

    void RaycastHit()
    {
        //Projectile instantiation
        GameObject Temporary_Bullet_Handler;
        Temporary_Bullet_Handler = Instantiate(ProjectilePrefab, ProjectileEmitter.transform.position, ProjectileEmitter.transform.rotation) as GameObject;

        //Correct bullet rotation
        //Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 90);

        //
        Rigidbody Temporary_RigidBody;
        Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

        //Adds force/speed to newly instantiated projectile
        Temporary_RigidBody.AddForce(transform.forward * ProjectileSpeed);

        //Destroys projectile after 3 seconds
        Destroy(Temporary_Bullet_Handler, 3.0f);
    }

    //void States()
    //{
    //    State = EnemyManager.State;     //Get state value from Enemy Manager.

    //    if (State == "Attacking")
    //    {
    //        target = player;
    //        //Follow player
    //        Agent.SetDestination(target.transform.position);    //Change navmesh target to player.
    //    }

    //    else if (State == "Retreating")
    //    {
    //        target = EnemyManager.Spawn_Point;
    //        //Change navmesh target to random spawnpoint in scene. 
    //        //Get variable value from static variable in Enemy Manager.
    //        Agent.SetDestination(target.transform.position);
    //    }
    //}
}

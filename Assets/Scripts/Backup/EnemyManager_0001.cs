using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager_0001 : MonoBehaviour
{
    //These variables are not yet being used:
    //public float difficulty;
    private float AllowedValue;
    private float Current_AllowedValue;
    //public float Total_EnemiesAttacking;

    public Text damageInflicted;
    public Text damageRecieved;
    public Text adjustmentValue;
    public Text allowedValue;

    public GameObject[] Spawn_Points;
    public static GameObject Spawn_Point;

    public GameObject[] EnemyPrefabCount;
    public GameObject player;

    //Get variables from PlayerController and EnemyAI here:
    private static float DamageInflicted;
    private static float DamageRecieved;

    //Send this variable back to EnemyAI:
    public static float AdjustmentValue;

    int EnemyCount;
    int NewEnemyCount;
    [SerializeField] Text Enemies;

    [SerializeField] GameObject EnemyPrefab;

    public static string State = null;

    void start()
    {
        EnemyPrefabCount = GameObject.FindGameObjectsWithTag("enemy");  //fills array of game objects
        EnemyCount = EnemyPrefabCount.Length;                           //int number    //enemy count always equals actual amount of enemies in scene.
        Debug.Log("EnemyCount = " + EnemyCount);

        NewEnemyCount = EnemyCount; /* triggering infinite loop? */     //int number    
        Debug.Log("NewEnemyCount = " + NewEnemyCount);

        Enemies.text = EnemyCount.ToString();                           //outputs value to ui
        Current_AllowedValue = EnemyCount;                              //getting allowedvalue from enemycount
                                                                        /* triggering infinite loop? allowed value effectively being set to enemy count every frame later on?!? */
        Debug.Log("Current_AllowedValue = " + Current_AllowedValue);

        AllowedValue = Current_AllowedValue;
        Debug.Log("AllowedValue = " + AllowedValue);
    }

    void Update()
    {
        EnemyPrefabCount = GameObject.FindGameObjectsWithTag("Enemy");  //Fills array of game objects
        EnemyCount = EnemyPrefabCount.Length;                           //Int number    //Enemy count always equals actual amount of enemies in scene.
        Debug.Log("EnemyCount = " + EnemyCount);


        if (EnemyCount != NewEnemyCount)    //Measures when an enemy is killed.
        {
            Debug.Log("if (EnemyCount != NewEnemyCount)");

            AdjustDifficultyVariables();

            EnemyFluctuation();     /* Cause of bug is that it's executing every frame? Adding x ammt. of enemies every frame?!? 
                                       Not good. How do we stop this? Add condition to change AllowedValue or some other variable? 
                                       So that it only executes once? Or, so that nested statements inside functions will only execute once? */

            NewEnemyCount = EnemyCount; /* Triggering infinite loop? */ //Int number
                                                                        //Updated to reflect new enemy count.
            Debug.Log("NewEnemyCount = " + NewEnemyCount);


            Enemies.text = EnemyCount.ToString();                       //Outputs value to UI
            Current_AllowedValue = EnemyCount;
            Debug.Log("Current_AllowedValue = " + Current_AllowedValue);
        }

    }

    void AdjustDifficultyVariables()
    {
        Debug.Log("AdjustDifficultyVariables()");

        //Compute all variables
        DamageInflicted += EnemyAI.DamageInflicted;
        DamageRecieved = PlayerController.DamageRecieved;
        AdjustmentValue = (DamageInflicted - DamageRecieved);// / EnemyCount;  //If there's more enemies, adjustment value doesn't go up so much.          /* Triggering infinite loop? Divide by Allowed Value instead? */
        Debug.Log("AdjustmentValue = " + AdjustmentValue);


        Current_AllowedValue = AllowedValue;          /* Triggering infinite loop? Allowed value effectively being set to Enemy Count every frame?!? */
        Debug.Log("Current_AllowedValue = " + Current_AllowedValue);

        AllowedValue = EnemyCount + (Mathf.Ceil(AdjustmentValue));// * EnemyCount;   //Brings AdjustmentValue variable up.
        Debug.Log("AllowedValue = " + AllowedValue);

        //Outputting values to UI
        damageInflicted.text = DamageInflicted.ToString();
        damageRecieved.text = DamageRecieved.ToString();
        adjustmentValue.text = AdjustmentValue.ToString();
        allowedValue.text = AllowedValue.ToString();
    }

    void EnemyFluctuation()
    {
        Debug.Log("EnemyFluctuation()");

        //Addition of Enemies
        //Put this statement in update function:
        if (Current_AllowedValue < AllowedValue)            //If enemies in scene are less than the Allowed Value       /////* Triggering infinite loop?*/
        {
            Debug.Log("if (Current_AllowedValue < AllowedValue)");

            float EnemiesToAdd = AllowedValue - EnemyCount;
            Debug.Log("TempVal = " + EnemiesToAdd);


            for (int i = (int)EnemiesToAdd; i > 0; i--)      /* Getting stuck in an infinite loop? */
            {
                Debug.Log("for (int i = (int)TempVal; i > 0; i--)");

                AddEnemy();
            }

            //Revert values back, keep from getting stuck in infinite loop?
            Current_AllowedValue = AllowedValue;
            AllowedValue = EnemyCount;/////

        }

        //Subtraction of Enemies
        //Put this statement in update function:
        else if (Current_AllowedValue > AllowedValue)
        {
            float EnemiesToSubtract = EnemyCount - AllowedValue;

            Debug.Log("TempVal = " + EnemiesToSubtract);

            for (int i = (int)EnemiesToSubtract; i < 0; i++)
            {
                SubtractEnemy();
            }

            //Revert values back, keep from getting stuck in infinite loop!
        }
    }

    void AddEnemy()
    {
        //State = "Attacking";

        //Spawns a single enemy into the scene, or turns the closest patroling enemy into attack state.
        //If enemies in scene < allowed value, find spawnpoint closest to player, instantiate enemy prefab  
        //If enemies in scene > allowed value, find closest patrolling enemy, change to attack state

        // If (enemies in scene > allowed value)
        //Change 
        //Enemy instantiation
        float SpawnPointIndex = Random.Range(0, Spawn_Points.Length);
        SpawnPointIndex = Mathf.Ceil(SpawnPointIndex);
        int spawnPointIndex = (int)SpawnPointIndex;
        Spawn_Point = Spawn_Points[spawnPointIndex];

        GameObject Temporary_Spawner;
        Temporary_Spawner = Instantiate(EnemyPrefab, Spawn_Point.transform.position, Spawn_Point.transform.rotation) as GameObject;
    }

    void SubtractEnemy()
    {
        //Select spawn point at random.
        //float SpawnPointIndex = Random.Range(0, Spawn_Points.Length);
        //SpawnPointIndex = Mathf.Ceil(SpawnPointIndex);
        //int spawnPointIndex = (int)SpawnPointIndex;
        //Spawn_Point = Spawn_Points[spawnPointIndex];

        //Make array length = EnemyPrefabCount.Length
        float[] enemyDistance = new float[EnemyPrefabCount.Length];

        for (int i = EnemyPrefabCount.Length - 1; i > 0; i--)
        {
            //Store distance from each enemy in each array position
            enemyDistance[i] = Vector3.Distance(EnemyPrefabCount[i].transform.position, player.transform.position);
        }

        //Select enemy[index] of furthest distance from player. 
        float max = 0;
        int greatestValueindex = 0;
        for (int i = 0; i < enemyDistance.Length; i++)
        {
            if (max < enemyDistance[i])
            {
                Debug.Log("max < enemyDistance[i]");

                max = enemyDistance[i];
                greatestValueindex = i;
            }
        }

        Destroy(EnemyPrefabCount[greatestValueindex]);
        Debug.Log("Enemy Destroyed");

        //Use get component to get the Enemy AI script and change it's nav agent target to Spawn_Point.
        //Change_Target = this.GetComponent<EnemyPrefabCount[].Enemy_AI>();
        //Change_Target.target = Spawn_Point;
    }
}

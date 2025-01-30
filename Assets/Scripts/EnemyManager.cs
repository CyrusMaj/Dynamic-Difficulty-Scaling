using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    private float AllowedValue;

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

    //Keeping track of enemies in scene and if one has been killed
    int EnemyCount;
    int PreviousEnemyCount;
    [SerializeField] Text Enemies;

    //For spawning new enemies
    [SerializeField] GameObject EnemyPrefab;

    void Start()
    {
        //Fills array of game objects
        EnemyPrefabCount = GameObject.FindGameObjectsWithTag("Enemy");

        //Enemy count always equals actual amount of enemies in scene.
        EnemyCount = EnemyPrefabCount.Length;

        PreviousEnemyCount = EnemyCount;
    }

    void Update()
    {
        //Fills array of game objects
        EnemyPrefabCount = GameObject.FindGameObjectsWithTag("Enemy");  

        //Enemy count always equals actual amount of enemies in scene.
        EnemyCount = EnemyPrefabCount.Length;

        //Measures when an enemy is killed.
        if (EnemyCount != PreviousEnemyCount)    
        {
            AdjustDifficultyVariables();

            EnemyFluctuation();     

            PreviousEnemyCount = EnemyCount;

            //Outputs value to UI
            Enemies.text = EnemyCount.ToString();    

            AllowedValue = EnemyCount;
        }
    }

    void AdjustDifficultyVariables()
    {
        //Compute all variables
        DamageInflicted += EnemyAI.DamageInflicted;
        DamageRecieved = PlayerController.DamageRecieved;
        AdjustmentValue = (DamageInflicted - DamageRecieved);

        AllowedValue = PreviousEnemyCount + (Mathf.Ceil(AdjustmentValue));

        //Outputting values to UI
        damageInflicted.text = DamageInflicted.ToString();
        damageRecieved.text = DamageRecieved.ToString();
        adjustmentValue.text = AdjustmentValue.ToString();
        allowedValue.text = AllowedValue.ToString();
    }

    void EnemyFluctuation()
    {
        //Addition of Enemies
        //If enemies in scene are less than the Allowed Value
        if (EnemyCount < AllowedValue)            
        {
            float EnemiesToAdd = AllowedValue - EnemyCount;

            for (int i = (int)EnemiesToAdd; i > 0; i--)
            {
                AddEnemy();
            }
        }

        //Subtraction of Enemies
        else if (EnemyCount > AllowedValue)
        {
            float EnemiesToSubtract = EnemyCount - AllowedValue;

            for (int i = (int)EnemiesToSubtract; i > 0; i--)
            {
                SubtractEnemy();
            }
        }

        else if(EnemyCount == 0)
        {
            for (int i = 5; i > 0; i--)
            {
                AddEnemy();
            }
        }
    }

    void AddEnemy()
    {
        //Spawns a single enemy into the scene, 
        //or turns the closest patroling enemy into attack state.
        //If enemies in scene < allowed value, 
        //find spawnpoint closest to player, instantiate enemy prefab  
        //If enemies in scene > allowed value, 
        //find closest patrolling enemy, change to attack state

        float SpawnPointIndex = Random.Range(0, Spawn_Points.Length);
        SpawnPointIndex = Mathf.Ceil(SpawnPointIndex);
        int spawnPointIndex = (int)SpawnPointIndex;
        Spawn_Point = Spawn_Points[spawnPointIndex];

        GameObject Temporary_Spawner;
        Temporary_Spawner = Instantiate(EnemyPrefab, 
            Spawn_Point.transform.position, Spawn_Point.transform.rotation) as GameObject;
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
            enemyDistance[i] = Vector3.Distance
                (EnemyPrefabCount[i].transform.position, player.transform.position);
        }

        //Select enemy[index] of furthest distance from player. 
        float max = 0;
        int greatestValueindex = 0;
        for (int i = 0; i < enemyDistance.Length; i++)
        {
            if (max < enemyDistance[i])
            {
                max = enemyDistance[i];
                greatestValueindex = i;
            }
        }

        Destroy(EnemyPrefabCount[greatestValueindex]);

        //Use get component to get the Enemy AI script and change it's nav agent target to Spawn_Point.
        //Change_Target = this.GetComponent<EnemyPrefabCount[].Enemy_AI>();
        //Change_Target.target = Spawn_Point;
    }
}

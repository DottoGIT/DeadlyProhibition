using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTile : MonoBehaviour
{
    public GameObject VampireWalking;
    public GameObject VampireShooting;

    public float chanceToSpawnWalker;
    public float chanceToSpawnShooter;
    public float chanceToSpawnFurniture;


    [HideInInspector] public int myIndex;
    [SerializeField] List<Transform> enemySpawners = new List<Transform>();
    [SerializeField] List<GameObject> Furniture = new List<GameObject>();
    
    public void SpawnFurniture()
    {
        foreach (var obj in Furniture)
        {
            float rng = Random.Range(0f, 1f);
            if(rng < chanceToSpawnFurniture)
            {
                obj.SetActive(true);
            }
            else
            {
                Destroy(obj);
            }

        }
    }

    public void SpawnEnemies()
    {
        foreach(var spawner in enemySpawners)
        {
            TryToSpawnEnemy(spawner.position);    
        }
    }

    void TryToSpawnEnemy(Vector3 pos)
    {
        float rng = Random.Range(0f, 1f);
        if (rng < chanceToSpawnShooter)
        {
            Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0, 360));
            Instantiate(VampireShooting, pos, randomRot,transform);
        }
        else if (rng < chanceToSpawnWalker)
        {
            Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0, 360));
            Instantiate(VampireWalking, pos, randomRot,transform);
        }

    }
    
}

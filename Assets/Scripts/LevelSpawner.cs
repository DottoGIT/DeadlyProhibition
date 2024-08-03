using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] GameObject myBlockade;
    //Cant spawn other levels (except force spawn)
    public bool isDisabled { get; private set; } = false;
    //Cant spawn anything after any condition (spawns door after level generation)
    public bool forceDisabled = false;

    public void TryToDestroy()
    {
        if(!isDisabled)
        {
            forceDisabled = true;
        }
    }

    public void BlockadeEntrance()
    {
        myBlockade.SetActive(true);
    }

    public void SpawnTile(GameObject TilePrefab, Transform parent ,float spawnChance = 2)
    {
        if((spawnChance > 1 && !forceDisabled)||( Random.Range(0f,1f) < spawnChance && !isDisabled))
        {
            GameObject obj = Instantiate(TilePrefab, transform);
            obj.transform.parent = parent;
            LevelManager.Instance.levelTiles.Add(obj.GetComponent<LevelTile>());
            obj.GetComponent<LevelTile>().myIndex = LevelManager.Instance.TileIndex++;

        }
        else
        {
            isDisabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("LevelSpawnerDestroyer"))
        {
            Destroy(gameObject);
        }

    }


}

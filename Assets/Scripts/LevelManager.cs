using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [HideInInspector] public List<LevelTile> levelTiles;
    public GameObject TilePrefab;
    public Transform TilesParent;
    public Animator ShaderAnim;

    public float spawnChance;
    public int TilesToSpawn;
    public int TileIndex = 0;

    AstarPath AStar;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Generate Level Tiles
        AStar = GetComponent<AstarPath>();
        StartCoroutine(GenerateLevelTiles());
    }

    void SpawnTileWave()
    {
        //Spawn Tiles
        foreach(var spawner in FindActiveSpawners())
        {
            if (FindObjectsOfType<LevelTile>().Length < TilesToSpawn)
            {
                spawner.SpawnTile(TilePrefab, TilesParent, spawnChance);
            }
            else
            {
                break;
            }
        }
        //Delete Stacked Tiles
        List<LevelSpawner> spawnerList = FindActiveSpawners();
        for(int i = 0; i < spawnerList.Count-1; i++)
        {
            for(int j = i+1; j < spawnerList.Count; j++)
            {
                if(spawnerList[i].transform.position == spawnerList[j].transform.position)
                {
                    spawnerList[i].TryToDestroy();
                }
            }
        }
    }



    List<LevelSpawner> FindActiveSpawners()
    {
        List<LevelSpawner> spawnersList = new List<LevelSpawner>();

        foreach(var spawner in FindObjectsOfType<LevelSpawner>())
        {
            if(!spawner.forceDisabled)
            {
                spawnersList.Add(spawner);
            }
        }
        return spawnersList;
    }


    void AfterLevelTilesGenerated()
    {
        foreach(var spawner in FindObjectsOfType<LevelSpawner>())
        {
            spawner.BlockadeEntrance();
        }
        StartCoroutine(SpawnFurniture());
        StartCoroutine(ScanLevel());
        StartCoroutine(SpawnEnemies());
        StartCoroutine(DestroyDuplicatedRoomsBug());
        StartCoroutine(DestroyFirstRoomBug());
        StartCoroutine(StartGame());
    }

    IEnumerator DestroyFirstRoomBug()
    {
        yield return new WaitForEndOfFrame();
        foreach (var room in levelTiles)
        {
            if (room.gameObject.transform.position == Vector3.zero)
            {
                Destroy(room.gameObject);
            }
        }
    }

    IEnumerator DestroyDuplicatedRoomsBug()
    {
        yield return new WaitForEndOfFrame();
        List<LevelTile> lvlTileCopy = new List<LevelTile>();
        for(int i = 0; i < levelTiles.Count-1; i++)
        {
            bool shouldAdd = true;
            for (int j = i+1; j < levelTiles.Count; j++)
            {
                if(levelTiles[i].transform.position == levelTiles[j].transform.position)
                {
                    shouldAdd = false;
                }
            }
            if(shouldAdd)
            {
                lvlTileCopy.Add(levelTiles[i]);
            }
            else
            {
                Destroy(levelTiles[i]);
            }
        }
        levelTiles = new List<LevelTile>(lvlTileCopy);
    }


    IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        ShaderAnim.SetTrigger("StartGame");
        GameManager.instance.LevelLoaded = true;
    }


    IEnumerator SpawnFurniture()
    {
        yield return new WaitForEndOfFrame();
        foreach (var tile in levelTiles)
        {
            tile.SpawnFurniture();
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForEndOfFrame();
        foreach(var tile in levelTiles)
        {
            tile.SpawnEnemies();
        }
        
    }

    IEnumerator ScanLevel()
    {
        yield return new WaitForEndOfFrame();
        AStar.Scan();
    }

    IEnumerator GenerateLevelTiles(int oldTilesCount = 0)
    {
        yield return new WaitForEndOfFrame();
        int currentTiles = FindObjectsOfType<LevelTile>().Length;
        if (currentTiles < TilesToSpawn)
        {
            if(oldTilesCount != currentTiles)
            {
                SpawnTileWave();
                StartCoroutine(GenerateLevelTiles(currentTiles));
            }
            else
            {
                FindObjectsOfType<LevelSpawner>()[Random.Range(0, FindObjectsOfType<LevelSpawner>().Length)].SpawnTile(TilePrefab, TilesParent);
                StartCoroutine(GenerateLevelTiles(currentTiles));
            }
        }
        else
        {
            AfterLevelTilesGenerated();
        }
    }
}

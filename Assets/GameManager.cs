using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] public GameObject grass;
    [SerializeField] public GameObject road;
    [SerializeField] AudioManager audioManager;
    [SerializeField] int extent;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;

    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);

    TMP_Text gameOverText;

    private void Start()
    {

        Screen.SetResolution(360, 640, false);
        Screen.fullScreen = false;

        // setup game over panel
        gameOverPanel.SetActive(false);
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();


        // belakang
        for (int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(grass, z);
        }

        // depan
        for (int z = 1; z <= frontDistance; z++)
        {
            // penentuan terrain block dengan probabilitas 50%
            var prefab = GetNextRandomTerrainPrefab(z);

            CreateTerrain(prefab, z);
        }

        Debug.Log(Tree.AllPositions.Count);

        /*
        foreach (var pos in Tree.AllPositions)
        {
            Debug.Log(Tree.AllPositions);
        }
        */

        player.SetUp(backDistance, extent);
    }

    private int playerLastMaxTravel;

    private void Update()
    {
        // Cek player status hidup
        if (player.IsDie && gameOverPanel.activeInHierarchy == false)
            StartCoroutine(ShowGameOverPanel());

        // Infinite Terrain System
        if (player.MaxTravel == playerLastMaxTravel)
            return;

        playerLastMaxTravel = player.MaxTravel;

        // Buat ke depan
        var randtbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel + frontDistance);
        CreateTerrain(randtbPrefab, player.MaxTravel + frontDistance);

        // Hapus yang di belakang
        var lastTB = map[player.MaxTravel - 1 + backDistance];

        //TerrainBlock lastTB = map[player.MaxTravel + frontDistance];
        //int lastPos = player.MaxTravel;
        //foreach (var (pos, tb) in map)
        //{
        //    if (pos < lastPos)
        //    {
        //        lastPos = pos;
        //        lastTB = tb;
        //    }
        //}

        map.Remove(player.MaxTravel - 1 + backDistance);
        Destroy(lastTB.gameObject);

        player.SetUp(player.MaxTravel + backDistance, extent);
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1);

        Debug.Log("Game Over");
        //player.enabled = false;
        gameOverText.text = "YOUR SCORE: " + player.MaxTravel;
        gameOverPanel.SetActive(true);
        audioManager.gameOver();
    }

    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab, new Vector3(0, 0, zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos, tb);
        Debug.Log(map[zPos] is Road);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos)
    {

        bool isUniform = true;
        var tbRef = map[nextPos - 1]; 
        for (int distance = 2; distance <= maxSameTerrainRepeat; distance++)
        {
            if (map[nextPos - distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }

        if (isUniform)
        {
            if (tbRef is Grass)
                return road;
            else
                return grass;
        }

        // penentuan terrain block dengan probabilitas 50%
        return Random.value > 0.5f ? road : grass;
    }
}

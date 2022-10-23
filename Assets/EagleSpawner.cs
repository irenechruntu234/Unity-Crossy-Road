using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EagleSpawner : MonoBehaviour
{
    [SerializeField] GameObject eaglePrefab;
    [SerializeField] GameObject warningPanel;
    [SerializeField] int spawnZPos = 7;
    [SerializeField] Player player;
    [SerializeField] float timeOut = 5;
    [SerializeField] float timer = 0;
    [SerializeField] TMP_Text timerText;
    [SerializeField] AudioManager audioManager;
    int playerLastMaxTravel = 0;

    private void Start()
    {
        warningPanel.SetActive(false);
    }

    private void SpawnEagle()
    {
        player.enabled = false;
        var position = new Vector3(
            player.transform.position.x, 
            1, 
            player.CurrentTravel + spawnZPos);
        var rotation = Quaternion.Euler(0, 180, 0);
        var eagleObject = Instantiate(eaglePrefab, position, rotation);
        var eagle = eagleObject.GetComponent<Eagle>();
        eagle.SetUpTarget(player);
        audioManager.EagleFly();
    }

    private void Update()
    {
        // jika player ada kemajuan
        if (player.MaxTravel != playerLastMaxTravel)
        {
            // maka reset timer
            timer = 0;
            playerLastMaxTravel = player.MaxTravel;
        }

        // kalau ga maju jalankan timer
        if (timer < timeOut)
        {
            timer += Time.deltaTime;
            timerText.text = "TIMER: " + (timeOut - (int) timer);

            if ((timeOut - (int)timer) < 3)
            {
                warningPanel.SetActive(true);
                audioManager.warning();
            }
            else
            {
                audioManager.normal();
                warningPanel.SetActive(false);
            }

            if (player.IsDie == true)
            {
                timer = 0;
            }
            return;
        }

        // kalau sudah timeout
        if (player.IsJumping() == false && player.IsDie == false)
            SpawnEagle();
    }
}

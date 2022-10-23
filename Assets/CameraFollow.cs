using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Vector3 offset;

    private void Start()
    {
        offset = this.transform.position - player.transform.position;
    }

    Vector3 lastAnimalPosition;
    void Update()
    {
        if (player.IsDie || lastAnimalPosition == player.transform.position)
            return;

        var targetAnimalPos = new Vector3(
            player.transform.position.x,
            0,
            player.transform.position.z);

        transform.position = targetAnimalPos + offset;

        lastAnimalPosition = player.transform.position;
    }
}

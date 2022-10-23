using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text stepText;
    [SerializeField] ParticleSystem dieParticles;
    [SerializeField, Range(0.01f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight = 0.5f;
    [SerializeField] AudioManager audioManager;
    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel; }
    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel; }
    public bool IsDie { get => this.enabled == false; }

    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos - 1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;
    }

    private void Update()
    {
        var moveDir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDir += new Vector3(0, 0, 1);
            audioManager.playSFXJalan();

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDir += new Vector3(0, 0, -1);
            audioManager.playSFXJalan();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir += new Vector3(1, 0, 0);
            audioManager.playSFXJalan();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir += new Vector3(-1, 0, 0);
            audioManager.playSFXJalan();
        }

        if (moveDir != Vector3.zero && IsJumping() == false)
            Jump(moveDir);
    }

    private void Jump(Vector3 targetDirection)
    {
        Debug.Log(backBoundary);
        Debug.Log(leftBoundary);
        Debug.Log(rightBoundary);
        // Atur rotasi;
        var targetPosition = 
            transform.position + targetDirection;
        transform.LookAt(targetPosition);

        // Loncat ke atas
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration/2));

        if (targetPosition.z <= backBoundary  ||
            targetPosition.x < leftBoundary ||
            targetPosition.x > rightBoundary)
            return;

        if (Tree.AllPositions.Contains(targetPosition))
            return;

        // Gerak maju/mundur/samping
        transform.DOMoveX(targetPosition.x, moveDuration);
        transform
            .DOMoveZ(targetPosition.z, moveDuration)
            .OnComplete(UpdateTravel);
    }

    public void UpdateTravel()
    {
        currentTravel = (int)this.transform.position.z;

        if (currentTravel > maxTravel)
            maxTravel = currentTravel;

        stepText.text = "STEP: " + maxTravel.ToString();
    }

    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == false)
            return;

        // di execute sekali pada frame ketika nempel pertama kali
        var car = other.GetComponent<Car>();
        if(car != null)
        {
            Debug.Log("Hit " + car.name);
            AnimateCrash(car);
        }

        /* if (other.tag == "Car")
        {
            Debug.Log("Hit " + other.name);
            AnimateDie(car);
        }
        */
    }

    private void AnimateCrash(Car car)
    {
        /*
        var isRight = car.transform.rotation.y == 90;

        transform.DOMoveX(isRight ? 8 : -8, 0.2f);
        transform
            .DORotate(Vector3.forward * 360, 0.2f)
            .SetLoops(-1, LoopType.Restart);
        */

        // Gepeng
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(2, 0.2f);
        transform.DOScaleZ(2, 0.2f);
        
        this.enabled = false;
        dieParticles.Play();
        audioManager.CarCrash();
    }

    private void OnTriggerStay(Collider other)
    {
        // di execute setiap frame selama masih nempel
        
    }

    private void OnTriggerExit(Collider other)
    {
        // di execute sekali pada frame ketika tidak nempel
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource bgm;
    [SerializeField] public AudioClip musicStart;
    [SerializeField] private AudioSource eagle;
    [SerializeField] private AudioSource crushed;
    [SerializeField] private AudioSource fail;
    [SerializeField] Player player;

    float newSpeed = 1.2f;

    private void Start()
    {
        bgm.PlayOneShot(musicStart);
        bgm.PlayScheduled(AudioSettings.dspTime + musicStart.length);
    }
    public void playSFXJalan()
    {
        sfx.Play();
    }

    public void EagleFly()
    {
        eagle.Play();
    }

    public void CarCrash()
    {
        crushed.Play();
    }

    public void gameOver()
    {
        bgm.Stop();
        fail.Play();
    }

    public void warning()
    {
        bgm.pitch = newSpeed;
    }

    public void normal()
    {
        bgm.pitch = 1f;
    }
}

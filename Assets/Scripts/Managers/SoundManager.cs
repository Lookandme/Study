using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField][Range(0f, 1f)] float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] float musicVolume;

    private AudioSource musicAudioSource;
    public AudioClip musicClip;
    private void Awake()
    {
        instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
        musicAudioSource.loop = true;

    }

    private void Start()
    {
        ChangeBackGroundMusic(musicClip);
    }

    private void ChangeBackGroundMusic(AudioClip musicClip)
    {
        instance.musicAudioSource.Stop();
        instance.musicAudioSource.clip = musicClip;
        instance.musicAudioSource.Play();


    }

    public static void PlayClip(AudioClip clip)
    {
        GameObject obj = GameManager.Instance.ObjectPool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, instance.soundEffectVolume, instance.soundEffectPitchVariance);
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;
    
    [SerializeField] List<CustomDictionaryItem<AudioClip>> sounds;      //This must be changed in large project
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if(!_instance)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    public void PlaySound(string soundAction)
    {
        var clip = FindSound(soundAction);
        if(clip)
            sfxSource.PlayOneShot(clip);
            
    }
    public void StartMusic()
    {
        musicSource.Play();
    }
    
    private AudioClip FindSound(string soundAction)
    {
        return sounds.Find(sound => sound.ID == soundAction).Item;
    }
}

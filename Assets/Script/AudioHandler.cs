using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class AudioHandler : MonoBehaviour
{
    [SerializeField] List<AudioClip> landSFX = new List<AudioClip>();
    [SerializeField] List<AudioClip> pullSlingshot = new List<AudioClip>();
    [SerializeField] List<AudioClip> shootSlingshot = new List<AudioClip>();
    [SerializeField] List<AudioClip> buttonPress = new List<AudioClip>();
    [SerializeField] List<AudioClip> buttonHover = new List<AudioClip>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSFX(List<AudioClip> sfxList)
    {

    }
}

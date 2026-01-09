using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioPlayer
{
    static AudioList audioList;
    static AudioSource[] audioPlayer = new AudioSource[10];
    static AudioSource test;
    public static void GetAudioSystem()
    {
        audioList = GameObject.Find("AudioPlayer0").GetComponent<AudioList>();
        GameObject[] audioPlayerCache = GameObject.FindGameObjectsWithTag("AudioPlayer");
        for (int i = 0; i < audioPlayerCache.Length; i++) 
        {
            audioPlayer[i] = audioPlayerCache[i].GetComponent<AudioSource>();
        }
    }

    public static void PlayAudioClip(int line, int num, float volume, float pitch, float pan)
    {
        audioPlayer[line].clip = audioList.audioClips[num];
        audioPlayer[line].volume = volume;
        audioPlayer[line].pitch = pitch;
        audioPlayer[line].panStereo = pan;
        audioPlayer[line].Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitSound : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform") || collision.CompareTag("PlatformSound"))
        {
            AudioPlayer.PlayAudioClip(1, 1, 0.8f, 1, 0);
        }
        else if (collision.CompareTag("LaunchPad"))
        {

        }
    }
}

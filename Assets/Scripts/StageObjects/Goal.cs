using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enter");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("enterplayer");
            AudioPlayer.PlayAudioClip(0, 4, 1, 1, 0);
            GameObject.Find("GamePlayController").GetComponent<GamePlayController>().IsDead(true);
        }
    }
}

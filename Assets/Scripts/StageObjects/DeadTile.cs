using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!PlayerContoroller.isDead)
            {
                GameObject.Find("GamePlayController").GetComponent<GamePlayController>().IsDead(false);
            }
        }
    }
}

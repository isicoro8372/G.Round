using System;
using UnityEngine;

public class CameraColliderController : MonoBehaviour
{
    [SerializeField] private float smooth;
    [SerializeField] private int scale;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.GetComponent<Rigidbody2D>().constraints != RigidbodyConstraints2D.FreezePosition)
            {
                GameObject.Find("GamePlayController").GetComponent<CameraController>().MoveToNextPos(smooth, Convert.ToInt32(gameObject.name), scale);
            }
        }
    }
}

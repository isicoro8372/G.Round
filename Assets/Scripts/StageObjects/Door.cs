using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private bool isActive = true;
    private void Start()
    {
        door = transform.Find("Obj_Door").gameObject;
        door.GetComponent<SpriteRenderer>().color = new Vector4(0, 0, 255, 255);
    }
    public void Use()
    {
        if (isActive)
        {
            door.SetActive(false);
            isActive = false;
        }
        else
        {
            door.SetActive(true);
            isActive = true;
        }
    }
}

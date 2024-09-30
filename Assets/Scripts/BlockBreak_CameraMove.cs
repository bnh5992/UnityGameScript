using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak_CameraMove : MonoBehaviour
{
    public GameObject player;
    Vector3 target;
    Vector3 velo;
    // Start is called before the first frame update
    void Start()
    {
        velo = Vector2.zero;
        StartCoroutine(CameraMove());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CameraMove()
    {
        while (true)
        {
            target = player.GetComponent<Transform>().position + Vector3.back * 10;
            target.x = 0;
            transform.position = target + transform.up * 7;
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak_Destroy : MonoBehaviour
{
    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("debris"))
        {
            Destroy(collider.gameObject);
        }
    }
}

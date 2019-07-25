using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col : MonoBehaviour {

    // Use this for initialization
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}

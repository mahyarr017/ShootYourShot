using UnityEngine;
using System.Collections;

public class Roll : MonoBehaviour
{

    public float ballSpeed;


    // Update is called once per frame
    void Update()
    {
        float xSpeed = Input.GetAxis("Horizontal");
        float ySpeed = Input.GetAxis("Vertical");
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddTorque(new Vector3(xSpeed, 0, ySpeed) * ballSpeed * Time.deltaTime);
    }
}
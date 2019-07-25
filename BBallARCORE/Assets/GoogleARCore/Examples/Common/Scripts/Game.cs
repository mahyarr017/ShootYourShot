using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	// Use this for initialization
    private static bool running = false;
    public static int score = 0;

    private bool posession = false;
    

    public GameObject ball;
    public GameObject player;

    float touchTimeStart, touchTimeFinish, timeInterval;

    Vector2 startPos, endPos, direction;

    float throwForceInXandY = 1f;

    float throwForceInZ = 50f;

    // Update is called once per frame
    void Update () {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (running)
        {
            rb.useGravity = true;
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began) && !posession)
                {
                    Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit raycastHit;
                      if (Physics.Raycast(raycast, out raycastHit))
                        {
                            Debug.Log("Something Hit");
                            if (raycastHit.collider.name == "Ball")
                            {
                                posession = true;
                                ball.transform.Translate(player.transform.position + new Vector3(1f, 1f, 1f));
                            }

                      } else if (posession && Input.touchCount > 0) {

                    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    {

                        // getting touch position and marking time when you touch the screen
                        touchTimeStart = Time.time;
                        startPos = Input.GetTouch(0).position;
                    }

                    // if you release your finger
                    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                    {

                        // marking time when you release it
                        touchTimeFinish = Time.time;

                        // calculate swipe time interval 
                        timeInterval = touchTimeFinish - touchTimeStart;

                        // getting release finger position
                        endPos = Input.GetTouch(0).position;

                        // calculating swipe direction in 2D space
                        direction = startPos - endPos;


                        
                        // add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces
                        rb.isKinematic = false;
                        rb.AddForce(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ / timeInterval);

                      

                    }
                }
                      
                        
                      
                

                
                }
        } else
        {
            rb.useGravity = false;
        }
        
    }


    public static void play()
    {
        running = true;

    }

    public static void pause()
    {
        running = false;
    }

    public static void made()
    {
        score += 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    // Use this for initialization
    public GameObject ball;
    public GameObject cam;

    GameObject[] objs = new GameObject[1];
    public static bool run = false;

    public static bool shot = false;

    private bool posession = false;

    public static int score = 0;

    float touchTimeStart, touchTimeFinish, timeInterval;

    static Vector3 hoopPos;

    Vector2 startPos, endPos, direction;

    float throwForceInXandY = 1f;

    float throwForceInZ = 1f;

    // Update is called once per frame
    void Update () {
		if (run)
        {
            if (!posession && !shot)
            {
                spawn();
            } else if (shot && Vector3.Distance(cam.transform.position, objs[0].transform.position) > 8f)
            {
                Destroy(objs[0]);
                shot = false;
                spawn();
            } else
            {
                shoot();
            }
        } else
        {
            Destroy(objs[0]);
            shot = false;
            posession = false;
        }
	}

    public static void play()
    {
        run = true;
    }

    public static void pause()
    {
        run = false;
    }

    void spawn()
    {
        objs[0] = ((GameObject)Instantiate(ball, 
        cam.transform.position, cam.transform.rotation));
        GameObject obj = objs[0];
        objs[0].GetComponent<Rigidbody>().useGravity = false;
        posession = true;
        objs[0].transform.position  = cam.transform.position + cam.transform.forward * .25f;
        objs[0].transform.parent = cam.transform;
    }

    void shoot()
    {
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

            Rigidbody rb = objs[0].transform.GetComponent<Rigidbody>();

            float power = timeInterval * 100f;

            // add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(cam.transform.forward * power);

            posession = false;
            shot = true;
            StartCoroutine(wait());
        }
    }

    public static void setPos(Vector3 pos)
    {
        hoopPos = pos;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(60f);
    }
    public static void made()
    {
        score += 1;
    }
}

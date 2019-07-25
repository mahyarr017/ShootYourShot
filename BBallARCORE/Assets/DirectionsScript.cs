using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DirectionsScript : MonoBehaviour
{
    public static Text msg;

    void Start()
    {
        msg = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        msg.text = "Tap and Hold to Increase Power " +
            "\n\nRelease to Shoot \n\nPress Blue Button to Reset Hoop Position";
    }



}
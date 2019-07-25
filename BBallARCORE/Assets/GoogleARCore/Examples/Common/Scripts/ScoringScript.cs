using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoringScript : MonoBehaviour {

    // Use this for initialization
    public static int score = Shot.score;
    public static Text msg;

	void Start () {
        msg = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        score = Shot.score;
        msg.text = "Score: " + score;
	}
}

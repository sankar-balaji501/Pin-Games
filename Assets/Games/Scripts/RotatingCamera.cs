using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using System;

public class RotatingCamera: MonoBehaviour {

    public GameObject player;
    public Transform target;
    private Vector3 offset;
    private float currCamDir;
    public float lerpSpeed = 5;

    void LateUpdate() {
        GameObject player = GameObject.Find("Player");
        BallController ballController = player.GetComponent<BallController>();
        int dir = ballController.getDirection();
        currCamDir = Mathf.LerpAngle(currCamDir * 90, dir * 90, 2 * Time.deltaTime);
        offset = new Vector3(-20 * Mathf.Sin((dir * 90) * Mathf.Deg2Rad), 30,
                             -20 * Mathf.Cos((dir * 90) * Mathf.Deg2Rad));
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, 
                                          lerpSpeed * Time.deltaTime);
        transform.LookAt(target);
    }
}
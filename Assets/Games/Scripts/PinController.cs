using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour {

    public int value;
	void Start () {
        value = 0;
	}

    public void Update() {
        if (transform.eulerAngles.x > 45 && transform.eulerAngles.x < 315 ||
            transform.eulerAngles.z > 45 && transform.eulerAngles.z < 315)
            value = 1;
    }
}

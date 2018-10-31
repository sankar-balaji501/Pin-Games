using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour {

    public int value;
	void Start () {
        value = 0;
	}

    public void Update() {
        if (transform.eulerAngles.y > 45 && transform.eulerAngles.y < 315)
            value = 1;
    }
}

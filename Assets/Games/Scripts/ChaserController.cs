using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using System;

public class ChaserController : MonoBehaviour {
    float amplitudeY;
    float omegaY;
    float index;
    int offset;
    int i = 0;
    private Vector3 pos;

    // Use this for initialization
    void Start () {
        amplitudeY = UnityEngine.Random.Range(.8f, .9f);
        omegaY = UnityEngine.Random.Range(5.8f, 6.2f);
        offset = UnityEngine.Random.Range(0, 30);
    }

    // Update is called once per frame
    void Update() {
        i++;
        pos = transform.position;
        if (i > offset) {
            index += Time.deltaTime;
            float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * index));
            transform.position = new Vector3(pos.x, y + 2, pos.z);
        }
        
    }
}

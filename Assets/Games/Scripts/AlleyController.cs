using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AlleyController : MonoBehaviour {

    // Create public variables for player speed, and for the Text UI game objects
    public int i = 0;
    public Text countText;
    public Text timeText;
    public Text winText;

    // Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
    private int count;
    private float timeLeft = 10.0f;
    private int win = -1;
    private GameObject[] pins;

    // Use this for initialization
    void Start () {
        count = 0;
        SetCountText();
        winText.text = "";
        pins = GameObject.FindGameObjectsWithTag("Pin");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Quit"))
            Application.Quit();
        if (Input.GetButtonDown("Restart"))
            SceneManager.LoadScene("1.1");
        if (Input.GetButtonDown("Pause"))
            Time.timeScale = 1 - Time.timeScale;
        if (Input.GetButtonDown("Scene1")) {
            SceneManager.LoadScene("1.1");
        }
        if (Input.GetButtonDown("Scene2")) {
            SceneManager.LoadScene("1.2.2");
        }
        SetTimeText();
        UpdateToppledPins();
        SetCountText();
    }

    void UpdateToppledPins() {
        int totalCount = 0;
        foreach (GameObject pin in pins) {
            PinController curr = pin.GetComponent<PinController>();
            if (curr.value == 1) {
                totalCount++;
            }
        }
        count = totalCount;
    }

    void SetTimeText() {
        if (timeLeft > 0 && win == -1) {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) {
                timeText.text = "Time Remaining: 0.00";
                Lose();
            }
            else {
                if (timeLeft < 1)
                    timeText.text = "Time Remaining: 0" + timeLeft.ToString("0.0");
                else
                    timeText.text = "Time Remaining: " + timeLeft.ToString("0.0");
            }
        }
    }

    void SetCountText() {
        countText.text = "Count: " + count.ToString();
        if (count == 6 && win == -1) {
            win = 1;
            winText.text = "You Win! You did it! You're Amazing! Yaaaaaaay!!!";
        }
    }

    void Lose() {
        win = 0;
        winText.text = "You Lost! You're a failure!";
    }
}

using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class BallController : MonoBehaviour {

    public Text debugText, endText;
    public GameObject player;
    private string[] layout;
    private Vector3 pos, newpos;
    private int ballTileX, ballTileZ, flagX, flagZ;
    public int TileMidX { get; private set; }
    public int TileMidZ { get; private set; }
    private bool onTile, paused, gameOver;
    private int dir = 0, maxspeed = 30, accel = 20, turnFlag = 0, savedDir = 0;
    private float speed = 0;
    private Vector3[] dirV = {
         new Vector3(0, 0, 1),  // North
         new Vector3(1, 0, 0),  // East
         new Vector3(0, 0, -1), // South
         new Vector3(-1, 0, 0)  // West

    };
    private int SavedTileX = 0, SavedTileZ = 0;
    private int counter = 0;
    private bool slow;

    // Use this for initialization
    void Start () {
        debugText.text = "";
        endText.text = "";
        GameObject platform = GameObject.Find("Platform");
        PlatformController platformController = platform.GetComponent<PlatformController>();
        layout = platformController.GetLayout();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {
        GameObject chasers = GameObject.Find("Chasers");
        ChasersController chasersController = chasers.GetComponent<ChasersController>();
        CheckEndTile();
        Inputs();
        pos = player.transform.position;

        ballTileX = Mathf.RoundToInt(pos.x / 10);
        ballTileZ = Mathf.RoundToInt(pos.z / 10);

        if (ballTileZ > -1 && ballTileZ < layout.Length &&
            ballTileX > -1 && ballTileX < layout[ballTileZ].Length) {
            if (layout[ballTileZ][ballTileX].CompareTo('*') == 0 ||
                layout[ballTileZ][ballTileX].CompareTo('F') == 0) onTile = true;
            else onTile = false;
        }
        else {
            onTile = false;
        }

        if (speed < maxspeed) {
            speed = speed + accel * Time.deltaTime;
            if (speed > 30) speed = 30;
        }

        TileMidX = ballTileX * 10;
        TileMidZ = ballTileZ * 10;

        newpos = pos + dirV[dir] * speed * Time.deltaTime;
        if ((dir == 0 && newpos.z > TileMidZ) ||
            (dir == 1 && newpos.x > TileMidX) ||
            (dir == 2 && newpos.z < TileMidZ) ||
            (dir == 3 && newpos.x < TileMidX)) {    //If past middle of tile
            if (SavedTileX != ballTileX || SavedTileZ != ballTileZ) {
                SavedTileX = ballTileX;
                SavedTileZ = ballTileZ;
                chasersController.Enqueue("Forward");
            }
        }

        //Actual Movement
        if (gameObject.CompareTag("Player")) { // 0 = No Input
            if (Input.GetButtonDown("Left")) { // 1 = Left
                if (InSecondHalfOfTile()) {
                    speed = (speed / 4);
                }
                else {
                    dir = (dir + 3) % 4;
                    if (!OutOfBounds() && !NextEmpty()) {
                        turnFlag = 1;
                        counter = 0;
                        addToQueue();
                        flagX = ballTileX;
                        flagZ = ballTileZ;
                    }
                    else speed = (speed / 4);
                    dir = (dir + 5) % 4;
                }
            }
            if (Input.GetButtonDown("Right")) { // 2 = Right
                if (InSecondHalfOfTile()) {
                    speed = (speed / 4);
                }
                else {
                    dir = (dir + 5) % 4;
                    if (!OutOfBounds() && !NextEmpty()) {
                        turnFlag = 2;
                        counter = 0;
                        addToQueue();
                        flagX = ballTileX;
                        flagZ = ballTileZ;
                    }
                    else speed = (speed / 4);
                    dir = (dir + 3) % 4;
                }
            }
            if (turnFlag != 0 && (flagX != ballTileX || flagZ != ballTileZ)) {
                turnFlag = 0;
            }
            else if (!OutOfBounds() && !NextEmpty()) {
                if ((dir == 0 || dir == 2) && turnFlag != 0 &&
                    Mathf.RoundToInt(newpos.z * 2) % 20 == 0) {
                    transform.position = new Vector3(newpos.x, 1.5f,
                                                         (Mathf.RoundToInt(newpos.z) / 2) * 2);
                    if (turnFlag == 1) dir = (dir + 3) % 4;
                    if (turnFlag == 2) dir = (dir + 5) % 4;
                    turnFlag = 0;
                }
                else if ((dir == 1 || dir == 3) && turnFlag != 0 &&
                    Mathf.RoundToInt(newpos.x * 2) % 20 == 0) {
                    transform.position = new Vector3((Mathf.RoundToInt(newpos.x) / 2) * 2,
                                                         1.5f, newpos.z);
                    if (turnFlag == 1) dir = (dir + 3) % 4;
                    if (turnFlag == 2) dir = (dir + 5) % 4;
                    turnFlag = 0;
                }
                else {
                    transform.position = newpos;
                }
            }
            else {
                if (dir == 0 || dir == 2)
                    transform.position = new Vector3(newpos.x , 1.5f, 
                                                     (Mathf.RoundToInt(newpos.z) / 2) * 2);
                else
                    transform.position = new Vector3((Mathf.RoundToInt(newpos.x) / 2) * 2, 
                                                     1.5f, newpos.z);
                if (turnFlag == 1) {
                    dir = (dir + 3) % 4;
                    turnFlag = 0;
                }
                else if (turnFlag == 2) {
                    dir = (dir + 5) % 4;
                    turnFlag = 0;
                }
                else speed = (speed / 4);
            }
        }
        DebugInfo();
    }

    void addToQueue() {
        GameObject chasers = GameObject.Find("Chasers");
        ChasersController chasersController = chasers.GetComponent<ChasersController>();
        if (turnFlag == 1) {
            if (chasersController.getSize() > 0)
                chasersController.Dequeue();
            chasersController.Enqueue("Left");
            savedDir = dir;
        }
        if (turnFlag == 2) {
            if (chasersController.getSize() > 0)
                chasersController.Dequeue();
            chasersController.Enqueue("Right");
            savedDir = dir;
        }
    }

    bool OutOfBounds() {
        newpos = player.transform.position + dirV[dir] *
                 speed * Time.deltaTime;
        return !(Mathf.RoundToInt((dirV[dir].z / 2) + newpos.z / 10) > -1 &&
                Mathf.RoundToInt((dirV[dir].z / 2) + newpos.z / 10) < layout.Length &&
                Mathf.RoundToInt((dirV[dir].x / 2) + newpos.x / 10) > -1 &&
                Mathf.RoundToInt((dirV[dir].x / 2) + newpos.x / 10) < layout[ballTileX].Length);
    }

    bool NextEmpty() {
        newpos = player.transform.position + dirV[dir] *
                speed * Time.deltaTime;
        return !(layout[Mathf.RoundToInt((dirV[dir].z / 2) + newpos.z / 10)]
                [Mathf.RoundToInt((dirV[dir].x / 2) + newpos.x / 10)].CompareTo('*') == 0 ||
                layout[Mathf.RoundToInt((dirV[dir].z / 2) + newpos.z / 10)]
                [Mathf.RoundToInt((dirV[dir].x / 2) + newpos.x / 10)].CompareTo('F') == 0);
    }

    bool NextFilled() {
        return !(layout[Mathf.RoundToInt((dirV[dir].z / 2) + newpos.z / 10)]
                [Mathf.RoundToInt((dirV[dir].x / 2) + newpos.x / 10)].CompareTo('!') == 0);
    }

    void Inputs() {
        if (Input.GetButtonDown("Quit"))
            Application.Quit();
        if (Input.GetButtonDown("Restart")) {
            Time.timeScale = 1.0f;
            Debug.Log("Restarting game");
            SceneManager.LoadScene("1.2");
        }
        if (!gameOver && Input.GetButtonDown("Pause")) {
            if (!paused) {
                paused = true;
                Time.timeScale = 0.0f;
            }
            else {
                paused = false;
                if (slow) Time.timeScale = 0.5f;
                else Time.timeScale = 1.0f;
            }
        }
        if (!gameOver && Input.GetButtonDown("Slow")) {
            if (!paused) {
                if (!slow) {
                    slow = true;
                    Time.timeScale = 0.5f;
                }
                else {
                    slow = false;
                    Time.timeScale = 1.0f;
                }
            }
        }
        if (Input.GetButtonDown("Scene1")) {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("1.1");
        }
        if (Input.GetButtonDown("Scene2")) {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("1.2");
        }
    }

    bool InSecondHalfOfTile() {
        if (dir == 0 && pos.z > ballTileZ * 10 && pos.z < (ballTileZ * 10) + 5.0) {
            return true;
        }
        else if (dir == 1 && pos.x > ballTileX * 10 && pos.x < (ballTileX * 10) + 5) {
            return true;
        }
        else if (dir == 2 && pos.z < ballTileZ * 10 && pos.z > (ballTileZ * 10) - 5.0) {
            return true;
        }
        else if (dir == 3 && pos.x < ballTileX * 10 && pos.x > (ballTileX * 10) - 5.0) {
            return true;
        }
        else return false;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Chaser") {
            endText.text = "You were caught! You lose!";
            gameOver = true;
            Time.timeScale = 0.0f;
        }    
    }

    void CheckEndTile() {
        if (layout[ballTileZ][ballTileX].CompareTo('F') == 0) {
            if (pos.x == ballTileX * 10.0f && pos.z == ballTileZ * 10.0f) {
                endText.text = "You made it to the end! You win!";
                gameOver = true;
                Time.timeScale = 0.0f;
            }
        }
    }

    public int getDirection() {
        return dir;
    }

    public Vector3 getPosition() {
        return pos;
    }

    void DebugInfo() {
        debugText.text = "x: " + pos.x.ToString("0.0") +
         ", y:" + pos.y.ToString("0.0") +
         ", z: " + pos.z.ToString("0.0") +
         "\ntile: (" + ballTileX + ", " + ballTileZ + ")";
    }

}

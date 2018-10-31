using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using System;
using System.Collections.Generic;

public class ChasersController : MonoBehaviour {

    public Transform target;
    int maxspeed = 28, accel = 20, dir = 0;
    float speed = 0;
    public Vector3[] dirV = {
         new Vector3(0, 0, 1),  // North
         new Vector3(1, 0, 0),  // East
         new Vector3(0, 0, -1), // South
         new Vector3(-1, 0, 0)  // West

    };
    Vector3 pos, newpos;
    private int TileX, TileZ;
    private float TileMidX, TileMidZ;
    GameObject player;
    Queue path = new Queue();
    private object cmd;
    private int SavedTileX = -99, SavedTileZ = -99;
    int offset = 3, offset2 = 400;
    public bool onTile = true, rage = false;
    private string[] layout;
    int i;

    // Use this for initialization
    void Start () {
        (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
        GameObject platform = GameObject.Find("Platform");
        PlatformController platformController = platform.GetComponent<PlatformController>();
        layout = platformController.GetLayout();

        player = GameObject.Find("Player");
        for (i = 0; i < offset; i++)
            path.Enqueue("Forward");
        i = 0;
        /*       for (int i = 0; i < 7; i++)
                   path.Enqueue("forward");
               path.Enqueue("right");
               for (int i = 0; i < 5; i++)
                   path.Enqueue("forward");
               path.Enqueue("left");
               for (int i = 0; i < 2; i++)
                   path.Enqueue("forward");
               path.Enqueue("right");*/
        //foreach (string str in path) {
        //    Debug.Log(str);
        //}
    }
	
	// Update is called once per frame
	void Update () {
        pos = transform.position;
        transform.LookAt(target);
        updateSpeed();

        TileX = Mathf.RoundToInt(pos.x / 10);
        TileZ = Mathf.RoundToInt(pos.z / 10);
        TileMidX = TileX * 10;
        TileMidZ = TileZ * 10;

        newpos = pos + dirV[dir] * speed * Time.deltaTime;

        i++;
        if (i > offset2) {
            if (TileZ > -1 && TileZ < layout.Length &&
                TileX > -1 && TileX < layout[TileZ].Length) {
                if (layout[TileZ][TileX].CompareTo('*') != 0) onTile = false;
            }
            else {
                onTile = false;
            }
        }

        if (onTile) {
            if ((dir == 0 && newpos.z > TileMidZ) ||
                (dir == 1 && newpos.x > TileMidX) ||
                (dir == 2 && newpos.z < TileMidZ) ||
                (dir == 3 && newpos.x < TileMidX)) {    //If past middle of tile
                if (SavedTileX != TileX || SavedTileZ != TileZ) {
                    SavedTileX = TileX;
                    SavedTileZ = TileZ;
                    if (path.Count > 0) {
                        cmd = path.Dequeue();
                        String str = cmd.ToString();
                        if (str == "Forward") Forward();
                        else if (str == "Left") Left();
                        else if (str == "Right") Right();
                        else throw new Exception("Invalid Queue Element.\n");
                    }
                }
            }
            transform.position = newpos;
        }
        else { //Rage mode enabled
            if (!rage) speed = 5;
            rage = true;
            maxspeed = 27;
            i++;
            if ((i / 30) % 2 == 0)
                (gameObject.GetComponent("Halo") as Behaviour).enabled = true;
            if ((i / 90) % 2 == 1)
                (gameObject.GetComponent("Halo") as Behaviour).enabled = false;
            GameObject player = GameObject.Find("Player");
            BallController ballController = player.GetComponent<BallController>();
            Vector3 ballPos = ballController.getPosition();
            transform.position = Vector3.MoveTowards(pos, ballPos, speed * Time.deltaTime);
        }
    }

    void Forward() {
        Debug.Log("Forward");
    }


    void Left() {
        Debug.Log("Left");
        dir = (dir + 3) % 4;
        newpos = transform.position + dirV[dir] * speed * Time.deltaTime;
    }

    void Right() {
        Debug.Log("Right");
        dir = (dir + 5) % 4;
        newpos = transform.position + dirV[dir] * speed * Time.deltaTime;
    }

    public void Enqueue(String str) {
        path.Enqueue(str);
    }

    public void Dequeue() {
        path.Dequeue();
    }

    public int getSize() {
        return path.Count;
    }


    void updateSpeed() {
        if (speed < maxspeed) {
            speed = speed + accel * Time.deltaTime;
            if (speed > maxspeed) speed = maxspeed;
        }
    }

}

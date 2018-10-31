using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    string[] platformLayout = {
         "* *************",
         "* *   *       *",
         "* *   *   *   *",
         "* *   *   *   *",
         "* *********   *",
         "*     *   *   *",
         "*******   *****",
         "      *        ",
         "****  *        ",
         "*  *  *********",
         "*  *       *  *",
         "*  *****   *  *",
         "*  *   *   *  *",
         "*  *   *   *  *",
         "** *   *   *  *",
         " * *   *   *  *",
         " * *** *****  *",
         " *   *        *",
         " *   *  *******",
         " F   ****      "
    };

    // Use this for initialization
    void Start() {
        GameObject tile = Resources.Load("Tile") as GameObject;
        for (int row = 0; row < platformLayout.Length; row++) {
            for (int col = 0; col < platformLayout[row].Length; col++) {
                if (platformLayout[row][col].CompareTo('*') == 0) {
                     GameObject newTile = Instantiate(tile, new Vector3(10 * col, 0, 10 * row), 
                                          Quaternion.identity, transform);
                }
                else if (platformLayout[row][col].CompareTo('F') == 0) { //Finish Line
                    GameObject finishTile = Resources.Load("FinishTile") as GameObject;
                    GameObject newTile = Instantiate(finishTile, new Vector3(10 * col, 0, 10 * row),
                                         Quaternion.identity, transform);
                }
            }
        }
    }

    public string[] GetLayout() {
        return platformLayout;
    }
}

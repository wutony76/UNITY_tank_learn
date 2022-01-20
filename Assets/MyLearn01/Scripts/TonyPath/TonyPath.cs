using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TonyPath : MonoBehaviour{
    public bool showPath = true;
    public Color pathColor = Color.red;
    public bool loop = true;            //The path is loop or not
    public float Radius = 2.0f;         //The waypoint radius
    public Transform[] wayPoints;       //Waypoints array


    void Reset(){
        //Reset the wayPoint array
        wayPoints = new Transform[GameObject.FindGameObjectsWithTag("WayPoint").Length];
        for (int cnt = 0; cnt < wayPoints.Length; cnt++){
            wayPoints[cnt] = GameObject.Find("WayPoint_" + (cnt + 1).ToString()).transform;
        }
    }

    //Get the length of wayPoint array
    public float Length{
        get{
            return wayPoints.Length;
        }
    }

    //Get the position in the array with its index number
    public Vector3 GetPosition(int index){
        return wayPoints[index].position;
    }

    // 畫線
    void OnDrawGizmos() {
        if (!showPath) return;
        for (int i = 0; i < wayPoints.Length; i++){
            if (i + 1 < wayPoints.Length){
                Debug.DrawLine(wayPoints[i].position, wayPoints[i + 1].position, pathColor);
            }
            else {  /// 最後一個點處理
                if (loop){
                    Debug.DrawLine(wayPoints[i].position, wayPoints[0].position, pathColor);
                }
            }
        }
    }


}

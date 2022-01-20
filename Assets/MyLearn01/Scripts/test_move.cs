using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_move : MonoBehaviour
{


    void MoveByPath()
    {

        Hashtable moveSetting = new Hashtable();
        moveSetting.Add("time", 5.0f);
        moveSetting.Add("easetype", iTween.EaseType.linear);
        moveSetting.Add("looptype", iTween.LoopType.pingPong);
        moveSetting.Add("path", iTweenPath.GetPath("t_path1"));
        

        iTween.MoveTo(this.gameObject, moveSetting);
    }


    // Start is called before the first frame update
    void Start()
    {
        MoveByPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

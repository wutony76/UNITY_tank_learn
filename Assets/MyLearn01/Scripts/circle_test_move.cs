using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _test_move : MonoBehaviour{
    // Start is called before the first frame update

    void MoveByPath(){

        Hashtable moveSetting = new Hashtable();
        moveSetting.Add("time", 5.0f);
        moveSetting.Add("easetype", iTween.EaseType.linear);
        moveSetting.Add("path", iTweenPath.GetPath("TestPath"));

        iTween.MoveTo(this.gameObject, moveSetting);
    }

    void Start(){
        MoveByPath();
    }

    // Update is called once per frame
    void Update(){
        
    }
}

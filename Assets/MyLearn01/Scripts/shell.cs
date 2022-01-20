using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shell : MonoBehaviour{

    public int speed = 15;
    GameObject whoseObj;
    string team;
    float atk = 0;

    string target_tag; //RTank(Clone)


    // Start is called before the first frame update
    Vector3 g_vector3;
    float _y = 0;
    void Start() {
        g_vector3 = Tool.getRotationValue(transform);
        _y = g_vector3.y;
    }

    // Update is called once per frame
    float r_angle = 0;
    float r_delta = 10;
    void Update(){
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        //transform.Rotate(Vector3.down * Time.deltaTime * 5);
        //print("r_angle = " + r_angle);
        if (r_angle <= 60) {
            r_delta += 5;
            r_angle += Time.deltaTime * r_delta;
            transform.rotation = Quaternion.Euler(r_angle, _y, 0);
        }
    }





    void OnTriggerEnter(Collider other){
        GameObject collider_obj = other.gameObject;
        string s_name = collider_obj.name;
        print("shell_trigger_enter  = " + s_name);    
    }
    void OnCollisionStay(Collision collisionInfo){
        GameObject collider_obj = collisionInfo.gameObject;
        string s_name = collider_obj.name;
        print("shell_collider_enter  = " + s_name);
        //Destroy(this.gameObject);

        if (s_name == "RTank(Clone)") {
            var ai_script = collider_obj.GetComponent<tank_ai>();
            if (ai_script != null) {
                //不同隊攻擊成功
                print("shell_team = " + ai_script.getTeam() + "-" + this.team);
                if (ai_script.getTeam() != this.team) {
                    //ai_script.reduceHp(this.atk);
                    ai_script.reduceHp(this.atk, this.whoseObj);
                }
            }
            Destroy(this.gameObject);

        }else if (s_name == "HeroTank") {
            var ai_script = collider_obj.GetComponent<tank_contral>();
            if (ai_script != null){
                //不同隊攻擊成功
                print("shell_team = " + ai_script.getTeam() + "-" + this.team);
                if (ai_script.getTeam() != this.team){
                    //ai_script.reduceHp(this.atk);
                    ai_script.reduceHp(this.atk, this.whoseObj);
                }
            }
            Destroy(this.gameObject);

        }else if (s_name == "Terrain") {
            Destroy(this.gameObject);
        }
        else {
        }
    }


    public void setTeam(string team){
        this.team = team;
    }
    public string getTeam(){
        return this.team;
    }

    public void setAtk(float atk) {
        this.atk = atk;
    }
    public float getAtk() {
        return this.atk;
    }

    public void setWhoseObj( GameObject fatherObj ) {
        this.whoseObj = fatherObj;
    }
    public GameObject getWhoseObj(){
        return this.whoseObj;
    }

}

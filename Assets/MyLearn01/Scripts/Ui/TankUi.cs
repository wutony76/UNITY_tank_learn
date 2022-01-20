using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankUi : MonoBehaviour{


    public GameObject tank_tag;
    public Text lv_text;
    public HealthBar health_script;

    Camera camera;
    Collider tank_collider;
    float tank_height;
    bool isReady = false;
    bool isTank = true;



    // ---ui contral
    public void ui_setLV(int lv){
        if (lv_text) {
            lv_text.text = lv + "";
        }
    }
    public void ui_setHP(float hp) {
        if (health_script) {
            health_script.ui_setHP(hp);
            isReady = true;
        }
    }
    public void ui_reduceHp(float hp) {
        if (health_script){
            health_script.reduceHp(hp);
        }
    }


    // ---ori method
    void Start(){
        camera = Camera.main;
        tank_collider = tank_tag.transform.GetChild(1).gameObject.GetComponent<Collider>();

        float size_y = tank_collider.bounds.size.y;     //得到模型原始高度
        float scal_y = transform.localScale.y;          //得到模型缩放比例
        tank_height = (size_y * scal_y);      //它们的乘积就是高度
    }

    void Update(){
        //print("ai tank_tag =" + tank_tag);
        //print("ai gameObject =" + gameObject);
        //print("ai isReady =" + isReady);
        if (isTank){
            if (tank_tag == null){
                isTank = false;
                Destroy( this.gameObject );
            }
        }

        if (this.gameObject && tank_tag){
            Vector3 world_pos = new Vector3(tank_tag.transform.position.x, tank_tag.transform.position.y + tank_height, tank_tag.transform.position.z);
            Vector2 pos = camera.WorldToScreenPoint(world_pos);

            pos = new Vector2(pos.x, pos.y + 10);
            this.gameObject.transform.position = pos;
        }
    }
}

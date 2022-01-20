using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour{

    // ---tank ori value
    public float maxHealth = 10;
    public float currentHealth = 10;
    public RectTransform health_bar, hurt;

    // ---ui value
    float maxValueUI = 100;  
    float currenValueUI = 100;
    bool is_prepare = false;


    /*
    public GameObject tank_tag;
    Camera camera;
    Collider tank_collider;
    float tank_height;
    */

    void Start(){
        /*
        camera = Camera.main;
        tank_collider = tank_tag.transform.GetChild(1).gameObject.GetComponent<Collider>();

        float size_y = tank_collider.bounds.size.y;     //得到模型原始高度
        float scal_y = transform.localScale.y;          //得到模型缩放比例
        tank_height = (size_y * scal_y);      //它们的乘积就是高度
        */
    }

    void Update(){
        
        //按下H鈕扣血
        if (Input.GetKeyDown(KeyCode.H)){
            //接受傷害
            //currentHealth = currentHealth - 10;
            reduceHp(1);
        }
        

        if(!is_prepare) return;
        currenValueUI = maxValueUI * currentHealth / maxHealth;

        //將綠色血條同步到當前血量長度
        health_bar.sizeDelta = new Vector2(currenValueUI, health_bar.sizeDelta.y);
        //呈現傷害量
        if (hurt.sizeDelta.x > health_bar.sizeDelta.x){
            //讓傷害量(紅色血條)逐漸追上當前血量
            hurt.sizeDelta += new Vector2(-1, 0) * Time.deltaTime * 10;
        }

        /*
        Vector3 world_pos = new Vector3(tank_tag.transform.position.x, tank_tag.transform.position.y + tank_height, tank_tag.transform.position.z);
        Vector2 pos = camera.WorldToScreenPoint(world_pos);
        //pos = new Vector2( pos.x, Screen.height-pos.y );
        pos = new Vector2(pos.x, pos.y + 10);
        this.gameObject.transform.position = pos;
        */
    }

    public void ui_setHP(float hp){
        this.maxHealth = hp;
        this.currentHealth = hp;

        //滿血
        this.currenValueUI = 100;

        is_prepare = true;
    }
    public void reduceHp(float hp) {
        currentHealth = currentHealth - hp;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tank_contral : MonoBehaviour {
    // Start is called before the first frame update

    public GameObject bullet;
    public GameObject clipNd;
    public GameObject bullet_loc;
    public string team;
    public TankUi tank_ui;


    //settings
    int lv = 1;
    int exp = 0;

    float hp = 15;
    float atk = 1;
    int speed = 10;
    int rot_spend = 30;


    int tankState = Tank.NONE;


    float fire_time = 0;
    float fire_block = 0.3f;

    bool isForward = true;
    bool isFire = true;
    bool isExp = false; //false未領取

    void fire() {
        GameObject b = Instantiate(bullet, clipNd.transform.position, this.gameObject.transform.rotation);
        b.transform.parent = bullet_loc.transform;
        b.GetComponent<shell>().setTeam(this.team);
        b.GetComponent<shell>().setWhoseObj(this.gameObject);
        b.GetComponent<shell>().setAtk(this.atk);
        //b.GetComponent<shell>().atk = atk;
        //b.GetComponent <"shell">
    }

    void Start() { }
    // Update is called once per frame
    void Update() {





        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            if (!isForward) isForward = true;

        } else if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
            if (isForward) isForward = false;
        }


        if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.up * Time.deltaTime * rot_spend);

        } else if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.down * Time.deltaTime * rot_spend);

        }


        fire_time += Time.deltaTime;
        if (isFire) {
            if (Input.GetKey(KeyCode.Space)) {
                fire();
                isFire = false;
                fire_time = 0;
            }
        } else {
            if (fire_time >= fire_block) {
                isFire = true;
            }
        }

    }

    void OnTriggerEnter(Collider other) {
        string s_name = other.gameObject.name;
        print("enter_" + s_name);

        if (s_name == "w1" || s_name == "w2" || s_name == "w3" || s_name == "w4"){
            if (isForward) {
                transform.Translate(Vector3.back * Time.deltaTime * 30);
            } else {
                transform.Translate(Vector3.forward * Time.deltaTime * 30);
            }
        }    
    }

    void OnCollisionStay(Collision collisionInfo) {
        string s_name = collisionInfo.gameObject.name;
        if (s_name != "Terrain") {
            print("stay_"+s_name);
        }   
    }




    void ChangeTankState(int state){
        this.tankState = state;
    }



    public void setTeam(string team){
        this.team = team;
    }
    public string getTeam(){
        return this.team;
    }


    public void reduceHp(float _reduce_hp, GameObject whoseObj){
        this.hp -= _reduce_hp;
        tank_ui.ui_reduceHp(_reduce_hp);  // ui -hp
        //print("ai_tank_hp = " + this.hp);
        if (this.hp <= 0){
            if (!isExp){
                isExp = true;

                var ai_script = whoseObj.GetComponent<tank_ai>();
                var hero_script = whoseObj.GetComponent<tank_contral>();
                //print("ai_script = " + ai_script);
                //print("hero_script = " + hero_script);
                if (ai_script) ai_script.addExp(this.lv);     //ai +exp
                if (hero_script) hero_script.addExp(this.lv); //hero +exp
                ChangeTankState(Tank.DIE);
            }
        }
    }




    //+經驗
    public void addExp(int _exp) {
        this.exp += _exp;
        setLv(exp2Lv(this.exp));
    }
    int exp2Lv(int _exp){
        //int newLv = 0;
        int needLv = this.lv + 1;
        //int needExp = (needLv * needLv) + 6 * this.lv;
        int needExp = (1 * this.lv);

        //經驗夠 升級
        if (this.exp >= needExp){
            this.exp = 0;
            Invoke("updateAiValue", 1f);
            print("hero -等級升級");
            return needLv;
        }
        return this.lv;
    }
    void setLv(int _lv){
        this.lv = _lv;
        tank_ui.ui_setLV( this.lv );
    }

    //+屬性
    void updateAiValue(){
        print("ai updateAiValue 升級+屬性");
        float delta = this.lv;
        this.hp += delta * 0.3f;
        this.atk += delta * 0.2f;
    }







}

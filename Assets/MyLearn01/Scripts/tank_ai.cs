using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tank_ai : MonoBehaviour {


    public Material redMat;
    public Material greenMat;

    public GameObject bullet;
    public GameObject clipNd;
    public GameObject bullet_loc;



    TankUi tank_ui;

    bool isRun = false;
    bool isShot = true;
    bool isDie = false;
    bool isSearch = false;
    bool isExp = false; //false未領取

    float standy_time = 0;
    float fire_time = 0;
    float run_time = 0;
    float fire_block = 0.3f;

    int tankState = Tank.NONE;
    GameObject battery_obj = null;



    //conf
    string aiName;
    string team;
    GameObject lockTarget = null;

    int lv = AI.LV;
    float hp = AI.HP;
    float atk = AI.ATK;
    float lerpSpeed = 0.08f;

    int exp = 0;


    public void setUI(TankUi _ui){
        this.tank_ui = _ui;
        //tank_ui.ui_setHP(this.hp);
    }
    public void initTank() {
        print("initTank");
        tank_ui.ui_setHP(this.hp);
    }


    void AiRun() {
        //var rigidbody_scripts = this.gameObject.GetComponent<Rigidbody>();
        //rigidbody_scripts.useGravity = false;
        //rigidbody_scripts.constraints = RigidbodyConstraints.FreezeAll;
        //transform.position = new Vector3(transform.position.x, 0.914f, transform.position.z);
        ChangeTankState(Tank.RUN);
        //ChangeTankState(Tank.STANBY);
    }

    void Start() {
        // ---Battery 
        GameObject tankRander = this.gameObject.transform.GetChild(0).gameObject; //GameObject.Find("TankRenderers");
        int objCount = tankRander.transform.childCount;
        if (objCount > 0){
            for (int i = 0; i < objCount; i++){
                GameObject nd = tankRander.transform.GetChild(i).gameObject;
                if (nd.name == "TankTurret") {
                    battery_obj = nd;
                    break;
                }
            }
        }

        // ---get ui
        //tank_ui.ui_setHP(this.hp);

        // ---delay func
        Invoke("AiRun", 3f);
    }

    void Update() {
        switch (tankState) {
            case Tank.NONE:
                //var rigidbody_scripts = this.gameObject.GetComponent<Rigidbody>();
                //if (rigidbody_scripts == null) ChangeTankState(Tank.RUN);        
                break;


            case Tank.STANBY:
                if(!isSearch) standy_time += Time.deltaTime;
                if (standy_time >= Random.Range(0, 3) ) {
                    isSearch = true;
                    SearchObj();
                }
                break;


            case Tank.RUN:
                //return;
                run_time += Time.deltaTime;
                //print("run_time =" + run_time);
                if (run_time <= Random.Range(1, 4)){
                    if (this.gameObject.GetComponent<PathFollowing>()){
                        this.gameObject.GetComponent<PathFollowing>().ObjRun();
                    }
                }else{
                    print("start  search.");
                    //SearchObj();
                    ChangeTankState(Tank.STANBY);
                }
                break;


            case Tank.LOOKRUN:
                /*
                this.gameObject.transform.LookAt(lockTarget.transform);
                Vector3 obj_rot_vector3 = gameObject.transform.rotation.eulerAngles;
                gameObject.transform.rotation = Quaternion.Euler(0, obj_rot_vector3.y, 0);
                transform.Translate(Vector3.forward * Time.deltaTime * 10);
                */
                if (lockTarget != null){
                    if (this.gameObject.GetComponent<PathFollowing>()){
                        this.gameObject.GetComponent<PathFollowing>().TargetRun(lockTarget);
                    }
                }else{

                    ChangeTankState(Tank.STANBY);
                }
                break;


            case Tank.FIRE:
                if (lockTarget != null) {
                    var ai_sciprt = lockTarget.GetComponent<tank_ai>();
                    if (ai_sciprt.isExp) ChangeTankState(Tank.RUN);

                    if (battery_obj == null) return;
                    battery_obj.transform.LookAt(lockTarget.transform);
                    Vector3 rot_vector3 = battery_obj.transform.rotation.eulerAngles;
                    
                    //print("y :" + rot_vector3.y );
                    battery_obj.transform.rotation = Quaternion.Euler( 0, rot_vector3.y, 0 );

                    fire_time += Time.deltaTime;
                    if (isShot) {
                        Shot();
                        isShot = false;
                        fire_time = 0;
                    } else {
                        if (fire_time >= fire_block){
                            isShot = true;
                        }
                    }
                } else {
                    ChangeTankState(Tank.RUN);
                }
                break;


            case Tank.DIE:
                if (isDie) break;
                isDie = true;
                isShot = false;

                //移除碰撞
                List<BoxCollider> collider_list = new List<BoxCollider>(GetComponents<BoxCollider>());
                if (collider_list.Count > 0) {
                    collider_list.ForEach(delegate (BoxCollider collider){
                        Destroy(collider);
                    });
                }
                Destroy(this.gameObject.GetComponent<Rigidbody>());
                //Destroy(this.gameObject.GetComponent<PathFollowing>());
                //Destroy(this.gameObject.GetComponent<tank_ai>());

                //移除UI
                Destroy(tank_ui.gameObject);
                Invoke("DesAi", 3f);
                break;
        }
    }

    void ChangeTankState(int state) {
        this.tankState = state;
    }

    //設定材質
    public void setMat( Material mat ) {
        GameObject tankRander = this.gameObject.transform.GetChild(0).gameObject; //GameObject.Find("TankRenderers");
        //print("tankRander = " + tankRander.name );
        int objCount = tankRander.transform.childCount;
        if (objCount > 0){
            for (int i = 0; i < objCount; i++){
                GameObject nd = tankRander.transform.GetChild(i).gameObject;
                //print("Tank child nd =" + nd.name);
                Renderer render = nd.GetComponent<Renderer>();
                render.sharedMaterial = mat;
            }
        }
    }
    public void setRed() {
        setMat(redMat);
        setTeam(Team.RED);
    }
    public void setGreen(){
        setMat(greenMat);
        setTeam(Team.GREEN);
    }

    public void setTeam( string team ) {
        this.team = team;
    }
    public string getTeam(){
        return this.team;
    }
    public int getLv() {
        return this.lv;
    }

    public void setButtleLoc( GameObject buttletNd ) {
        this.bullet_loc = buttletNd;
    }


    //+經驗
    public void addExp( int _exp ) {
        this.exp += _exp;
        setLv(exp2Lv(this.exp));
    }
    int exp2Lv(int _exp) {
        //int newLv = 0;
        int needLv = this.lv + 1;
        //int needExp = (needLv * needLv) + 6 * this.lv;
        int needExp = (1*this.lv);

        //經驗夠 升級
        if (this.exp >= needExp) {
            this.exp = 0;
            Invoke("updateAiValue", 1f);
            print("ai -等級升級");
            return needLv;
        } 
        return this.lv;
    }
    void setLv(int _lv) {
        this.lv = _lv;
    }
    //+屬性
    void updateAiValue() {
        print("ai updateAiValue 升級+屬性");
        float delta = this.lv;
        this.hp += delta*0.3f;
        this.atk += delta*0.2f;

        // ++LV
        tank_ui.ui_setLV(this.lv);
        tank_ui.ui_setHP(this.hp);
    }
    //扣血
    public void reduceHp( float _reduce_hp ) {
        this.hp -= _reduce_hp;
        tank_ui.ui_reduceHp(_reduce_hp);  // ui -hp
        //print("ai_tank_hp = "+ this.hp );
        if (this.hp <= 0) {
            ChangeTankState(Tank.DIE);
        }
    }
    public void reduceHp(float _reduce_hp, GameObject whoseObj) {
        this.hp -= _reduce_hp;
        tank_ui.ui_reduceHp(_reduce_hp);  // ui -hp
        //print("ai_tank_hp = " + this.hp);
        if (this.hp <= 0){
            if (!isExp) {
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
    //目標
    void setLockTarget( GameObject target ) {
        this.lockTarget = target;
    }
    //發射
    void Shot() {
        //GameObject b = Instantiate(bullet, clipNd.transform.position, this.gameObject.transform.rotation);
        GameObject b = Instantiate(bullet, clipNd.transform.position, battery_obj.gameObject.transform.rotation);
        //b.transform.parent = bullet_loc.transform;
        b.GetComponent<shell>().setTeam(this.team);
        b.GetComponent<shell>().setWhoseObj(this.gameObject);
        b.GetComponent<shell>().setAtk(this.atk);
    }
    //搜尋
    void SearchObj() {
        int radius = 100;
        Collider[] objs = Physics.OverlapSphere(this.gameObject.transform.position, radius);
        GameObject newTarget = null;
        float dis = 10.0f;
        
        print("-ai SearchObj = " + objs.Length);
        if (objs.Length>0) {
            for(int i=0; i<objs.Length; i++){
                GameObject thisObj = objs[i].gameObject;
                if (thisObj.name == "RTank(Clone)") {
                //if (thisObj.name == "RTank"){
                    var ai_scirpt = thisObj.GetComponent<tank_ai>();
                    print("-ai SearchObj name= " + thisObj.name  + "-" + ai_scirpt.getTeam() + "  m_team = " + this.team );
                    if (ai_scirpt.getTeam() != this.team) {
                        var new_dis = Vector3.Distance(this.gameObject.transform.position, thisObj.transform.position);
                        //print("-ai SearchObj- RTank dis = " + new_dis);

                        if (dis > new_dis) {
                            dis = new_dis;
                            newTarget = thisObj;
                            print("-ai SearchObj set(newTarget)-");

                            setLockTarget(newTarget);
                            ChangeTankState(Tank.LOOKRUN);
                        }
                    }
                }

            }
        }

        if (newTarget == null) {
            print("AI standy change run.");
            run_time = 0;
            ChangeTankState(Tank.RUN);
        }
        isSearch = false;
        standy_time = 0;
        run_time = 0;
    }
    //死亡
    void DesAi() {
        GameObject mainObj = GameObject.Find("Main");
        if (this.team == Team.RED) {
            mainObj.GetComponent<main>().aiRedCount--;
        } else if (this.team == Team.GREEN) {
            mainObj.GetComponent<main>().aiGreenCount--;
        }
        Destroy(this.gameObject);
    }








    void OnTriggerEnter(Collider other) {
        GameObject collider_obj = other.gameObject;
        string s_name = collider_obj.name;
        print("ai_trigger_enter  = " + s_name);

        if (s_name == "RTank(Clone)") {
            var ai_script = collider_obj.GetComponent<tank_ai>();
            if (ai_script != null) {
                string teamState = ai_script.getTeam();
                print(this.team + " TeamState  = " + teamState);

                if (teamState != this.team) {
                    //is enemty
                    setLockTarget(collider_obj);
                    ChangeTankState(Tank.FIRE);
                }
            }

        } else {
        }
    }
    void OnCollisionStay(Collision collisionInfo){
        GameObject collider_obj = collisionInfo.gameObject;
        string s_name = collider_obj.name;
        //print("ai_collision_stay_" + s_name);

        if (s_name == "RTank(Clone)") {
            var ai_script = collider_obj.GetComponent<tank_ai>();
            if (ai_script != null){
                string teamState = ai_script.getTeam();
                //print(this.team + " TeamState  = " + teamState);

                if (teamState == this.team && lockTarget == null){
                    //ChangeTankState(Tank.STANBY);
                    ChangeTankState(Tank.RUN);
                }
            }


        }
    }
    

}


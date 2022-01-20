using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;



static class Const{
    public const int GAME_PREPARE = 0x00000010;

    public const int GAME_START = 0x00000100;
    public const int PAUS = 0x00000101;
    public const int END = 0x00000102;
}

static class Tank{
    public const int NONE = 0x00004000;
    public const int STANBY = 0x00004001;
    public const int RUN = 0x00004002;
    public const int LOOKRUN = 0x00004003;



    public const int FIRE = 0x00004006;
    public const int DIE = 0x00004007;

    //public const int FIRE = 0x00000102;
}

static class Team {
    public const string RED = "red";
    public const string GREEN = "green";
}





public class main : MonoBehaviour{
    public GameObject spawnNd;
    public GameObject battleNd;
    public GameObject buttleLocNd;
    public GameObject tankUiNd;
    public GameObject car;
    public GameObject tankUiPrefad;

    float clock;    //---timer
    int spawnChildCount = 0;
    public int maxAiCount = 20;
    public int aiRedCount = 0;
    public int aiGreenCount = 0;
    int gameState = Const.GAME_PREPARE;
    List<GameObject> spawnList = new List<GameObject>();

    void setGameState(int state){
        gameState = state;
    }

    public TankUi spawnTankUI(GameObject tankObj) {
        TankUi tank_ui;
        GameObject ui = Instantiate(tankUiPrefad);
        ui.transform.parent = tankUiNd.transform;
        tank_ui = ui.GetComponent<TankUi>();
        tank_ui.tank_tag = tankObj;
        return tank_ui;
    }

    void SpwanCar() {
        //int r_index = Random.Range(0, spawnChildCount - 1);
        //print("r_index =" + r_index);
        //GameObject sp_nd = spawnList[r_index];

        for ( int i = 0; i<spawnList.Count; i++ ) {
            // --add enemy
            GameObject sp_nd = spawnList[i];
            GameObject c_car = Instantiate(car, sp_nd.transform.position, sp_nd.transform.rotation);
            c_car.transform.parent = battleNd.transform;
            //print("sp_nd =" + sp_nd.name);


            // --select path
            int p_count = sp_nd.transform.childCount;
            if (p_count > 0) {
                int r_index = Random.Range(0, p_count - 1);
                GameObject nd = sp_nd.transform.GetChild(r_index).gameObject;
                //print("ND_name =" + nd.name);
                c_car.GetComponent<PathFollowing>().path = nd.GetComponent<TonyPath>();
                tank_ai tankScript = c_car.GetComponent<tank_ai>();
                //tankScript.setButtleLoc = buttleLocNd;
                //print("ND_name =" + sp_nd.name);
                switch (sp_nd.name) {
                    case "p1":
                        if (aiRedCount >= maxAiCount) {
                            Destroy(c_car);
                            break;
                        }
                        tankScript.setRed();
                        aiRedCount++;
                        break;
                    default:
                        break;

                    case "p2":
                        if (aiRedCount >= maxAiCount) {
                            Destroy(c_car);
                            break;
                        }
                        tankScript.setRed();
                        aiRedCount++;
                        break;

                    case "p3":
                        if (aiGreenCount >= maxAiCount){
                            Destroy(c_car);
                            break;
                        }
                        tankScript.setGreen();
                        aiGreenCount++;
                        break;
                    case "p4":
                        if (aiGreenCount >= maxAiCount){
                            Destroy(c_car);
                            break;
                        }
                        tankScript.setGreen();
                        aiGreenCount++;
                        break;
                }
            }

            // ---set tank ui
            c_car.GetComponent<tank_ai>().setUI(spawnTankUI(c_car));

            Thread.Sleep(100);
            InitTankAI(c_car);
            //c_car.GetComponent<tank_ai>().initHP();
            //Invoke("InitTankAI", 0.1f);
        }
    }


    // Game state
    void InitGame() {
        setGameState(Const.GAME_PREPARE);
        SpwanCar();
    }
    void InitTankAI(GameObject obj){
        obj.GetComponent<tank_ai>().initTank();
    }
    


    // Start
    void Start(){
        //-set conf
        spawnChildCount = spawnNd.transform.childCount;
        if (spawnChildCount > 0) {
            for (int i = 0; i < spawnChildCount; i++){
                GameObject nd = spawnNd.transform.GetChild(i).gameObject;
                spawnList.Add(nd);
                //print("spawnList add_nd =" + nd.name);
            }
            //print("spawnList =" + spawnList);
        }
        
        //-init game.
        InitGame();
    }


    // Update is called once per frame
    void Update() {
        clock += Time.deltaTime;
        switch (gameState) {
            case Const.GAME_PREPARE:
                //print("== PREPARE STATE ==");
                if (clock >= 15) {
                    //SpwanCar();
                    clock = 0;
                    setGameState(Const.GAME_START);
                }
                break;

            case Const.GAME_START:
                //print("== START STATE ==");
                //---start.---
                if (clock >= 15){
                    SpwanCar();
                    clock = 0;
                }
                //---end.---
                break;
        }
    }

}

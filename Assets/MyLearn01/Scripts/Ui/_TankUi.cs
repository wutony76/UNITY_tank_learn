using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTankUi : MonoBehaviour {


    private Camera camera;
    private string name = "我是雨松MOMO";
    public Collider tankCollider;


    float npcHeight;
    public Texture2D blood_red;
    public Texture2D blood_black;

    private int HP = 100;


    void Start(){
        camera = Camera.main;


        //得到模型原始高度
        float size_y = tankCollider.bounds.size.y;//collider.bounds.size.y;
        print("size_y=" + size_y);
        //得到模型缩放比例
        float scal_y = transform.localScale.y;
        //它们的乘积就是高度
        npcHeight = (size_y * scal_y);
    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnGUI(){
        //得到NPC头顶在3D世界中的坐标
        //默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + npcHeight, transform.position.z);
        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
        Vector2 position = camera.WorldToScreenPoint(worldPosition);
        //得到真实NPC头顶的2D坐标
        position = new Vector2(position.x, Screen.height - position.y);
        //注解2
        //计算出血条的宽高
        Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood_red));

        //通过血值计算红色血条显示区域
        int blood_width = blood_red.width * HP / 100;

        float ui_height = 1;

        //先绘制黑色血条
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - (bloodSize.y / 2), bloodSize.x/2, ui_height), blood_black);
        //在绘制红色血条
        GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 2), position.y - (bloodSize.y / 2), blood_width/2, ui_height), blood_red);

        //注解3
        //计算NPC名称的宽高
        Vector2 nameSize = GUI.skin.label.CalcSize(new GUIContent(name));
        //设置显示颜色为黄色
        GUI.color = Color.yellow;
        //绘制NPC名称
        GUI.Label(new Rect(position.x - (nameSize.x / 2), position.y - nameSize.y - bloodSize.y, nameSize.x, nameSize.y), name);

    }
}

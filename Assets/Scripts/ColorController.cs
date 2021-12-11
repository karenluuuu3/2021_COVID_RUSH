using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    public Material red;
    public Material green;

    public MeshRenderer Cube0;
    public MeshRenderer Cube2;
    public MeshRenderer Cube3;
    public MeshRenderer Cube4;
    public MeshRenderer Cube5;
    public MeshRenderer Cube6;
    public MeshRenderer Cube7;
    public MeshRenderer Cube8;
    public MeshRenderer Cube9;
    public MeshRenderer Cube10;
    public MeshRenderer Cube11;
    public MeshRenderer Cube12;
    public MeshRenderer Cube13;
    public MeshRenderer Cube14;
    public MeshRenderer Cube15;
    public MeshRenderer Cube16;
    public MeshRenderer Cube17;
    public MeshRenderer Cube18;
    public MeshRenderer Cube19;
    public MeshRenderer Cube20;
    public MeshRenderer Cube21;
    public MeshRenderer Cube22;
    public MeshRenderer Cube23;
    public MeshRenderer Cube24;
    public MeshRenderer Cube25;
    public MeshRenderer Cube26;
    public MeshRenderer Cube27;
    public MeshRenderer Cube28;
    public MeshRenderer Cube29;
    public MeshRenderer Cube30;
    public MeshRenderer Cube31;
    public MeshRenderer Cube32;
    public MeshRenderer Cube33;
    public MeshRenderer Cube34;
    public MeshRenderer Cube35;
    public MeshRenderer Cube36;
    public MeshRenderer Cube37;
    public MeshRenderer Cube38;
    public MeshRenderer Cube39;
    public MeshRenderer Cube40;

    private int index;// 選方塊
    private string str;// 方塊名稱


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("pressedM");
            Cube14.material = green;


            /*
            for (int i = 0; i < 40; i++)// 隨機取40個換顏色
            {
                index = Random.Range(0, 80);


                // str = "Cube" + index;

                // c = MeshRenderer str;


                MeshRenderer c = mesh_array[index];

                if (c.material == green)
                {
                    c.material = red;
                }

                if (c.material == red)
                {
                    c.material = green;
                }
            }
            */
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float MoveSpeed = 1000f;       //移動速度
    public float RotateSpeed = 20f;     //旋轉速度
    public Rigidbody PlayerRigidbody;
    public bool jumpState = false;
    void Start()
    {
        PlayerRigidbody = this.GetComponent<Rigidbody>();
    }

    void PlayerMove(){
        if (PlayerRigidbody.velocity.y ==0){
            jumpState = false;
        }
         if (Input.GetKeyDown(KeyCode.Space) && jumpState == false)//跳躍
        {
          PlayerRigidbody.velocity = new Vector3(0, 25, 0);
          jumpState = true;
        }

        //獲取WSAD鍵
        float h = -1*Input.GetAxis("Horizontal");    //左右
        float v = -1*Input.GetAxis("Vertical");    //前後

        Vector3 GlobalPos = this.transform.position;//物體所處的世界座標
         Vector3 GlobalDirectionForward = this.transform.TransformDirection(Vector3.forward);
        Vector3 ForwardDirection = v * GlobalDirectionForward;//物體前後移動的方向
        //求出相對於物體右方方向的世界方向
        Vector3 GlobalDirectionRight = this.transform.TransformDirection(Vector3.right);
        Vector3 RightDirection = h * GlobalDirectionRight;//物體左右移動的方向

        //再將前後左右需要移動的方向都加起來 得出 最終要移動的方向
        Vector3 MainDirection = ForwardDirection + RightDirection;

        //通過給剛體的 指定的方向 施加速度，得以控制角色運動
        PlayerRigidbody.velocity = new Vector3(
            MoveSpeed * MainDirection.x, 
            PlayerRigidbody.velocity.y, 
            MoveSpeed * MainDirection.z
        );

        //另外，一般第三人稱角色控制的遊戲，可通過滑鼠左右移動控制旋轉
        float x =  RotateSpeed * Input.GetAxis("Mouse X");
            this.transform.rotation = Quaternion.Euler(
            this.transform.rotation.eulerAngles +
            Quaternion.AngleAxis(x, Vector3.up).eulerAngles
        );

    }

    void FixedUpdate()
    {
       PlayerMove();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

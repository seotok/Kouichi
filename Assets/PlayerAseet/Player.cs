using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Player : MonoBehaviour
{
    //Animatorを入れる変数
    private Animator animator;

    //ユニティちゃんの位置を入れる
    Vector3 playerPos;

    public float speed = 15.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    float inputHorizontal;
    float inputVertical;

    [SerializeField] float smooth = 10f;

    void Start()

    {

    }



    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

     
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

            moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (Input.GetKey("e"))
        {

        }
        else if (Input.GetKey("q"))
        {

        }
        else
        {           
            if (Input.GetKey("d"))
            {
                
            }
            else if (Input.GetKey("a"))
            {
                
            }
            else if (Input.GetKey("s"))
            {

            }
            // キャラクターの向きを進行方向に
            else if (Input.GetKey("w"))
            {
                // カメラの方向から、X-Z平面の単位ベクトルを取得
                Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

                // 方向キーの入力値とカメラの向きから、移動方向を決定
                Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

                if (moveForward != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(moveForward);
                }
            }
        }


    }
}

/*

 */

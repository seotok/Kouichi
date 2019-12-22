using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    float viewAngle;//エディター上で60を入力
    float _inputX, _inputY;

    void Update()
    {
        _inputX = Input.GetAxis("Mouse X");
        _inputY = Input.GetAxis("Mouse Y");

        Rotate(_inputX, _inputY, viewAngle);
    }

    void Rotate(float _inputX, float _inputY, float limit)
    {
        float maxLimit = limit, minLimit = 360 - maxLimit;
        //X軸回転
        var localAngle = transform.localEulerAngles;
        localAngle.x += _inputY;
        if (localAngle.x > maxLimit && localAngle.x < 180)
            localAngle.x = maxLimit;
        if (localAngle.x < minLimit && localAngle.x > 180)
            localAngle.x = minLimit;
        transform.localEulerAngles = localAngle;
        //Y軸回転
        var angle = transform.eulerAngles;
        angle.y += _inputX;
        transform.eulerAngles = angle;
    }
}

/*
 //プレイヤーを変数に格納
    public GameObject Player;

    //回転させるスピード
    public float rotateSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //回転角度
        float angleH = Input.GetAxis("Mouse X") * rotateSpeed;
        float angleV = Input.GetAxis("Mouse Y") * rotateSpeed;

        //プレイヤーの位置情報
        Vector3 playerPos = Player.transform.position;

        //カメラを回転させる
        transform.RotateAround(playerPos, Vector3.up, angleH);
        transform.RotateAround(playerPos, Vector3.right, angleV);

    }
*/


/* 12/16時点のカメラ
 * 
 [SerializeField]
    float viewAngle;//エディター上で60を入力
    float _inputX, _inputY;

    void Update()
    {
        _inputX = Input.GetAxis("Mouse X");
        _inputY = Input.GetAxis("Mouse Y");

        Rotate(_inputX, _inputY, viewAngle);
    }

    void Rotate(float _inputX, float _inputY, float limit)
    {
        float maxLimit = limit, minLimit = 360 - maxLimit;
        //X軸回転
        var localAngle = transform.localEulerAngles;
        localAngle.x += _inputY;
        if (localAngle.x > maxLimit && localAngle.x < 180)
            localAngle.x = maxLimit;
        if (localAngle.x < minLimit && localAngle.x > 180)
            localAngle.x = minLimit;
        transform.localEulerAngles = localAngle;
        //Y軸回転
        var angle = transform.eulerAngles;
        angle.y += _inputX;
        transform.eulerAngles = angle;
    }
*/
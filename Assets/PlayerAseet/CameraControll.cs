using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{

    private GameObject mainCamera;      //メインカメラ格納用
    private GameObject subCamera1;       //サブカメラ1格納用 
    private GameObject subCamera2;       //サブカメラ2格納用


    //呼び出し時に実行される関数
    void Start()
    {
        //メインカメラとサブカメラをそれぞれ取得
        mainCamera = GameObject.Find("MainCamera");
        subCamera1 = GameObject.Find("SubCamera1");
        subCamera2 = GameObject.Find("SubCamera2");

        //サブカメラを非アクティブにする
        subCamera1.SetActive(false);
        subCamera2.SetActive(false);
    }


    //単位時間ごとに実行される関数
    void Update()
    {
        //スペースキーが押されている間、サブカメラをアクティブにする
        if (Input.GetKey("e"))
        {
            //サブカメラ1をアクティブに設定
            mainCamera.SetActive(false);
            subCamera1.SetActive(true);
            subCamera2.SetActive(false);
        }
        else if (Input.GetKey("q")) //スペースキーが押されている間、サブカメラをアクティブにする
        {
            //サブカメラ2をアクティブに設定
            mainCamera.SetActive(false);
            subCamera1.SetActive(false);
            subCamera2.SetActive(true);
        }
        else
        {
            //メインカメラをアクティブに設定
            subCamera1.SetActive(false);
            subCamera2.SetActive(false);
            mainCamera.SetActive(true);
        }
    }
}

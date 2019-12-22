using UnityEngine;
using MLAgents;
using System.Collections.Generic;

public class RunnerAgent : Agent
{
  [SerializeField]
  Param param;

  public float limY = 1.0f;
  public int hitcount = 0;

  Rigidbody rBody;

  //public Transform EscapeFrom;
  List<GameObject> EscapeFrom = new List<GameObject>();

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "chaser"
      || collision.gameObject.tag == "boids")
    {
      hitcount++;
    }
  }

  void Start()
  {
    MakeChaserList();
    rBody = GetComponent<Rigidbody>();
  }

  Vector3 ChaserMeanPos()
  {
    Vector3 meanpos = new Vector3(0, 0, 0);
    int count = 0;
    foreach (GameObject chaser in EscapeFrom)
    {
      meanpos += chaser.transform.position;
      count++;
    }
    meanpos /= count;
    return meanpos;
  }

  void MakeChaserList()
  {
    EscapeFrom = new List<GameObject>();
    foreach (GameObject chaser in GameObject.FindGameObjectsWithTag("chaser"))
    {
      EscapeFrom.Add(chaser);
    }
  }

  public override void AgentReset()
  {
    MakeChaserList();

    if (this.transform.position.y < limY || this.transform.position.y > param.fieldsize/2 ||
        this.transform.position.x > param.fieldsize / 2 || this.transform.position.x < -param.fieldsize / 2 ||
        this.transform.position.z > param.fieldsize / 2 || this.transform.position.z < -param.fieldsize / 2)
    {
      //回転加速度と加速度のリセット
      this.rBody.angularVelocity = Vector3.zero;
      this.rBody.velocity = Vector3.zero;
      //エージェントを初期位置に戻す
      this.transform.position 
        = new Vector3(Random.Range(-param.fieldsize / 8, param.fieldsize / 8),
                      Random.Range(limY, param.fieldsize / 8),
                      Random.Range(-param.fieldsize / 8, param.fieldsize / 8));
    }
    //ターゲット再配置
    foreach (GameObject chaser in EscapeFrom)
    {
      chaser.transform.position
        = new Vector3(Random.Range(-param.fieldsize / 8, param.fieldsize / 8),
                      Random.Range(limY, param.fieldsize / 8),
                      Random.Range(-param.fieldsize / 8, param.fieldsize / 8));
      chaser.GetComponent<RollerAgent>().BoidsRemover();
      chaser.GetComponent<RollerAgent>().BoidsMaker();
    }
  }

  public override void InitializeAgent()
  {
    MakeChaserList();
  }

  ////////////////////

  public override void CollectObservations()
  {
    // MakeChaserList();
    // ターゲットとエージェントの位置
    AddVectorObs(ChaserMeanPos());
    //AddVectorObs(EscapeFrom.position);
    AddVectorObs(this.transform.position);

    //エージェントの速度
    AddVectorObs(rBody.velocity.x);
    AddVectorObs(rBody.velocity.y);
    AddVectorObs(rBody.velocity.z);
  }

  //rigidB.AddForce(Vector3.up *jspeed);
  //rigidB.velocity = Vector3.ClampMagnitude(rigidB.velocity, maxpush);

  public float speed = 1000;
  public override void AgentAction(float[] vectorAction, string textAction)
  {
    //行動
    Vector3 controlSignal = Vector3.zero;
    controlSignal.x = vectorAction[0];
    controlSignal.y = vectorAction[1];
    controlSignal.z = vectorAction[2];
    rBody.AddForce(controlSignal * speed);

    if (param.learningmode)
    {
      //報酬
      //ボール（エージェント）が動いた距離から箱（ターゲット）への距離を取得

      foreach (GameObject boid in GameObject.FindGameObjectsWithTag("boids"))
      {
        float distanceToTarget = Vector3.Distance(this.transform.position, boid.transform.position);

        //箱（ターゲット）に到達した場合
        if (distanceToTarget < 1.42f)
        {
          //報酬を与え完了
          SetReward(-1.0f);
          Done();
        }
      }
      


      if (Vector3.Distance(this.transform.position, ChaserMeanPos()) > 4.42f)
      {
        //報酬を与え完了
        SetReward(1.0f);
      }

      //床から落ちた場合
      if (this.transform.position.y < limY || this.transform.position.y > param.fieldsize / 2 ||
          this.transform.position.x > param.fieldsize / 2 || this.transform.position.x < -param.fieldsize / 2 ||
          this.transform.position.z > param.fieldsize / 2 || this.transform.position.z < -param.fieldsize / 2)
      {
        SetReward(-1.0f);
        Done();
      }
    }
  }

  public override float[] Heuristic()
  {
    var action = new float[3];
    action[0] = Input.GetAxis("Horizontal");
    action[1] = Input.GetAxis("Jump");
    action[2] = Input.GetAxis("Vertical");
    return action;
  }

}

    

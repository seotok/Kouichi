using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MLAgents;

public class RollerAgent : Agent
{
  public Param param;

  public BoidParam boidparam;

  public float limY = 1.0f;

  public int hitcount = 0;

  Rigidbody rBody;

  [SerializeField]
  public int BoidsCount = 100;

  [SerializeField]
  public GameObject BoidPrefab;

  public int boidsNowNum = 0;

  //List<Boid> boids_ = new List<Boid>();

  /*
  public ReadOnlyCollection<Boid> boids
  {
    get { return boids_.AsReadOnly(); }
  }
  */

  void AddBoids()
  {
    var go = Instantiate(BoidPrefab, Random.insideUnitSphere+transform.position, Random.rotation);
    go.transform.SetParent(this.transform.parent);
    var boid = go.GetComponent<Boid>();
    boid.param = boidparam;
    boid.Target = gameObject;

    boidsNowNum++;
  }

  /*
  void RemoveBoids()
  {
    if (boidsNowNum == 0) return;

    var lastIndex = boids_.Count - 1;
    var agent = boids_[lastIndex];
    Destroy(agent.gameObject);
    boids_.RemoveAt(lastIndex);
  }
  */
  int Removeboid(Boid boid)
  {
    if (boid.Target != gameObject) return 0;
    boidsNowNum--;
    Destroy(boid.gameObject);
    return 1;
  }

  public void BoidsMaker()
  {
    while (boidsNowNum < BoidsCount)
      AddBoids();
    foreach (GameObject boid in GameObject.FindGameObjectsWithTag("boids"))
    {
      if (boidsNowNum <= BoidsCount) break;
      Removeboid(boid.GetComponent<Boid>());
    }
  }

  public void BoidsRemover()
  {
    foreach (GameObject boid in GameObject.FindGameObjectsWithTag("boids"))
    {
      if (boidsNowNum <= 0) break;
      Removeboid(boid.GetComponent<Boid>());
    }
  }

  void Start()
  {
    rBody = GetComponent<Rigidbody>();
    BoidsMaker();
  }

  public Transform Target;

  public override void AgentReset()
  {
    if (this.transform.position.y < limY || this.transform.position.y > param.fieldsize / 2 ||
        this.transform.position.x > param.fieldsize / 2 || this.transform.position.x < -param.fieldsize / 2 ||
        this.transform.position.z > param.fieldsize / 2 || this.transform.position.z < -param.fieldsize / 2)
    {
      foreach (GameObject chaser in GameObject.FindGameObjectsWithTag("chaser"))
      {
        chaser.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        chaser.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //エージェントを初期位置に戻す
        chaser.transform.position
          = new Vector3(Random.Range(-param.fieldsize / 8, param.fieldsize / 8),
                      Random.Range(limY, param.fieldsize / 8),
                      Random.Range(-param.fieldsize / 8, param.fieldsize / 8));
        chaser.GetComponent<RollerAgent>().BoidsRemover();
        chaser.GetComponent<RollerAgent>().BoidsMaker();
      }
      /*
      //回転加速度と加速度のリセット
      this.rBody.angularVelocity = Vector3.zero;
      this.rBody.velocity = Vector3.zero;
      //エージェントを初期位置に戻す
      this.transform.position = new Vector3(0, 5.0f, 0);
      */
    }
    //ターゲット再配置
    Target.position = new Vector3(Random.Range(-param.fieldsize / 8, param.fieldsize / 8),
                                  Random.Range(limY, param.fieldsize / 8),
                                  Random.Range(-param.fieldsize / 8, param.fieldsize / 8));

    BoidsRemover();
    BoidsMaker();
  }


  ////////////////////

  public override void CollectObservations()
  {
    // ターゲットとエージェントの位置
    AddVectorObs(Target.position);
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
      foreach (GameObject boid in GameObject.FindGameObjectsWithTag("boids"))
      {
        if (boid.GetComponent<Boid>().Target != gameObject) continue;
        float distanceToTarget = Vector3.Distance(boid.transform.position, Target.position);

        //箱（ターゲット）に到達した場合
        if (distanceToTarget < 1.42f)
        {
          //報酬を与え完了
          SetReward(1.0f);
          Done();
        }
      }
      /*
      //ボール（エージェント）が動いた距離から箱（ターゲット）への距離を取得
      float distanceToTarget = Vector3.Distance(this.transform.position, Target.position);

      //箱（ターゲット）に到達した場合
      if (distanceToTarget < 1.42f)
      {
        //報酬を与え完了
        SetReward(1.0f);
        Done();
      }
      */

      //床から落ちた場合
      if (this.transform.position.y < 1.0 || this.transform.position.y > param.fieldsize / 2 ||
          this.transform.position.x > param.fieldsize / 2 || this.transform.position.x < -param.fieldsize / 2 ||
          this.transform.position.z > param.fieldsize / 2 || this.transform.position.z < -param.fieldsize / 2)
      {
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
  
    

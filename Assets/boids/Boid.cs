using UnityEngine;
using System.Collections.Generic;


public class Boid : MonoBehaviour
{
  //public Simulation simulation { get; set; }
  public BoidParam param { get; set; }
  public Vector3 pos { get; private set; }
  public Vector3 velocity { get; private set; }
  public GameObject Target;
  Vector3 accel = Vector3.zero;
  List<Boid> neighbors = new List<Boid>();
  List<GameObject> terrains = new List<GameObject>();

  private Rigidbody myrigid;
  
  void suicide()
  {
    Target.GetComponent<RollerAgent>().boidsNowNum--;
    //Destroy(gameObject);
  }

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      Target.GetComponent<RollerAgent>().hitcount++;
      if (param.kamikaze) suicide();
    }
    else if(collision.gameObject.tag == "terrain" && param.fragile)
    {
      suicide();
    }
  }
  
  void Start()
  {
    pos = transform.position;
    velocity = transform.forward * param.initSpeed;
    //transform.parent = null;
    myrigid = this.GetComponent<Rigidbody>();
  }

  void Update()
  {
    UpdateNeighbors();
    UpdateWalls();
    UpdateTerrainForce();
    UpdateSeparation();
    UpdateAlignment();
    UpdateCohesion();
    //UpdateTargetForce();
    UpdateMove();
  }

  void UpdateNeighbors()  // 近隣の個体を検出
  {
    neighbors.Clear();
    terrains.Clear();

    var prodThresh = Mathf.Cos(param.neighborFov * Mathf.Deg2Rad);
    var distThresh = param.neighborDistance;
    var terrainprodThresh = Mathf.Cos(param.terrainFov * Mathf.Deg2Rad);
    var terraindistThresh = param.terrainDistance;

    foreach (var other in GameObject.FindGameObjectsWithTag("boids"))
    {
      if (other == this) continue;

      var to = other.transform.position - pos;
      var dist = to.magnitude;
      if (dist < distThresh)
      {
        var dir = to.normalized;
        var fwd = velocity.normalized;
        var prod = Vector3.Dot(fwd, dir);
        if (prod > prodThresh)
        {
          neighbors.Add(other.GetComponent<Boid>());
        }
      }
    }

    foreach (var other in GameObject.FindGameObjectsWithTag("terrain"))
    {
      var to = new Vector3(0, 0, 0);
      if (other.GetComponent<BoxCollider>())
      {
        to = other.GetComponent<BoxCollider>().ClosestPoint(pos) - pos;
      }
      else if (other.GetComponent<MeshCollider>())
      {
        to = other.GetComponent<MeshCollider>().ClosestPoint(pos) - pos;
      }
      
      var dist = to.magnitude;
      if (dist < terraindistThresh)
      {
        var dir = to.normalized;
        var fwd = velocity.normalized;
        var prod = Vector3.Dot(fwd, dir);
        if (prod > terrainprodThresh)
        {
          terrains.Add(other);
        }
      }
    }
  }
  void UpdateWalls()  // 壁から離れる
  {

    var scale = param.wallScale * 0.5f;
    accel +=
        CalcAccelAgainstWall(-scale - pos.x, Vector3.right) +
        CalcAccelAgainstWall(-scale - pos.y, Vector3.up) +
        CalcAccelAgainstWall(-scale - pos.z, Vector3.forward) +
        CalcAccelAgainstWall(+scale - pos.x, Vector3.left) +
        CalcAccelAgainstWall(+scale - pos.y, Vector3.down) +
        CalcAccelAgainstWall(+scale - pos.z, Vector3.back);
  }

  Vector3 CalcAccelAgainstWall(float distance, Vector3 dir)
  {
    if (distance < param.wallDistance)
    {
      return dir * (param.wallWeight / Mathf.Abs(distance / param.wallDistance));
    }
    return Vector3.zero;
  }

  void UpdateSeparation()  // 近隣の個体から離れる
  {
    if (neighbors.Count == 0) return;

    Vector3 force = Vector3.zero;
    foreach (var neighbor in neighbors)
    {
      force += (pos - neighbor.pos).normalized;
    }
    force /= neighbors.Count;

    accel += force * param.separationWeight;
  }

  void UpdateAlignment()  // 近隣の個体の速度ベクトルに倣う
  {
    if (neighbors.Count == 0) return;

    var averageVelocity = Vector3.zero;
    foreach (var neighbor in neighbors)
    {
      averageVelocity += neighbor.velocity;
    }
    averageVelocity /= neighbors.Count;

    accel += (averageVelocity - velocity) * param.alignmentWeight * param.turbulence;
  }

  void UpdateCohesion()  // 近隣の個体集団の中心に向かおうとする
  {
    if (neighbors.Count == 0) return;

    var averagePos = Vector3.zero;

    Vector3 targetpos = Target.transform.position;

    foreach (var neighbor in neighbors)
    {
      averagePos += neighbor.pos;
    }
    averagePos /= neighbors.Count;
    averagePos = averagePos * (1.0f - param.targetratio) + targetpos * param.targetratio;
    accel += (averagePos - pos) * param.cohesionWeight * param.turbulence;
  }
  /*
  void UpdateTargetForce()  // ターゲットに反発する
  {
    Vector3 targetpos = simulation.targetpos;
    if (100 > (pos - targetpos).sqrMagnitude)
    {
      float num = (pos - targetpos).magnitude;
      if (num == 0) num = 0.00000000000000001f;
      accel += (pos - targetpos).normalized * (param.targetforceWeight / Mathf.Abs(num / 10));//ゼロ除算注意
    }
  }*/

  void UpdateTerrainForce()  // 壁からの斥力
  {
    foreach (var terrain in terrains)
    {
      Vector3 terrainoutpos = new Vector3(0, 0, 0);
      if (terrain.GetComponent<BoxCollider>())
      {
        terrainoutpos = terrain.GetComponent<BoxCollider>().ClosestPoint(pos);
      }
      else if (terrain.GetComponent<MeshCollider>())
      {
        terrainoutpos = terrain.GetComponent<MeshCollider>().ClosestPoint(pos);
      }
      float num = (pos - terrainoutpos).magnitude;
      if (num == 0) num = 0.00000000000000001f;
      accel += (pos - terrainoutpos).normalized * (param.terrainforceWeight / Mathf.Abs(num / param.neighborDistance));
    }
  }

  void UpdateMove()  // 移動処理
  {
    //myrigid.AddForce(accel, ForceMode.Acceleration);

    var dt = Time.deltaTime;

    velocity += accel * dt;
    var dir = velocity.normalized;
    var speed = velocity.magnitude;
    velocity = Mathf.Clamp(speed, param.minSpeed, param.maxSpeed) * dir;
    pos += velocity * dt;

    var rot = Quaternion.LookRotation(velocity);
    transform.rotation = rot;
    myrigid.velocity = velocity;
    //transform.SetPositionAndRotation(pos, rot);

    accel = Vector3.zero;
  }
}
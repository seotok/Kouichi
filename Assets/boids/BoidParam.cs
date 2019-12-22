using UnityEngine;


[CreateAssetMenu(menuName = "Boid1/BoidParam")]
public class BoidParam : ScriptableObject
{
  public float initSpeed = 2f;
  public float minSpeed = 2f;
  public float maxSpeed = 5f;
  public float neighborDistance = 1f;
  public float neighborFov = 90f;
  public float separationWeight = 5f;
  public float wallScale = 5f;
  public float wallDistance = 3f;
  public float wallWeight = 1f;
  public float alignmentWeight = 2f;
  public float cohesionWeight = 3f;
  public float turbulence = 1.0f;  // 値が小さいと乱れが大きい 0.0 ~ 1.0
  public float targetratio = 0.5f;  // ターゲットへ向かう力の大きさ 0.0 ~ 1.0
  //public float targetforceWeight = 1f;
  public float terrainforceWeight = 1f;
  public float terrainFov = 90f;
  public float terrainDistance = 1f;
  public bool kamikaze = false;
  public bool fragile = false;
}


using UnityEngine;


[CreateAssetMenu(menuName = "Agents/Param")]
public class Param : ScriptableObject
{
  public float neighborDistance = 1f;
  public float neighborFov = 90f;
  public float separationWeight = 5f;
  public float alignmentWeight = 2f;
  public float turbulence = 1.0f;
  public float fieldsize = 40.0f;
  public bool learningmode = false;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
  public Param param;
  public Vector3 point;
  private Vector3 scl = new Vector3(0.0f, 0.0f, 0.0f);
  // Start is called before the first frame update
  void Start()
  {
    if (!param.learningmode)
    {
      transform.position = param.fieldsize / 2 * point;
      if (point.x == 0) scl.x = param.fieldsize;
      if (point.y == 0) scl.y = param.fieldsize;
      if (point.z == 0) scl.z = param.fieldsize;
      GetComponent<BoxCollider>().size = scl;
    }
    else
    {
      GetComponent<BoxCollider>().size = new Vector3(0,0,0);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}

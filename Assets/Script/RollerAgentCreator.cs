using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Barracuda;

public class RollerAgentCreator : MonoBehaviour
{
  [SerializeField]
  int AgentsCount = 100;

  [SerializeField]
  int BoidsCount = 100;

  [SerializeField]
  public GameObject Agentrefab;

  [SerializeField]
  Param param;

  [SerializeField]
  BoidParam boidparam;

  [SerializeField]
  GameObject BoidPrefab;

  public Transform Target;

  public int totalboids = 0;

  List<RollerAgent> agents_ = new List<RollerAgent>();

  public ReadOnlyCollection<RollerAgent> agents
  {
    get { return agents_.AsReadOnly(); }
  }

  void AddAgents()
  {
    var go = Instantiate(Agentrefab, Random.insideUnitSphere, Random.rotation);
    go.transform.SetParent(transform);
    var agent = go.GetComponent<RollerAgent>();
    agent.Target = Target;
    agent.param = param;
    agent.boidparam = boidparam;
    agent.BoidPrefab = BoidPrefab;
    agent.BoidsCount = BoidsCount;
    agents_.Add(agent);
  }

  void RemoveAgents()
  {
    if (agents_.Count == 0) return;

    var lastIndex = agents_.Count - 1;
    var agent = agents_[lastIndex];
    Destroy(agent.gameObject);
    agents_.RemoveAt(lastIndex);
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    while (agents_.Count < AgentsCount)
    {
      AddAgents();
    }
    while (agents_.Count > AgentsCount)
    {
      RemoveAgents();
    }  
  }

  void LateUpdate()
  {
    totalboids = 0;
    foreach (GameObject boid in GameObject.FindGameObjectsWithTag("boids"))
    {
      totalboids++;
    }
  }
}

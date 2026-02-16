using UnityEngine;
using UnityEngine.AI;

public class UpSpeed_Agent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public float speedUp = 5;

    public GameObject Target;
    
    private NavMeshAgent agent;
    
    void Start()
    {
        agent = Target.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void addSpeed()
    {
        agent.speed += speedUp;
    }

    public void removeSpeed()
    {
        agent.speed -= speedUp;
    }
}

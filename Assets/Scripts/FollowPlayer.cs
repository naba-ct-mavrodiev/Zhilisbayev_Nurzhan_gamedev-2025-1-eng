using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Player;
    private Vector3 offset = new Vector3(0, 5, 7);
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    void Update()
    {
        transform.position = Player.transform.position + offset;

    }
    


}

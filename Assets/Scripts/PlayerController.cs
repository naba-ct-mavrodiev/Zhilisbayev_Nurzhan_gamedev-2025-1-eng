using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed;
    public float sideSpeed;

    private float horizontalInput;

    private float forwardInput;

    void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal");

        forwardInput = Input.GetAxis("Vertical");

        //Move the vehicle forward    
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        transform.Translate(Vector3.forward * Time.deltaTime * sideSpeed * forwardInput);



        transform.Rotate(Vector3.up, Time.deltaTime * sideSpeed * horizontalInput);
    }
}

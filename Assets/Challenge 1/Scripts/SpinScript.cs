using UnityEngine;

public class SpinScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speedSpin;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * speedSpin);
    }
}

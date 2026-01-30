using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0,0,-1) * movementSpeed * Time.deltaTime;
    }
}

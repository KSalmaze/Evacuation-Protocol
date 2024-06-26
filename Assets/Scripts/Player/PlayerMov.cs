using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float backSpeed;
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetAxis("Run") > 0 && verticalInput > 0)
        {
            verticalInput *= runningSpeed;
        }
        if (verticalInput < 0)
        {
            verticalInput *= backSpeed;
        }
        
        transform.Rotate(Vector3.up * (horizontalInput * rotationSpeed * Time.deltaTime));
        transform.Translate(Vector3.forward * (verticalInput * moveSpeed * Time.deltaTime));
    }
}

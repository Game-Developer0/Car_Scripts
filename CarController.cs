using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    private Rigidbody playerRB;
    public ParticleSystem rightNitro;
    public ParticleSystem LeftNitro;
    public float maxTorque = 500.0f;
    public float  maxSteeringAngle= 50.0f;
    public float maxSpeed = 150f;
    private float maxBrakeTorque=50f;
    public float currentSpeed;
    private float horizontalInput;
    private float verticalInput;
    public bool isBreaking;
    public float powerupStrength = 50f;
    public bool hasPowerup = false;


    // Use this for initialization
    void Start()
    {
        rightNitro.Stop();
        LeftNitro.Stop();
        playerRB = GetComponent<Rigidbody>();
       

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        GetInput();
        Drive();
        HandleSteering();
        Braking();
    }
    
    IEnumerator PowerupCountRoutine()
    {
        yield return new WaitForSeconds(5);
        hasPowerup = false;
        rightNitro.Stop();
        LeftNitro.Stop();
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
    }
    void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 100;

        
        if (currentSpeed < maxSpeed && !isBreaking )
        {
            float TargetSpeed = verticalInput * maxTorque;
            wheelRL.motorTorque = TargetSpeed;
            wheelRR.motorTorque = TargetSpeed;
        }
        else if(currentSpeed > maxSpeed&&!hasPowerup)
        {
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
        }
        }
        void HandleSteering()
    {
        
        float currentSteeringAngle = horizontalInput * maxSteeringAngle;

        wheelFL.steerAngle = currentSteeringAngle;
        wheelFR.steerAngle = currentSteeringAngle;
        

    }
    void Braking()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isBreaking = true;
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            isBreaking=false;
            wheelRL.brakeTorque = 0;  
            wheelRR.brakeTorque = 0;
        }
        }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(collision.gameObject);
            playerRB.AddForce(transform.forward * powerupStrength, ForceMode.Impulse);
            rightNitro.Play();
            LeftNitro.Play();
            StartCoroutine(PowerupCountRoutine());
        }

    }

}

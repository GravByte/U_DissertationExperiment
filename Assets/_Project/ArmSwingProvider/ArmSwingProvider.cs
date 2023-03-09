using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmSwingProvider : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The CharacterController attached to the XR Origin")]
    [SerializeField] private CharacterController characterController;
    
    [Tooltip("The left and right hand game objects")]
    [SerializeField] private GameObject leftHand, rightHand;
    
    [Header("Vector Positions")]
    private Vector3 _prevPosLeft, _prevPosRight, _direction;

    [SerializeField] private Camera mainCamera;
    
    [Header("Player Movement")]
    [Tooltip("The speed at which the player falls")]
    [SerializeField] private Vector3 gravity = new Vector3(0, -9.81f, 0);
    
    [Tooltip("The speed at which the player moves")]
    [SerializeField] float pSpeed = 5f;
    
    //---- Start is called before the first frame update
    void Start()
    {
        //---- Get the CharacterController component & check if it exists, otherwise throw an error
        characterController = GetComponent<CharacterController>();
        if (characterController == null) Debug.LogError("No CharacterController assigned to ArmSwingProvider");
        
        //---- Get the main camera & check if it exists, otherwise throw an error
        mainCamera = Camera.main;
        if (mainCamera == null) Debug.LogError("No main camera found");
        
        //---- Assign the left and right hand game objects & check if they exist, otherwise throw an error
        leftHand = GameObject.FindWithTag("LeftHand");
        rightHand = GameObject.FindWithTag("RightHand");
        if (leftHand == null || rightHand == null) Debug.LogError("No left or right hand game objects found");
        
        //---- Saves the initial position of the player hands
        SetPrevPos();
    }

    //---- Update is called once per frame
    void Update()
    {
        //---- Calculate velocity of player hand movement
        Vector3 leftHandVelocity = leftHand.transform.position - _prevPosLeft;
        Vector3 rightHandVelocity = rightHand.transform.position - _prevPosRight;
        float totalVelocity =+ leftHandVelocity.magnitude * 0.8f + rightHandVelocity.magnitude * 0.8f;

        //---- If true, player is swinging arms
        if (totalVelocity >= 0.05f)
        {
            //---- Get direction of player head
            _direction = mainCamera.transform.forward;
            
            //---- Move player in direction of head using the character controller
            characterController.Move(pSpeed * Time.deltaTime * Vector3.ProjectOnPlane(_direction, Vector3.up));
        }
        
        //---- Apply gravity to player
        characterController.Move(gravity * Time.deltaTime);
        SetPrevPos();
    }
    
    void SetPrevPos()
    {
        //---- Set previous position of player hands
        _prevPosLeft = leftHand.transform.position;
        _prevPosRight = rightHand.transform.position;
    }
}

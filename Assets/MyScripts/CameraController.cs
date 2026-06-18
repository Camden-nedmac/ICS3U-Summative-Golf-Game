using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UIElements;
using System.Numerics;
using Unity.Mathematics;


public class CameraController : MonoBehaviour
{
    public GameObject followObject;
    [SerializeField] private float rotationSpeed;
    public InputActionReference leftRightLookAction;
    private float currentAngle = 0f;
    void LateUpdate()
    {
        //rotate around the follow object camera
        float lookInput = leftRightLookAction.action.ReadValue<float>();
        UnityEngine.Vector3 newCameraOffset;
        //add rotation input to the camera's angle relative to the object the camera is following
        currentAngle += lookInput * rotationSpeed * Time.deltaTime;
        
        //use trig to get the x and z offsets of the camera, y offset is constant
        newCameraOffset = new UnityEngine.Vector3(Mathf.Cos(currentAngle) * 3, 1.5f, Mathf.Sin(currentAngle) * 3);
        transform.position = newCameraOffset + followObject.transform.position;
        
        transform.LookAt(followObject.transform, UnityEngine.Vector3.up);
        
    }
}

using System;
using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private GameEventHandler gameEventHandler;
    public float driverHorizPower;
    public float driverVertPower;
    public float putterHorizPower;
    public float putterVertPower;
    public InputActionReference MouseLeftClick;
    public Mouse mouse;
    public Camera mainCamera;
    public LayerMask terrainLayerMask;
    public Button driverButton;
    public Button putterButton;

    private float currentClubPower;
    private float currentClubLaunchHeight;
    private Vector2 mousePosition;
    private float frictionCoefficient = 0.1f;
    private bool isDragging = false;
    private bool isStopped = true;
    private void OnEnable()
    {
        MouseLeftClick.action.Enable();
        MouseLeftClick.action.started += _ => LeftButtonStarted();
        MouseLeftClick.action.canceled += _ => LeftButtonCanceled();
    }
    private void OnDisable()
    {
        MouseLeftClick.action.Disable();
        MouseLeftClick.action.started -= _ => LeftButtonStarted();
        MouseLeftClick.action.canceled -= _ => LeftButtonCanceled();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameEventHandler = FindAnyObjectByType<GameEventHandler>();

        driverButton.onClick.AddListener(() => SwitchClub(driverHorizPower,driverVertPower));
        putterButton.onClick.AddListener(() => SwitchClub(putterHorizPower,putterVertPower));
    }
    //input started
    private void LeftButtonStarted()
    {
        isDragging = IsMouseOverBall();
    }
    //button is no longer held down/is released
    private void LeftButtonCanceled()
    {
        if(isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            //check if ray hits terrain and if mouse is not hovering over ball and if ball isn't moving
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, terrainLayerMask) && !IsMouseOverBall() && isStopped)
            {
                Vector3 launchDirection = transform.position - hit.point;
                //apply force to ball according to the current club's power, and the launch direction
                rb.AddForce(new Vector3(launchDirection.x * currentClubPower, currentClubLaunchHeight, launchDirection.z * currentClubPower), ForceMode.Impulse);
                gameEventHandler.HandleBallLaunched();
            }
        }
        isDragging = false;
    }
    //cast a ray from mouse and check if its hovering over the ball
    private bool IsMouseOverBall()
    {
        //get mouse position and use it to cast a ray
        mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.rigidbody == rb;
        }
        return false;
    }
    //rigidbody starts colliding with terrain, increase drag
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            frictionCoefficient = 0.7f;
        }
    }
    //rigidbody stops colliding with terrain, decrease drag
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            frictionCoefficient = 0.01f;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        //apply drag
        rb.linearDamping = frictionCoefficient;
        rb.angularVelocity *= 0.99f;
    
        //check if ball is stopped and stop ball
        if (rb.linearVelocity.magnitude < 0.1f)
        {
            isStopped = true;
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            isStopped = false;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {   
        gameEventHandler.HandleBallTouchedHole(collider);
    }

    public void SwitchClub(float clubPower, float clubLaunchHeight)
    {
        currentClubPower = clubPower;
        currentClubLaunchHeight = clubLaunchHeight;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float MouseSens;
    public Transform camTrans;
    public CharacterController cc;

    private float camRotation = 0f;

    public float MoveSpeed;
    public float Grav = -9.8f;
    public float JumpSpeed;

    public float vertSpeed;

    public GameObject lightPanels;
    public GameObject redLights;
    public GameObject phoneLight;

    public GameObject ObjectiveTxt;
    public GameObject WinTxt;
    public GameObject ButtonTxt;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lightPanels.gameObject.SetActive(true);
        redLights.gameObject.SetActive(false);
        phoneLight.gameObject.SetActive(false);

        ObjectiveTxt.SetActive(false);
        WinTxt.SetActive(false);
        ButtonTxt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(camTrans.position, camTrans.forward, out hit))
        {
            Debug.DrawLine(camTrans.position + new Vector3(0f, -1f, 0f), hit.point, Color.green, 5f);
            if (hit.collider.gameObject.tag == "PowerButton")
            {
                ButtonTxt.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    lightPanels.gameObject.SetActive(true);
                    redLights.gameObject.SetActive(false);
                    WinTxt.SetActive(true);
                }
            }
            else 
            {
                ButtonTxt.SetActive(false);
            }

        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            MouseSens += 100;
        }
        else if (Input.GetKeyDown(KeyCode.Comma)) 
        {
            if (MouseSens > 100f) 
            {
                MouseSens -= 100;
            }
        }

        float mouseInputY = Input.GetAxis("Mouse Y") * MouseSens * Time.deltaTime;
        camRotation -= mouseInputY;
        camRotation = Mathf.Clamp(camRotation, -90f, 90f);
        camTrans.localRotation = Quaternion.Euler(camRotation, 0f, 0f);

        float mouseInputX = Input.GetAxis("Mouse X") * MouseSens * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, mouseInputX));

        Vector3 movement = Vector3.zero;

        // X/Z movement
        float forwardMovement = Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
        float sideMovement = Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;

        movement += (transform.forward * forwardMovement) + (transform.right * sideMovement);

        if (cc.isGrounded)
        {
            vertSpeed = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vertSpeed = JumpSpeed;
            }
        }

        vertSpeed += (Grav * Time.deltaTime);
        movement += (transform.up * vertSpeed * Time.deltaTime);

        cc.Move(movement);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LightsOff") 
        {
            lightPanels.gameObject.SetActive(false);
            redLights.gameObject.SetActive(true);
            phoneLight.gameObject.SetActive(true);
            ObjectiveTxt.SetActive(true);
        }
    }

}
    


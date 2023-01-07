using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamMovement : MonoBehaviour
{
    [SerializeField] private float camSpeed = 1.0f;
    [SerializeField] private float zoomSpeed = 1.0f;
    [SerializeField] private bool invertZoomDirection = false;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translation = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.W)) 
        {
            translation.y += camSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S)) 
        { 
            translation.y -= camSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) 
        {
            translation += Camera.main.transform.right * camSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A)) 
        {
            translation -= Camera.main.transform.right * camSpeed * Time.deltaTime;
        }
        if (Input.mouseScrollDelta.y != 0) 
        {
            cam.orthographicSize += Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime * (invertZoomDirection ? -1.0f : 1.0f);
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 0.1f, 100.0f);
        }

        transform.position += translation;
    }
}

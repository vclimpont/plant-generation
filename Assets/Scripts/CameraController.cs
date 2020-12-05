using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float translationSpeed;
    public float rotationSpeed;

    private Camera mainCamera;
    private Vector3 crtScreenMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            Translate(new Vector3(0, 1, 0), translationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Translate(new Vector3(0, -1, 0), translationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Translate(new Vector3(-1, 0, 0), translationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Translate(new Vector3(1, 0, 0), translationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Translate(new Vector3(0, 0, 1), translationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Translate(new Vector3(0, 0, -1), translationSpeed * Time.deltaTime);
        }

        if(Input.GetMouseButton(1))
        {
            Vector3 newScreenMousePosition = GetScreenMousePosition(100f);
            if(crtScreenMousePosition != newScreenMousePosition)
            {
                crtScreenMousePosition = newScreenMousePosition;
                LookAt(GetWorldMousePosition(crtScreenMousePosition));
            }
        }
    }

    void LookAt(Vector3 position)
    {
        transform.LookAt(position);
    }

    void Translate(Vector3 direction, float value)
    {
        transform.Translate(direction * value);
    }

    Vector3 GetWorldMousePosition(Vector3 screenMousePosition)
    {
        return mainCamera.ScreenToWorldPoint(screenMousePosition);
    }

    Vector3 GetScreenMousePosition(float dst)
    {
        Vector3 mousePosition = Input.mousePosition;
        return new Vector3(mousePosition.x, mousePosition.y, dst);
    }
}

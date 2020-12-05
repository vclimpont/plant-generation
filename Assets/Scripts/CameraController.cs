using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float translationSpeed;
    public float rotationSpeed;

    private float yaw;
    private float pitch;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            Translate(new Vector3(0, 0, 1), translationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Translate(new Vector3(0, 0, -1), translationSpeed * Time.deltaTime);
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
            Translate(new Vector3(0, 1, 0), translationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Translate(new Vector3(0, -1, 0), translationSpeed * Time.deltaTime);
        }

        if(Input.GetMouseButton(1))
        {
            yaw += rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
            pitch -= rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y");

            RotateEuler(pitch, yaw, 0f);
        }
    }

    void RotateEuler(float pitch, float yaw, float roll)
    {
        transform.eulerAngles = new Vector3(pitch, yaw, roll);
    }

    void Translate(Vector3 direction, float value)
    {
        transform.Translate(direction * value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float translationSpeed;
    public float deltaSpeed;
    public float rotationSpeed;

    private Vector3 startPosition;
    private Vector3 startRotation;
    private float yaw;
    private float pitch;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation.eulerAngles;
    }

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

        if(Input.GetKeyDown(KeyCode.F))
        {
            ResetTransform();
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            SpeedTranslation(deltaSpeed);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SpeedTranslation(-deltaSpeed);
        }

        float scrollDelta;
        if((scrollDelta = Input.mouseScrollDelta.y) != 0)
        {
            SpeedTranslation(scrollDelta * deltaSpeed);
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

    void ResetTransform()
    {
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
    }

    void SpeedTranslation(float dtSpeed)
    {
        float dtTranslation = translationSpeed + dtSpeed;

        translationSpeed = dtTranslation > 0 ? dtTranslation : translationSpeed;
    }
}

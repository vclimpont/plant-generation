using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public Camera mainCamera;
    public LSystem[] lSystems;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = RayCastOnMousePosition();

            if(hitInfo.collider != null && hitInfo.collider.CompareTag("Floor"))
            {
                SpawnTree(hitInfo.point);
            }
        }
    }

    private void SpawnTree(Vector3 position)
    {
        LSystem ls = GetRandomLSystem();

        if(ls.Rules == null)
        {
            ls.InitRules();
        }

        ls.DrawTree(position);
    }

    private RaycastHit RayCastOnMousePosition()
    {
        RaycastHit hitInfo;
        Vector3 direction = Vector3.Normalize(GetMousePosition() - mainCamera.transform.position);
        Physics.Raycast(mainCamera.transform.position, direction, out hitInfo);

        return hitInfo;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        return mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 100));
    }

    private LSystem GetRandomLSystem()
    {
        return lSystems[Random.Range(0, lSystems.Length)];
    }
}

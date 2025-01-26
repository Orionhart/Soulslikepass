using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public GameObject[] objectList;
    public float creationInterval = 2f;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= creationInterval)
        {
            CreateRandomObject();
            timer = 0f;
        }
    }

    private void CreateRandomObject()
    {
        int randomIndex = Random.Range(0, objectList.Length);
        GameObject selectedObject = objectList[randomIndex];

        GameObject newObject = Instantiate(selectedObject, transform.position, transform.rotation);
        newObject.transform.rotation = transform.rotation;
    }
}
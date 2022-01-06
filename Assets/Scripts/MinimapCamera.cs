using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField]
    public GameObject target;
    [SerializeField]
    public int height = 500;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(target.transform.position.x, height, target.transform.position.z);
        transform.LookAt(target.transform);
    }
}

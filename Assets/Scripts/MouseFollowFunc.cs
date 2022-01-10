using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MouseFollowFunc : MonoBehaviour
{
    
      public float speed = 8.0f;
      public float distanceFromCamera = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Cursor.visible == true){
             Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceFromCamera;
            Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 position = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * Time.deltaTime));
            transform.position = position;
         }
    }
}

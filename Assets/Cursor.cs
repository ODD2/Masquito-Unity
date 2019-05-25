using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private Vector3 v3;

    // Update is called once per frame
    void Update()
    {
        v3 = Input.mousePosition;
        v3.z = 10;
        transform.position = Camera.main.ScreenToWorldPoint(v3);
    }
}

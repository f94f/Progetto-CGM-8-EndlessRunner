using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float acceleration = 1.0f;
    public float maxSpeed = 60.0f;
    private float curSpeed;

    // Start is called before the first frame update
    void Start()
    {
        curSpeed = 10;
        acceleration = acceleration < 1 ? 1 : acceleration;
        GetComponent<Rigidbody>().velocity = new Vector3(curSpeed, 0, 0);
        //StartCoroutine(GetFaster());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private IEnumerator GetFaster()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody>().velocity = new Vector3(curSpeed, 0, 0);

        curSpeed += acceleration;

        if (curSpeed > maxSpeed)
            curSpeed = maxSpeed;

        Debug.Log(GetComponent<Rigidbody>().velocity);

        StartCoroutine(GetFaster());
    }
}

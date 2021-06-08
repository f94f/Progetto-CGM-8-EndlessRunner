using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float curSpeed;
    private bool laneChange = false;

    // Start is called before the first frame update
    void Start()
    {
        curSpeed = 10;
        GetComponent<Rigidbody>().velocity = new Vector3(10, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0 && !laneChange && (transform.position.z > -16))
        {
            Debug.Log("rigth");
            // Move to the rigth
            GetComponent<Rigidbody>().velocity = new Vector3(curSpeed, 0, -7);
            laneChange = true;
            StartCoroutine(StopLaneCh());
        }
        else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0 && !laneChange && (transform.position.z < -2))
        {
            Debug.Log("left");
            // Move to the left
            GetComponent<Rigidbody>().velocity = new Vector3(curSpeed, 0, 7);
            laneChange = true;
            StartCoroutine(StopLaneCh());
        }
    }

    private IEnumerator StopLaneCh()
    {
        yield return new WaitForSeconds(1);
        GetComponent<Rigidbody>().velocity = new Vector3(curSpeed, 0, 0);
        laneChange = false;
        Debug.Log(GetComponent<Transform>().position);
    }
}

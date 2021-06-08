using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageColector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        try
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
        catch
        {
            Destroy(other.gameObject);
        }
    }
}

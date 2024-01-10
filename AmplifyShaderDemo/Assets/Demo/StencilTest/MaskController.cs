using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.up * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
    }
}

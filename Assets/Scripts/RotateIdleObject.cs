using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateIdleObject : MonoBehaviour
{
    [SerializeField] private float rotateSpeed=45;
 
    void Update()
    {
        transform.Rotate(Vector2.up * Time.deltaTime * rotateSpeed);
    }
   
}

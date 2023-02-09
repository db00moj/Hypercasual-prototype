using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMaterialMove : MonoBehaviour
{
    [SerializeField]private Material myMaterial;
    [SerializeField] private float waterMovingSpeed=5;

    void Update()
    {
        myMaterial.mainTextureOffset = new Vector2(myMaterial.mainTextureOffset.x + waterMovingSpeed * Time.deltaTime, myMaterial.mainTextureOffset.y + waterMovingSpeed * Time.deltaTime);
    }
}

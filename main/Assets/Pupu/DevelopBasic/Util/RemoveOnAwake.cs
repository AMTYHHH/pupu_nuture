using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnAwake : MonoBehaviour
{
    void Awake()=>Destroy(gameObject);
}

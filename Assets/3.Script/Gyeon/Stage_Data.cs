using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]//에셋으로 만들기 위한 방법
public class Stage_Data : ScriptableObject
{
    [SerializeField] private Vector3 _LimitMin;
    [SerializeField] private Vector3 _LimitMax;

    public Vector3 LimitMax
    {
        get
        {
            return _LimitMax;
        }
    }
    public Vector3 LimitMin
    {
        get
        {
            return _LimitMin;
        }
    }
}

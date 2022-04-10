using UnityEngine;

public delegate void EmptyEvent();
public delegate void FloatEvent(float value);
public delegate void GenericEvent<T>(T v1);
public delegate void GenericEvent<T,Y>(T v, Y v2);
public delegate void GenericEvent<T,Y,Z>(T v, Y v2, Z v3);
public delegate void GenericEvent<T,Y,Z,P>(T v, Y v2, Z v3, P v4);

[System.Serializable]
public class MaxMinValue
{
    [SerializeField] private float max;
    [SerializeField] private float min;

    public float Max { get { return max; } }
    public float Min { get { return min; } }
}
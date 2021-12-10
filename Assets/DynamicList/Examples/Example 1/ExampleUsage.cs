using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

public class ExampleUsage: MonoBehaviour
{
    public DynamicList<A> myList = new DynamicList<A>();
}

[Serializable]
public class A
{
    public float a;
}

public class B: A
{
    public float b;
}


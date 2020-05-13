using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEvent
{
    public delegate void NoParam();
    public delegate void OneParam(object obj);
    public delegate void TwoParam(object obj1, object obj2);
    public delegate void ThreeParam(object obj1, object obj2, object obj3);
    public delegate void ManyParam(object[] objs);
}

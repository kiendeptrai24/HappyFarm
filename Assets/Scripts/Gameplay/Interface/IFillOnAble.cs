

using System;
using UnityEngine;
public interface IFillOnAble
{
    Action<GameObject> OnFillOnAnble { get; set; }
    Action<GameObject> OnFillOnUnable { get; set; }
    Vector3 position { get; }
    void OnFill(GameObject source);
    void OnEmpty();
    bool Isfilled();
}

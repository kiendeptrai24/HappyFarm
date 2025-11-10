

using System;
using UnityEngine;
public interface IFillOnAble
{
    Action OnFillOnAnble { get; set; }
    Action OnFillOnUnable { get; set; }
    Vector3 position { get; }
    void OnFill(GameObject source);
    void OnEmpty();
    bool Isfilled();
}

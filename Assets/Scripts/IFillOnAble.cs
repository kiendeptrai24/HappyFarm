

using UnityEngine;

public interface IFillOnAble
{
    Vector3 position { get; }
    void OnFill(GameObject source);
    void OnEmpty();
    bool Isfilled();
}

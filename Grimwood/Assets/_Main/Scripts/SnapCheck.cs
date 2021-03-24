using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCheck : MonoBehaviour
{
    bool _isObjectSnapped = false;

    public bool GetIsObjectSnapped()
    {
        return _isObjectSnapped;
    }

    public void SetIsObjectSnapped(bool value)
    {
        _isObjectSnapped = value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTargetHandler : MonoBehaviour
{
    public bool IsImageTargetFound => _isImageTargetFound;
    private bool _isImageTargetFound = false;


    public void OnImageTargetFound()
    {
        if (_isImageTargetFound) return;
        _isImageTargetFound = true;
    }
}

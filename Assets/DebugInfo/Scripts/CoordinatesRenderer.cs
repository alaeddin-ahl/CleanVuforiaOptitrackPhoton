using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinatesRenderer : MonoBehaviour
{
    [SerializeField]
    private GameObject _linePrefab;

    // Start is called before the first frame update
    void Start()
    {
        InitLine(new Vector3(0,90,0), Color.red);  //x-axis 
        InitLine(new Vector3(-90, 0, 0), Color.green);  //y-axis 
        InitLine(new Vector3(0,0,0), Color.blue);  //z-axis 
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitLine(Vector3 rotation, Color color)
    {
        var line = Instantiate(_linePrefab, gameObject.transform);
        line.transform.localPosition = Vector3.zero;
        line.transform.localRotation =  Quaternion.Euler(rotation);

        var renderer = line.GetComponent<LineRenderer>();
        renderer.endColor = renderer.startColor = color;
    }
}

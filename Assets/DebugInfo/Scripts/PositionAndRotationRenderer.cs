using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PositionAndRotationRenderer : MonoBehaviour
{
    [SerializeField]
    private GameObject _textPrefab;

    [SerializeField]
    private string _prefix;

    //[SerializeField]
    private TextMeshPro _textMesh;



    // Start is called before the first frame update
    void Start()
    {
        var textObj = Instantiate(_textPrefab, transform);
        _textMesh = textObj.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        _textMesh.text = string.IsNullOrWhiteSpace(_prefix)
            ? $"{gameObject.transform.position}\n{gameObject.transform.eulerAngles}"
            : $"{_prefix}\n{gameObject.transform.position}\n{gameObject.transform.eulerAngles}";

        _textMesh.transform.LookAt(Camera.main.transform);
        _textMesh.transform.Rotate(Vector3.up, 180);
    }
}

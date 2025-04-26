using UnityEngine;

public class KitchenArrow : MonoBehaviour
{
    private GameObject _dateCanvas;
    private DateScriptHandler _scriptHandler;

    private void Start()
    {
        _dateCanvas = GameObject.Find("DateCanvas");
        _scriptHandler = _dateCanvas.GetComponent<DateScriptHandler>();
    }

    private void OnMouseDown()
    {
        _scriptHandler.ExcusePlayer(); // to reset
        _dateCanvas.GetComponent<Canvas>().enabled = true;
    }
}
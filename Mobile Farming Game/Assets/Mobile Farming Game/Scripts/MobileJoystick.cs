using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileJoystick : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;

    [Header("Settings")]
    [SerializeField] private float moveFactor;
    private Vector3 clickedPosition;
    public Vector3 move { get; private set; }
    private bool canControl;

    void Start()
    {
        HideJoystick();
    }

    void Update()
    {
        if (canControl)
            ControlJoystick();
    }

    public void ClickedOnJoystickZoneCallback()
    {
        clickedPosition = Input.mousePosition;
        joystickOutline.position = clickedPosition;

        ShowJoystick();
    }

    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
        canControl = true;
    }

    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        canControl = false;

        move = Vector3.zero;
    }

    private void ControlJoystick()
    {
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - clickedPosition;

        float canvasScale = GetComponentInParent<Canvas>().scaleFactor;

        float joystickMoveRadius = joystickOutline.rect.width * canvasScale / 2;
        float moveMagnitude = Mathf.Min(direction.magnitude, joystickMoveRadius);

        move = direction.normalized * moveMagnitude;

        joystickKnob.position = clickedPosition + move;

        if (Input.GetMouseButtonUp(0))
            HideJoystick();
    }
}

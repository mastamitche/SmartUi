using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbStick : MonoBehaviour {

    SmartUI Base;
    SmartUI Stick;

    bool holding = false;
    public float direction; // in rads
    public float amount; // 0-1 from center
    public static ThumbStick instance;

    // Use this for initialization
    void Start () {
        instance = this;
        Base = transform.GetComponent<SmartUI>();
        Stick = transform.Find("Stick").GetComponent<SmartUI>();
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.touchCount > 0)
            handleTouch();

        //testWithMouse();
    }

    void testWithMouse()
    {
        setJoystickByPosition(Input.mousePosition);
    }

    void setJoystickByPosition(Vector2 screenPosition)
    {
        float maxDist = Base.position.width / 2 + Stick.position.width / 4;
        Vector2 center = Base.screenPositionOfCenter();
        center.y = Screen.height - center.y;
        Vector2 newPosition = new Vector2(screenPosition.x - center.x, center.y - screenPosition.y);
        if (newPosition.magnitude > maxDist)
            newPosition = newPosition.normalized * maxDist;

        Stick.position.position = newPosition;
        Stick.setElementToRect();

        direction = -Mathf.Atan2(newPosition.y, newPosition.x);
        amount = newPosition.magnitude / maxDist;
    }

    void handleTouch()
    {

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (Base.contains(touch.position))
                {
                    // we are in thumbstick, start tracking
                    holding = true;
                    setJoystickByPosition(touch.position);
                    Stick.setElementToRect();
                }
                break;

            case TouchPhase.Moved:
                if (holding)
                {
                    setJoystickByPosition(touch.position);
                }
                break;
                
            case TouchPhase.Ended:
                if (holding)
                {
                    holding = false;
                    Stick.position.x = 0;
                    Stick.position.y = 0;
                    Stick.setElementToRect();
                }
                break;
        }
    }

    public static Vector2 Vector2FromAngle(float angle) {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateTo : UIAnimation {
    long startTime;
    float startAngle;
    float endAngle;
    float endAfter;

    public rotateTo(float startAngle,float endAngle, float overTime) {
        this.startAngle = startAngle;
        this.endAngle = endAngle;
        endAfter = overTime;
    }

    public override void init() {
        startTime = System.DateTime.Now.Ticks / 10000L;
        sui.removeAnimationsOfType(typeof(resetAngle));
        sui.removeAnimationsOfType(typeof(rotate));
        sui.removeAnimationsOfType(typeof(wobble));
    }

    public override bool tick()
    {
        float percentage = ((System.DateTime.Now.Ticks / 10000L)+0.0f-startTime)/ (endAfter);
        //Debug.Log(percentage);
        sui.angle = startAngle + ((startAngle - endAngle) * percentage);
        if (percentage >= 1)
        {
            sui.angle = endAngle;
            return true;
        }
        return false;
    }
}

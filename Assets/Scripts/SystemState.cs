using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemState {

    private static SystemState _instance;
    public static SystemState Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new SystemState();
            }

            return _instance;
        }
    }

    public int Day { get; set; }
    public float FatigueLeft { get; set; }

    private SystemState()
    {
        Restart();
    }

    public void Restart()
    {
        Day = 1;
        FatigueLeft = 14;
    }
}

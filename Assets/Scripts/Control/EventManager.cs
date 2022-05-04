﻿using System.Collections;
using System.Collections.Generic;
using AVJ.Settings;
using UnityEngine;

public static class EventManager
{
    public static Queue<LayerEvent> Events = new Queue<LayerEvent>();
    public static List<ISettingControl> InputEvent = new List<ISettingControl>();

    public static bool IsWaitForBinding
    {
        get => BindTarget == null;
    }
    
    public static ISettingControl BindTarget = null;

    public static void Clear() => Events = new Queue<LayerEvent>();
}
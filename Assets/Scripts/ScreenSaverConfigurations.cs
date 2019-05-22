using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenSaverConfigurations {

    public static bool playWithSound = true;
    public static bool screenSleep = false;
    [Tooltip("If false videos are loaded from path parallel to .exe file /Video")]
    public static bool loadVideoFilesFromResources = true;
    public static string videosPath = "Videos/";
}

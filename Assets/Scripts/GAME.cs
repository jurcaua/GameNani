using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GAME {

    public static bool IsMobile { get { return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer; } }
    public static bool IsAndroid { get { return Application.platform == RuntimePlatform.Android; } }

}

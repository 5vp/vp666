using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

[Preserve]
public class StopVray
{
    #if UNITY_WEBGL
    private static void Application_focusChanged(bool obj)
    {
        Application.focusChanged -= Application_focusChanged;
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }
    #endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void BeforeSplashScreen()
    {
#if UNITY_WEBGL
        Application.focusChanged += Application_focusChanged;
#else
        Task.Run(()=>{
            SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
        });
#endif
    }
}

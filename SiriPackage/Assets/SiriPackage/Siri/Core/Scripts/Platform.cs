using UnityEngine;

namespace Siri {
    public class Platform {
        public static bool isMobile() {
            return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android;
        }

        public static bool isWeb() {
            return Application.platform == RuntimePlatform.WebGLPlayer;
        }

        public static bool isDesktop() {
            return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer;
        }

        public static bool isEditor() {
            return Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor;
        }
    }
}


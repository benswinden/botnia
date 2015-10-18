using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Global : MonoBehaviour {


    #region GLOBAL GAME VARIABLES
    
    public static Manager manager;    

    public static CameraController cameraController;    
    
    public static Player player;



    //Grid based game    
    public static Dictionary<Vector2, GameObject> blockList = new Dictionary<Vector2, GameObject>();        // Holds a reference to objects at positions on the grid
    
    #endregion


    #region GLOBAL GAME FUNCTIONS    

    //Math
    public static class MathEasing {
        public enum types {
            Linear,
            QuadEaseIn,
            QuadEaseOut,
            QuadEaseInOut,
            CubicEaseIn,
            CubicEaseOut,
            CubicEaseInOut,
            QuartEaseIn,
            QuartEaseOut,
            QuartEaseInOut,
            QuintEaseIn,
            QuintEaseOut,
            QuintEaseInOut,
            SineEaseIn,
            SineEaseOut,
            SineEaseInOut,
            ExpoEaseIn,
            ExpoEaseOut,
            ExpoEaseInOut,
            CircEaseIn,
            CircEaseOut,
            CircEaseInOut
        }

        //Linear
        public static float Linear(float from, float to, float currentTime, float duration) {
            return (to - from) * currentTime / duration + from;
        }

        //Quadratic
        public static float QuadEaseIn(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration;
            return changeInValue * currentTime * currentTime + from;
        }

        public static float QuadEaseOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration;
            return -changeInValue * currentTime * (currentTime - 2) + from;
        }

        public static float QuadEaseInOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration / 2;

            if (currentTime < 1) return changeInValue / 2 * currentTime * currentTime + from;
            currentTime--;
            return -changeInValue / 2 * (currentTime * (currentTime - 2) - 1) + from;
        }

        //Cubic
        public static float CubicEaseIn(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration;
            return changeInValue * currentTime * currentTime * currentTime + from;
        }

        public static float CubicEaseOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration;
            currentTime--;
            return changeInValue * (currentTime * currentTime * currentTime + 1) + from;
        }

        public static float CubicEaseInOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration / 2;

            if (currentTime < 1) return changeInValue / 2 * currentTime * currentTime * currentTime + from;
            currentTime -= 2;
            return changeInValue / 2 * (currentTime * currentTime * currentTime + 2) + from;
        }

        //Quart
        public static float QuartEaseIn(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration;
            return changeInValue * currentTime * currentTime * currentTime * currentTime + from;
        }

        public static float QuartEaseOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration;
            currentTime--;
            return -changeInValue * (currentTime * currentTime * currentTime * currentTime - 1) + from;
        }

        public static float QuartEaseInOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration / 2;

            if (currentTime < 1) return changeInValue / 2 * currentTime * currentTime * currentTime * currentTime + from;
            currentTime -= 2;
            return -changeInValue / 2 * (currentTime * currentTime * currentTime * currentTime - 2) + from;
        }

        //Quint
        public static float QuintEaseIn(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration;
            return changeInValue * currentTime * currentTime * currentTime * currentTime * currentTime + from;
        }

        public static float QuintEaseOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration;
            currentTime--;
            return changeInValue * (currentTime * currentTime * currentTime * currentTime * currentTime + 1) + from;
        }

        public static float QuintEaseInOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration / 2;

            if (currentTime < 1) return changeInValue / 2 * currentTime * currentTime * currentTime * currentTime * currentTime + from;
            currentTime -= 2;
            return changeInValue / 2 * (currentTime * currentTime * currentTime * currentTime * currentTime + 2) + from;
        }

        //Sine
        public static float SineEaseIn(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            return -changeInValue * Mathf.Cos(currentTime / duration * (Mathf.PI / 2)) + changeInValue + from;
        }

        public static float SineEaseOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            return changeInValue * Mathf.Sin(currentTime / duration * (Mathf.PI / 2)) + from;
        }

        public static float SineEaseInOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            return -changeInValue / 2 * (Mathf.Cos(Mathf.PI * currentTime / duration) - 1) + from;
        }

        //Expo
        public static float ExpoEaseIn(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            return -changeInValue * Mathf.Cos(currentTime / duration * (Mathf.PI / 2)) + changeInValue + from;
        }

        public static float ExpoEaseOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            return changeInValue * Mathf.Sin(currentTime / duration * (Mathf.PI / 2)) + from;
        }

        public static float ExpoEaseInOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            return changeInValue * Mathf.Pow(2, 10 * (currentTime / duration - 1)) + from;
        }

        //Circ
        public static float CircEaseIn(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            return changeInValue * (-Mathf.Pow(2, -10 * currentTime / duration) + 1) + from;
        }

        public static float CircEaseOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            currentTime /= duration / 2;
            if (currentTime < 1) return changeInValue / 2 * Mathf.Pow(2, 10 * (currentTime - 1)) + from;
            currentTime--;
            return changeInValue / 2 * (-Mathf.Pow(2, -10 * currentTime) + 2) + from;

        }

        public static float CircEaseInOut(float from, float to, float currentTime, float duration) {
            float changeInValue = to - from;
            if (currentTime < 1) return -changeInValue / 2 * (Mathf.Sqrt(1 - currentTime * currentTime) - 1) + from;
            currentTime -= 2;
            return changeInValue / 2 * (Mathf.Sqrt(1 - currentTime * currentTime) + 1) + from;
        }

        //Delegator
        public static float CalculateEase(types type, float from, float to, float currentTime, float duration) {
            switch (type) {
                case types.Linear:
                    return Linear(from, to, currentTime, duration);

                case types.QuadEaseIn:
                    return QuadEaseIn(from, to, currentTime, duration);
                case types.QuadEaseOut:
                    return QuadEaseOut(from, to, currentTime, duration);
                case types.QuadEaseInOut:
                    return QuadEaseInOut(from, to, currentTime, duration);

                case types.CubicEaseIn:
                    return CubicEaseIn(from, to, currentTime, duration);
                case types.CubicEaseOut:
                    return CubicEaseOut(from, to, currentTime, duration);
                case types.CubicEaseInOut:
                    return CubicEaseInOut(from, to, currentTime, duration);

                case types.QuartEaseIn:
                    return QuartEaseIn(from, to, currentTime, duration);
                case types.QuartEaseOut:
                    return QuartEaseOut(from, to, currentTime, duration);
                case types.QuartEaseInOut:
                    return QuartEaseInOut(from, to, currentTime, duration);

                case types.QuintEaseIn:
                    return QuintEaseIn(from, to, currentTime, duration);
                case types.QuintEaseOut:
                    return QuintEaseOut(from, to, currentTime, duration);
                case types.QuintEaseInOut:
                    return QuintEaseInOut(from, to, currentTime, duration);

                case types.SineEaseIn:
                    return SineEaseIn(from, to, currentTime, duration);
                case types.SineEaseOut:
                    return SineEaseOut(from, to, currentTime, duration);
                case types.SineEaseInOut:
                    return SineEaseInOut(from, to, currentTime, duration);

                case types.ExpoEaseIn:
                    return ExpoEaseIn(from, to, currentTime, duration);
                case types.ExpoEaseOut:
                    return ExpoEaseOut(from, to, currentTime, duration);
                case types.ExpoEaseInOut:
                    return ExpoEaseInOut(from, to, currentTime, duration);

                case types.CircEaseIn:
                    return CircEaseIn(from, to, currentTime, duration);
                case types.CircEaseOut:
                    return CircEaseOut(from, to, currentTime, duration);
                case types.CircEaseInOut:
                    return CircEaseInOut(from, to, currentTime, duration);
            }

            Debug.Log("FAILED");
            return 0;
        }
    }

    #endregion
	

}

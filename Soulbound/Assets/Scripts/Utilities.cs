using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Utils
    {
        
        public static Vector3 GetVectorFromAngle (float angle)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static bool AreCharactersFacing(CharacterController char01, CharacterController char02)
        {
            if (char01.IsFacingRight() && !char02.IsFacingRight()) return true;
            if (!char01.IsFacingRight() && char02.IsFacingRight()) return true;
            else return false;
        }

    }

}

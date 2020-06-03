using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class Utils
    {

        public static Vector3 GetVectorFromAngle(float angle)
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

        public static void SetDesiredTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }

    }

    public class DevTools
    {
        public static void ReloadLevel()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        public static void GiveSoulPoints()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Player.currentSoulPoints += 5;
            }
        }

        public static void AddEnemy()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                GameObject enemyGO; 
                Enemy[] enemyGOs;
                enemyGO = Object.FindObjectOfType<Enemy>().gameObject;
                enemyGOs = Object.FindObjectsOfType<Enemy>();
                var enemy = Object.Instantiate(enemyGO, enemyGOs[Random.Range(0, enemyGOs.Length)].transform.position, enemyGO.transform.rotation) as GameObject;

            }
        }

    }
}

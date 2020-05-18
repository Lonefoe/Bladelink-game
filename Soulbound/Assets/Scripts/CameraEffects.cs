using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class CameraEffects : Singleton<CameraEffects>
{
    CinemachineImpulseSource shake;

    private void Awake()
    {
        shake = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float amplitude, float frequency)
    {
        if (shake == null) return;
        shake.m_ImpulseDefinition.m_AmplitudeGain = amplitude;
        shake.m_ImpulseDefinition.m_FrequencyGain = frequency;
        shake.GenerateImpulse();
    }

    public IEnumerator PauseEffect(float duration = .05f)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;

    }

}

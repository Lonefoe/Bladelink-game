using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using Utilities;

public class CameraEffects : Singleton<CameraEffects>
{
    CinemachineImpulseSource shake;

    private bool isSlowed = false;

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
        Utils.SetDesiredTimeScale(0f);
        yield return new WaitForSecondsRealtime(duration);
        Utils.SetDesiredTimeScale(1f);
        Time.fixedDeltaTime = 0.02f;
    }

    public IEnumerator Slowmotion(float slowAmount = .2f, float slowTime = 1f)
    {
        Time.timeScale = slowAmount;
        Time.fixedDeltaTime = slowAmount * 0.02f;
        AudioManager.Instance.Play("SlowDown");
        yield return new WaitForSecondsRealtime(slowTime);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        AudioManager.Instance.Play("SlowUp");
    }
    
}

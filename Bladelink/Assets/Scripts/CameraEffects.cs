using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Utilities;

public class CameraEffects : Singleton<CameraEffects>
{
    public Volume globalPostProcessing;

    CinemachineImpulseSource shake;
    Vignette vignette;

    private bool isSlowed = false;
    private float currentSlowAmount;

    private void Awake()
    {
        shake = GetComponent<CinemachineImpulseSource>();
        
    }
    
    void Start()
    {
        Vignette _vignette;
        if(globalPostProcessing.profile.TryGet<Vignette>(out _vignette)) vignette = _vignette;
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
        if (isSlowed) yield break;
        Utils.SetDesiredTimeScale(0f);
        Debug.Log("pause");
        yield return new WaitForSecondsRealtime(duration);
        Utils.SetDesiredTimeScale(1f);
        Time.fixedDeltaTime = 0.02f;

    }

    public IEnumerator VignettePop(float time)
    {
        if(vignette != null && vignette.active == false) { vignette.active = true; }
        else yield break;
        yield return new WaitForSecondsRealtime(time);
        if(vignette != null && vignette.active == true) vignette.active = false;
    }

    public IEnumerator Slowmotion(float slowAmount = .2f, float slowTime = 1f)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            Time.timeScale = slowAmount;
            Time.fixedDeltaTime = slowAmount * 0.02f;
            AudioManager.Instance.Play("SlowDown");
            currentSlowAmount = slowAmount;
            yield return new WaitForSeconds(slowAmount * slowTime);
            isSlowed = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            AudioManager.Instance.Play("SlowUp");
            currentSlowAmount = 1;
        }
    }
    
}

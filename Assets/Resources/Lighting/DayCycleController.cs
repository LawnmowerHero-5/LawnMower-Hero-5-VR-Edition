using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DayCycleController : MonoBehaviour
{
    [Range(0, 24)] 
    public float timeOfDay;

    public float orbitSpeed = 1.0f;
    public Light sun;
    public Light moon;
    public Volume skyVolume;
    public AnimationCurve starsCurve;

    private bool isNight;
    private PhysicallyBasedSky sky;

    private void Start()
    {
        skyVolume.profile.TryGet<PhysicallyBasedSky>(out sky);
    }

    void Update()
         {
             timeOfDay += Time.deltaTime * orbitSpeed;
             if (timeOfDay > 24)
                 timeOfDay = 0;
             
             UpdateTime();
         }

         private void OnValidate()
         {
             UpdateTime();
             skyVolume.profile.TryGet<PhysicallyBasedSky>(out sky);
         }

         private void UpdateTime()
         {
             float alpha = timeOfDay / 24.0f;
             float sunRotation = Mathf.Lerp(-90, 270, alpha);
             float moonRotation = sunRotation - 180;
             sun.transform.rotation = Quaternion.Euler(sunRotation, -150.0f, 0);
             moon.transform.rotation = Quaternion.Euler(moonRotation, -150.0f, 0);
             
             CheckNightDayTransition();
         }

         private void CheckNightDayTransition()
         {
             if (isNight)
             {
                 if (moon.transform.rotation.eulerAngles.x > 180)
                 {
                     StartDay();
                 }
                 
             }
             else
             {
                 if (sun.transform.rotation.eulerAngles.x > 180)
                 {
                     StartNight();
                 }
             }
         }

         private void StartDay()
         {
             isNight = false;
             sun.shadows = LightShadows.Soft;
             moon.shadows = LightShadows.None;
         }

         private void StartNight()
         {
             isNight = true;
             sun.shadows = LightShadows.None;
             moon.shadows = LightShadows.Soft;
         }
}
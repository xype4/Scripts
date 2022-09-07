using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public GameObject Player; 
    public GameObject Stars;

    [Range (0,1)]
    public float TimeOfDay;
    public float DayDuraction = 30f;

    public AnimationCurve SunCurve;
    public Gradient FogGradient;

    public Material DaySkyBox;
    public Material NightSkyBox;
    private ParticleSystem stars;

    public Light Sun;
    public Light Moon;

    private float SunInt;
    private float MoonInt;
    private float CurveFactor; 



    void Start()
    {
        SunInt = Sun.intensity;
        MoonInt = Moon.intensity;
        stars = Stars.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        TimeOfDay += Time.deltaTime/DayDuraction;
        if (TimeOfDay >= 1) TimeOfDay -=1;

        CurveFactor = SunCurve.Evaluate(TimeOfDay);

        var mainModule = stars.main;
        mainModule.startColor = new Color(1,1,1, 1-CurveFactor);

        RenderSettings.skybox.Lerp(NightSkyBox, DaySkyBox, CurveFactor);
        RenderSettings.sun = CurveFactor>0.1f ? Sun: Moon;
        RenderSettings.fogColor = FogGradient.Evaluate(TimeOfDay);
        DynamicGI.UpdateEnvironment();

        Sun.transform.localRotation = Quaternion.Euler(TimeOfDay*360f,170,0);
        Moon.transform.localRotation = Quaternion.Euler(TimeOfDay*360f+ 180,170,0);
        Sun.intensity = SunInt*CurveFactor;
        Moon.intensity = (float)MoonInt*(CurveFactor-1)*-1;

        Stars.transform.position = Player.transform.position;
    }
}

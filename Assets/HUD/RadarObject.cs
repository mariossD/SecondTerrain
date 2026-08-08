using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarObject : MonoBehaviour
{
    public Image radarIcon;
    void Start()
    {
        Radar.SubscribeRadarObject(this.gameObject, radarIcon);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        Radar.UnsubscribeRadarObject(this.gameObject);
    }
}

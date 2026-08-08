using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarObjects
{
    public Image radarIcon { get; set; }
    public GameObject player { get; set; }
}

public class Radar : MonoBehaviour
{
    public Transform playerPosition;
    public float mapScale = 2.0f;

    public static List<RadarObjects> radarObjects = new List<RadarObjects>();

    public static void SubscribeRadarObject(GameObject player, Image radarIcon)
    {
        Image imageRadarIcon = Instantiate(radarIcon);
        radarObjects.Add(new RadarObjects() { radarIcon = imageRadarIcon, player = player });
    }

    public static void UnsubscribeRadarObject(GameObject player)
    {
        List<RadarObjects> newRadarObject = new List<RadarObjects>();

        for (int i = radarObjects.Count - 1; i >= 0; i--)
        {
            if (radarObjects[i].player == player)
            {
                Destroy(radarObjects[i].radarIcon);
                continue;
            }
            else
            {
                newRadarObject.Add(radarObjects[i]);
            }
        }

        radarObjects.RemoveRange(0, radarObjects.Count);
        radarObjects.AddRange(newRadarObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerPosition == null) return;

        foreach (RadarObjects radarObject in radarObjects)
        {
            Vector3 radarPosition = radarObject.player.transform.position - playerPosition.position;
            float distance = Vector3.Distance(playerPosition.position, radarObject.player.transform.position) * mapScale;

            float deltaY = Mathf.Atan2(radarPosition.x, radarPosition.z) * Mathf.Rad2Deg - 270 - playerPosition.eulerAngles.y;
            radarPosition.x = distance * Mathf.Cos(deltaY * Mathf.Deg2Rad) * -1;
            radarPosition.z = distance * Mathf.Sin(deltaY * Mathf.Deg2Rad);

            radarObject.radarIcon.transform.SetParent(this.transform);

            RectTransform radarRectTransform = this.GetComponent<RectTransform>();
            radarObject.radarIcon.transform.position = new Vector3(radarPosition.x + radarRectTransform.pivot.x, radarPosition.z + radarRectTransform.pivot.y, 0) + this.transform.position;
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCVision : MonoBehaviour
{
    public event EventHandler Detected_notifier;

    public GameObject NearestDetectedTrigger { get => _nearestTrigger; private set => _nearestTrigger = value; }
    public List<GameObject> AllDetectedTrigger_list { get => _allDetectedTrigger_list; private set => _allDetectedTrigger_list = value; }
    public Vector2 LastTargetPosition { get => _lastTargetPosition; set => _lastTargetPosition = value; }

    [SerializeField] private List<string> _triggerTags;
    [SerializeField] private LayerMask _triggerLayer;
    [SerializeField] private Vector2 _visionBoxSize;

    [Header("Debug")]
    [SerializeField] private GameObject _nearestTrigger;
    [SerializeField] private List<GameObject> _allDetectedTrigger_list;
    [SerializeField] private Vector2 _lastTargetPosition;

    private void Awake()
    {
        _allDetectedTrigger_list = new List<GameObject>();
    }

    public bool CheckTriggersInVisionZone()
    {
        if (NearestDetectedTrigger != null)
        {
            LastTargetPosition = NearestDetectedTrigger.transform.position;
        }

        AllDetectedTrigger_list = StandartMethods.GetObjectsInBoxZoneByTagsAndLayerMask(this.transform.position, _visionBoxSize, _triggerTags, _triggerLayer);
        NearestDetectedTrigger = StandartMethods.GetNearestObject(this.transform.position, AllDetectedTrigger_list);

        if (NearestDetectedTrigger != null)
        {
            Detected_notifier?.Invoke(this, EventArgs.Empty);
            return true;
        }

        return false;
    }
}
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCVision : MonoBehaviour
{
    public event EventHandler detected_notifier;

    public GameObject NearestDetectedTrigger { get => _nearestTrigger; private set => _nearestTrigger = value; }
    public List<GameObject> AllDetectedTrigger_list { get => _allDetectedTrigger_list; private set => _allDetectedTrigger_list = value; }

    [SerializeField] private List<string> _triggerTags;
    [SerializeField] private LayerMask _triggerLayer;
    [SerializeField] private Vector2 _visionBoxSize;

    [Header("Debug")]
    [SerializeField] private GameObject _nearestTrigger;
    [SerializeField] private List<GameObject> _allDetectedTrigger_list;

    private Rigidbody2D _myRB;

    private void Awake()
    {
        _allDetectedTrigger_list = new List<GameObject>();
    }

    public void CheckTriggersInVisionZone()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(this.transform.position, _visionBoxSize, 0, Vector2.zero, 10, _triggerLayer);
        AllDetectedTrigger_list = GetHitsByTags(_triggerTags, hits);
        NearestDetectedTrigger = GetNearestDetectedTrigger();
    }

    private GameObject GetNearestDetectedTrigger()
    {
        if (AllDetectedTrigger_list.Count > 0)
        {
            GameObject nearestTrigger = AllDetectedTrigger_list[0];
            float nearestDistance = float.MaxValue;

            foreach (var trigger in AllDetectedTrigger_list)
            {
                var currentDistance = Vector2.Distance(this.transform.position, trigger.transform.position);

                if (currentDistance < nearestDistance)
                {
                    nearestDistance = currentDistance;
                    nearestTrigger = trigger;
                }
            }

            return nearestTrigger;
        }

        return null;
    }

    private List<GameObject> GetHitsByTags(List<string> tags, RaycastHit2D[] hits)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (var item in hits)
        {
            if (CompareTags(tags, item.collider.gameObject))
            {
                result.Add(item.collider.gameObject);
            }
            else
            {
                continue;
            }
        }

        return result;

        bool CompareTags(List<string> tags, GameObject gameObject)
        {
            foreach (var tag in tags)
            {
                if (gameObject.CompareTag(tag))
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }

            return false;
        }
    }
}
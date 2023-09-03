using System;
using System.Collections.Generic;
using UnityEngine;

public static class StandartMethods
{
    /// <summary>
    /// Проверка на содержания бита(степени двойки) в десятичном числе
    /// </summary>
    /// <param name="bit">Бит(степень двойки)</param>
    /// <param name="number">Десятичное число</param>
    /// <returns>Содержит ли число этот бит?</returns>
    static public bool CheckBitInNumber(int bit, int number)
    {
        bit = (int)Math.Pow(2, bit);

        for (int i = 31; i >= 0; i--)
        {
            if (Math.Pow(2, i) == bit)
            {
                if (number - Math.Pow(2, i) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (number - Math.Pow(2, i) > 0)
                {
                    number -= (int)Math.Pow(2, i);
                    continue;
                }
                else
                {
                    continue;
                }
            }
        }

        return false;
    }

    static public List<GameObject> GetObjectsInBoxZoneByTagsAndLayerMask(Vector2 origin, Vector2 size, List<string> tags, int layer)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, size, size.x + size.y, Vector2.zero, 100, layer);
        return GetHitsByTags(tags, hits);
    }

    static public List<GameObject> GetObjectsInCircleZoneByTagsAndLayerMask(Vector2 origin, float radius, List<string> tags, int layer)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, radius, Vector2.zero, radius, layer);
        return GetHitsByTags(tags, hits);
    }

    static public List<GameObject> GetHitsByTags(List<string> tags, RaycastHit2D[] hits)
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

    static public GameObject GetNearestObject(Vector2 origin, List<GameObject> gameObjects)
    {
        if (gameObjects.Count > 0)
        {
            GameObject nearestObject = gameObjects[0];
            float nearestDistance = float.MaxValue;

            foreach (var item in gameObjects)
            {
                var currentDistance = Vector2.Distance(origin, item.transform.position);

                if (currentDistance < nearestDistance)
                {
                    nearestDistance = currentDistance;
                    nearestObject = item;
                }
            }

            return nearestObject;
        }

        return null;
    }
}
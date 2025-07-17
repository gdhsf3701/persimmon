using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ScetchBookDrawing : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    int pointCounter = 0;
    int currentPoint = 0;
    [SerializeField] int pointCheckingFrequency = 4;
    [SerializeField] private List<Vector3> linePoints = new List<Vector3>();
    public event Action<List<Vector3>> linePointsChanged;
    [SerializeField] private LineRenderer pen;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("start");
        pointCounter = 0;
        pen.positionCount = 0;
        currentPoint = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        pointCounter++;
        if(pointCounter % pointCheckingFrequency == 0)
        {
            Vector3 worldPos = eventData.position;
            worldPos = Camera.main.ScreenToWorldPoint(worldPos);
            worldPos.z = 0;
            linePoints.Add(worldPos);
            pen.positionCount++;
            pen.SetPosition(currentPoint++,worldPos);
            pointCounter = 0;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        linePointsChanged?.Invoke(linePoints);
    }
}

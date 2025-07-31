using System;
using System.Collections.Generic;
using System.IO;
using PDollar_drowingTool.Scripts;
using PDollarGestureRecognizer;
using UnityEngine;

namespace CSI._01_Code
{
	public class CheckImage : MonoBehaviour
	{
    
		private List<Gesture> trainingSet = new List<Gesture>();

		private List<Point> points = new List<Point>();
		private int strokeId = -1;

		private Vector3 virtualKeyPosition = Vector2.zero;
		private Vector3 currentvirtualKeyPosition = Vector2.zero;
		private Rect drawArea;

		private int vertexCount = 0;

		[SerializeField] private LineRenderer currentGestureLineRenderer;
	
		private bool recognized;
		
		private float drowingTime;
    
		void Start()
		{
			drawArea = new Rect(0, 0, Screen.width, Screen.height);

			//Load pre-made gestures
			TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>($"GestureSet/10-stylus-MEDIUM/");
			foreach (TextAsset gestureXml in gesturesXml)
				trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

			//Load user custom gestures
			string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
			foreach (string filePath in filePaths)
				trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
		}

		void Update()
		{
			if (Input.GetMouseButton(0))
			{
				
				virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
			}

			if (drawArea.Contains(virtualKeyPosition)) // 그리는 영역에 들어왔을 때
			{
				if (Input.GetMouseButtonDown(0))
				{
					if (recognized)
					{
						SetTrailColor(Color.yellow);
						drowingTime = 0;
						recognized = false;
						strokeId = -1;
						points.Clear();
						currentGestureLineRenderer.positionCount = 0;
						vertexCount = 0;
					}
					

				}
				if (Input.GetMouseButton(0))
				{
					if (virtualKeyPosition != currentvirtualKeyPosition)
					{
						drowingTime++;
						currentvirtualKeyPosition = virtualKeyPosition;
					}
					points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));
					currentGestureLineRenderer.colorGradient = new Gradient();
					currentGestureLineRenderer.positionCount = ++vertexCount;
					currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
					Gesture candidate = new Gesture(points.ToArray());
					Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

					Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				recognized = true;
				Gesture candidate = new Gesture(points.ToArray());
				Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

				Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);
			}
		}

		private void SaveData(string newGestureName = "")
		{
			string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, newGestureName, DateTime.Now.ToFileTime());

			GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);
		}

		private void SetTrailColor(Color color)
		{
			Gradient gradient = new Gradient();
			gradient.colorKeys = new GradientColorKey[2];
			gradient.colorKeys[0].color = color;
			gradient.colorKeys[0].time = 0;
			gradient.colorKeys[1].color = color;
			gradient.colorKeys[1].time = 1;
			gradient.alphaKeys = new GradientAlphaKey[2];
			gradient.alphaKeys[0].alpha = 1;
			gradient.alphaKeys[0].time = 0;
			gradient.alphaKeys[1].alpha = 1;
			gradient.alphaKeys[1].time = 1;
			gradient.SetKeys(gradient.colorKeys, gradient.alphaKeys);
			currentGestureLineRenderer.colorGradient = gradient;
			currentGestureLineRenderer.startColor = color;
			currentGestureLineRenderer.endColor = color;
		}
	}
}

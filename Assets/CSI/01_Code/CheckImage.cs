using System;
using System.Collections.Generic;
using System.IO;
using PDollar_drowingTool.Scripts;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.Events;
using Work.Bakbak.Code.Shape;

namespace CSI._01_Code
{
	public class CheckImage : MonoBehaviour
	{
    
		private List<Gesture> trainingSet = new List<Gesture>();

		private List<Point> points = new List<Point>();
		private int strokeId = -1;

		private Vector3 virtualKeyPosition = Vector2.zero;
		private Vector3 currentvirtualKeyPosition = Vector2.zero;

		private int vertexCount = 0;

		[SerializeField] private LineRenderer currentGestureLineRenderer;
	
		private bool recognized = true;
		
		private float drowingTime;
 
		public UnityEvent<ShapType> DrawedEvent;

		[SerializeField] private ShapeSO line;
		[SerializeField] private ShapeSO hLine;
		[SerializeField] private ShapeSO underCheck;
		[SerializeField] private ShapeSO upperCheck;
		[SerializeField] private ShapeSO star;
		[SerializeField] private ShapeSO circle;
		
    
		void Start()
		{
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
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0;
			Vector3 rayDirection = (mousePosition - Camera.main.transform.position).normalized;
			bool hit;
			hit = Physics.Raycast(Camera.main.transform.position, rayDirection , 100f);
			Debug.DrawRay(Camera.main.transform.position, rayDirection * 100f);
			if (hit) // 그리는 영역에 들어왔을 때
			{
				if (Input.GetMouseButtonDown(0))
				{
					if (recognized)
					{
						ResetDraw();
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
					currentGestureLineRenderer.positionCount = ++vertexCount;
					currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
					if (points.Count <= 0 || drowingTime < 5) return;
					
				}
				if (Input.GetMouseButtonUp(0))
				{
					if (points.Count <= 0 || drowingTime < 5)
					{
						ResetDraw();
						return;
					}
					recognized = true;
					Gesture candidate = new Gesture(points.ToArray());
					Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

					Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);
					if (gestureResult.Score > 0.7f)
					{
						DrawedEvent?.Invoke(StringToShapType(gestureResult.GestureClass));
					}
				}
			}
			else
			{
				if (points.Count <= 0|| drowingTime < 5)
				{
					ResetDraw();
					return;
				}
						

			}
			
		}

		private void ResetDraw()
		{
			recognized = true;
			strokeId = -1;
			points.Clear();
			currentGestureLineRenderer.positionCount = 0;
			SetTrailColor(Color.yellow);
			vertexCount = 0;
			drowingTime = 0;
		}

		private void SaveData(string newGestureName = "")
		{
			string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, newGestureName, DateTime.Now.ToFileTime());

			GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);
		}

		private ShapType StringToShapType(string shape)
		{
			ShapType shapType = ShapType.HLine;
			switch (shape)
			{
				case "HLine":
					shapType = ShapType.HLine;
					break;
				case "Line":
					shapType = ShapType.Line;
					break;
				case "O":
					shapType = ShapType.Circle;
					break;
				case "star":
					shapType = ShapType.Star;
					break;
				case "UnderCheck":
					shapType = ShapType.UnderCheck;
					break;
				case "UperCheck":
					shapType = ShapType.UpperCheck;
					break;
			}

			return shapType;
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

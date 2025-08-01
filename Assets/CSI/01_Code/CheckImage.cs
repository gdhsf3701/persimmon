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
		[SerializeField] private ParticleSystem particleSystem;

		[SerializeField] private ShapeSO line;
		[SerializeField] private ShapeSO hLine;
		[SerializeField] private ShapeSO underCheck;
		[SerializeField] private ShapeSO upperCheck;
		[SerializeField] private ShapeSO star;
		[SerializeField] private ShapeSO circle;
		[SerializeField] private ShapeSO eleck;
		
		ParticleSystem.MainModule main;
    
		void Start()
		{
			main = particleSystem.main;
			//Load pre-made gestures
			TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>($"GestureSet/10-stylus-MEDIUM/");
			foreach (TextAsset gestureXml in gesturesXml)
				trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

			//Load user custom gestures
			string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
			foreach (string filePath in filePaths)
				trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
		}

		void FixedUpdate()
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
						particleSystem.Play();
						Vector3 pos = Camera.main.ScreenToWorldPoint(virtualKeyPosition);
						particleSystem.transform.position = new Vector3(pos.x, pos.y, 0);
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
					if (vertexCount <= 0 || drowingTime < 5) return;
					Gesture candidate = new Gesture(points.ToArray());
					Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
					ShapeSO shapType = GetShapeSo(StringToShapType(gestureResult.GestureClass));
					Vector3 pos = Camera.main.ScreenToWorldPoint(virtualKeyPosition);
					if (particleSystem.isPlaying == false)
					{
						particleSystem.Play();
					}
					particleSystem.transform.position = new Vector3(pos.x, pos.y, 0);
					if (gestureResult.Score > 0.7f)
					{
						SetTrailColor(shapType.Color, drowingTime/30);
						main.startColor = shapType.Color;
						main.startColor = shapType.Color;
					}
					else
					{
						SetTrailColor(Color.white, drowingTime/30);
						main.startColor = Color.white;
					}
				}
			}
			else
			{
				ResetDraw();
			}

		}

		private void Update()
		{
			if (Input.GetMouseButtonUp(0))
			{
				if (vertexCount <= 0 || drowingTime < 5)
				{
					ResetDraw();
					return;
				}
				Debug.Log("MouseUp");
				recognized = true;
				Gesture candidate = new Gesture(points.ToArray());
				Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

				if (gestureResult.Score > 0.7f)
				{
					Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);
					DrawedEvent?.Invoke(StringToShapType(gestureResult.GestureClass));
				}
				particleSystem.Stop();
				ResetDraw();
			}
		}

		private void ResetDraw()
		{
			recognized = true;
			strokeId = -1;
			points.Clear();
			currentGestureLineRenderer.positionCount = 0;
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
				case "Eleck":
					shapType = ShapType.Eleck;
					break;
				case "Eleck2":
					shapType = ShapType.Eleck;
					break;
			}

			return shapType;
		}

		private ShapeSO GetShapeSo(ShapType shapType)
		{
			ShapeSO shape = null;
			switch (shapType)
			{
				case ShapType.Line:
					shape = line;
					break;
				case ShapType.HLine:
					shape = hLine;
					break;
				case ShapType.UnderCheck:
					shape = underCheck;
					break;
				case ShapType.UpperCheck:
					shape = upperCheck;
					break;
				case ShapType.Star:
					shape = star;
					break;
				case ShapType.Circle:
					shape = circle;
					break;
				case ShapType.Eleck:
					shape = eleck;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(shapType), shapType, null);
			}

			return shape;
		}
		private void SetTrailColor(Color color,float alpha = 1)
		{
			alpha = Mathf.Clamp(alpha, 0.15f, 1);
			currentGestureLineRenderer.startColor = color;
			currentGestureLineRenderer.endColor = color;
			Gradient gradient = new Gradient();
			gradient.SetKeys(
				new GradientColorKey[] {
					new GradientColorKey(color, 0.0f),
					new GradientColorKey(color, 1.0f)
				},
				new GradientAlphaKey[] {
					new GradientAlphaKey(alpha, 0.0f),
					new GradientAlphaKey(alpha, 1.0f)
				}
			);
			currentGestureLineRenderer.colorGradient = gradient;
		}
	}
}

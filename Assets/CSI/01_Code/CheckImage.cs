using System.Collections.Generic;
using System.IO;
using PDollarGestureRecognizer;
using UnityEngine;

public class CheckImage : MonoBehaviour
{
    
    private List<Gesture> trainingSet = new List<Gesture>();

    private List<Point> points = new List<Point>();
    private int strokeId = -1;

    private Vector3 virtualKeyPosition = Vector2.zero;
    private Rect drawArea;

    private int vertexCount = 0;

    [SerializeField] private LineRenderer currentGestureLineRenderer;
	
    private bool recognized;
    
    void Start()
		{
			drawArea = new Rect(0, 0, Screen.width - Screen.width / 3, Screen.height);

			//Load pre-made gestures
			TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
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
						recognized = false;
						strokeId = -1;
						points.Clear();
						currentGestureLineRenderer.positionCount = 0;
						vertexCount = 0;
					}
					

				}
				if (Input.GetMouseButton(0))
				{
					points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

					currentGestureLineRenderer.positionCount = ++vertexCount;
					currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));

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
    
}

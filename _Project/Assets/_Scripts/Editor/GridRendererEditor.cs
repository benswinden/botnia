using UnityEngine;
using System.Collections;
using UnityEditor;

    [CustomEditor(typeof(GridRenderer))]
public class GridRendererEditor : Editor {

        public override void OnInspectorGUI() {            
            base.OnInspectorGUI();

            if (GUILayout.Button("Create Grid")) {
                CreateGrid();
            }            

            if (GUILayout.Button("Show Grid")) {
                showGrid();
            }

            if (GUILayout.Button("Hide Grid")) {
                hideGrid();
            }            
        }

        void showGrid() {

            // Find all line renderers and turn them on
            LineRenderer[] lineList = GameObject.FindObjectsOfType<LineRenderer>();

            foreach (LineRenderer line in lineList) {
                line.GetComponent<LineRenderer>().enabled = true;
            }
        }

        void hideGrid() {
            
            // Find all line renderers and turn them off            
            LineRenderer[] lineList = GameObject.FindObjectsOfType<LineRenderer>();

            foreach (LineRenderer line in lineList) {
                line.GetComponent<LineRenderer>().enabled = false;
            }
        }       
        
        void CreateGrid() {

            GameObject gridRenderer = GameObject.Find("GridRenderer");

            // Instantiate horizontal lines
            for (int i = 0; i < 100; i++) {

                LineRenderer line = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
                line.name = "GridLine";
                line.SetVertexCount(2);
                line.SetPosition(0, new Vector3(0 , -i, -1));
                line.SetPosition(1, new Vector3(100, -i, -1));
                line.SetWidth(0.03f, 0.03f);                

                line.transform.parent = gridRenderer.transform;
            }

            // Instantiate vertical lines
            for (int i = 0; i < 100; i++) {

                LineRenderer line = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
                line.name = "GridLine";
                line.SetVertexCount(2);
                line.SetPosition(0, new Vector3(i, 0, -1));
                line.SetPosition(1, new Vector3(i, -100, -1));
                line.SetWidth(0.03f, 0.03f);                

                line.transform.parent = gridRenderer.transform;
            }
        }
}

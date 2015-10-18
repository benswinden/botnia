using UnityEngine;
using System.Collections;
using UnityEditor;

    [CustomEditor(typeof(ComponentExample))]
public class ComponentExampleEditor : Editor {

        public override void OnInspectorGUI() {            
            base.OnInspectorGUI();

            if (GUILayout.Button("Sort Blocks")) {
                SortBlocks();
            }

            if (GUILayout.Button("Show Markers")) {
                ShowMarkers();
            }

            if (GUILayout.Button("Hide Markers")) {
                HideMarkers();
            }

            if (GUILayout.Button("Show Grid")) {
                showGrid();
            }

            if (GUILayout.Button("Hide Grid")) {
                hideGrid();
            }

            if (GUILayout.Button("Create Grid")) {
                CreateGrid();
            }            
        }


        void ShowMarkers() {

            GameObject[] blockList = GameObject.FindGameObjectsWithTag("Ground");

            foreach (GameObject block in blockList) {
                
                if (block.name.Substring(0,5).Equals("Block"))
                    block.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

        void HideMarkers() {

            GameObject[] blockList = GameObject.FindGameObjectsWithTag("Ground");

            foreach (GameObject block in blockList) {
                if (block.name.Substring(0, 5).Equals("Block"))
                    block.GetComponent<SpriteRenderer>().enabled = false;
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

        // Note gets the sprite objects which are children of the actual block objects, necessary in order to have direct access to the sprite renderer in order to set that component dirty
        void SortBlocks() {

            // Find all the block objects and set their sorting order according to their position
            GameObject[] blockList = GameObject.FindGameObjectsWithTag("Ground");

            foreach (GameObject block in blockList) {

                // Check whether this is a parent or child ground object
                if (block.transform.parent == null) {
                 
                    block.GetComponent<SpriteRenderer>().sortingOrder = -(int)block.transform.position.y;
                }
                else {

                    block.GetComponent<SpriteRenderer>().sortingOrder = -(int)block.transform.parent.position.y;
                }
                EditorUtility.SetDirty(block.GetComponent<SpriteRenderer>());
            }
        }
        
        void CreateGrid() {

            GameObject gridRenderer = GameObject.Find("__GridRenderer");

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

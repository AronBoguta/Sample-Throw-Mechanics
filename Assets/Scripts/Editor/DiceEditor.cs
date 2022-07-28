using UnityEditor;
using UnityEngine;

namespace DiceRoll
{
    [CustomEditor(typeof(EditableDie))]
    public class DiceEditor : Editor
    {
        private EditableDie die;
        private int hitStatus = -1;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            die = target as EditableDie;
            if (GUILayout.Button("Align all faces"))
            {
                OnAlignButton();
            }
        }

        private void OnAlignButton()
        {
            CalculateAllCenters();
            AllignAllFaces();
        }

        private void AllignAllFaces()
        {
            for (int faceID = 0; faceID < die.DieFaces.Count; faceID++)
            {
                AlignFaceWithDie(faceID);
            }
        }

        private void AlignFaceWithDie(int faceID)
        {
            int nearestTriangle = GetNearestTriangleID(faceID);
            die.DieFaces[faceID].transform.position = die.TriangleCenters[nearestTriangle];
        }

        private int GetNearestTriangleID(int faceID)
        {
            int nearestTriangleID = 0;
            float distance = float.MaxValue;

            Vector3 dieFacePosition = die.DieFaces[faceID].transform.position;

            for (int i = 0; i < die.TriangleCenters.Count; i++)
            {
                float distanceToCenter = (dieFacePosition - die.TriangleCenters[i]).magnitude;
                if (distance > distanceToCenter)
                {
                    nearestTriangleID = i;
                    distance = distanceToCenter;
                }
            }

            return nearestTriangleID;
        }

        private void CalculateAllCenters()
        {
            if (die.TriangleCenters.Count == 0)
            {
                for (int vertexId = 0; vertexId < die.ColliderTriangles.Length; vertexId += 3)
                {
                    die.TriangleCenters.Add(CalculateCenter(vertexId));
                }
            }
        }

        public Vector3 CalculateCenter(int firstVertId)
        {
            var point1 = die.ColliderVerts[die.ColliderTriangles[firstVertId]];
            var point2 = die.ColliderVerts[die.ColliderTriangles[firstVertId + 1]];
            var point3 = die.ColliderVerts[die.ColliderTriangles[firstVertId + 2]];

            return (point1 + point2 + point3) / 3.0f;
        }
    }
}
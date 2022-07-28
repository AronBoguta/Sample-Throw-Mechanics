using System;
using UnityEngine;

namespace DiceRoll
{
    [RequireComponent(typeof(Rigidbody))]
    public class RollableDie : MonoBehaviour, IRollable
    {
        [SerializeField] private EditableDie editableDie;

        public Action<bool> RollingStopped
        {
            get { return rollingStopped; }
            set { rollingStopped = value; }
        }

        private Action<bool> rollingStopped;

        public Rigidbody Rigidbody => rigidbody;
        private Rigidbody rigidbody;

        private bool isRolling = false;
        private bool faceChanged = false;
        private FacePoints topFace;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            topFace = GetTopFace(editableDie.DieFaces[0]);
        }

        private void Update()
        {
            if (!isRolling) return; //Return immediately if die is not rolling

            var newTopFace = GetTopFace(topFace);
            if (topFace != newTopFace)
            {
                topFace = newTopFace;
                faceChanged = true;
            }

            if (isRolling && rigidbody.IsSleeping())
            {
                isRolling = false;
                rollingStopped.Invoke(faceChanged);
                faceChanged = false;
            }
        }

        public void StartRolling()
        {
            isRolling = true;
        }

        public int GetScore()
        {
            return topFace.Points;
        }

        public void SimulateThrow(Vector3 direction, float force)
        {
            rigidbody.AddForceAtPosition(direction * force, rigidbody.centerOfMass);
        }

        public FacePoints GetTopFace(FacePoints comparedFace)
        {
            var result = comparedFace;

            foreach (var face in editableDie.DieFaces)
            {
                if (comparedFace.transform.position.y < face.transform.position.y)
                {
                    result = face;
                }
            }

            return result;
        }
    }
}
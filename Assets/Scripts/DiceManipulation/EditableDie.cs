using System.Collections.Generic;
using UnityEngine;

namespace DiceRoll
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshCollider))]
    public class EditableDie : MonoBehaviour
    {
        [SerializeField] private List<FacePoints> dieFaces;
        [SerializeField] private MeshCollider meshCollider;

        public List<FacePoints> DieFaces => dieFaces;
        public int[] ColliderTriangles => meshCollider.sharedMesh.triangles;
        public Vector3[] ColliderVerts => meshCollider.sharedMesh.vertices;

        public List<Vector3> TriangleCenters
        {
            get { return triangleCenters; }
            set { triangleCenters = value; }
        }

        private List<Vector3> triangleCenters = new List<Vector3>();

        private Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            meshCollider = GetComponent<MeshCollider>();
        }
    }
}
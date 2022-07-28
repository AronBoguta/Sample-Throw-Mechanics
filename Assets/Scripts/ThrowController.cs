using System.Linq;
using UnityEngine;

namespace DiceRoll
{
    public class ThrowController : MonoBehaviour
    {
        [SerializeField] private int layerId;
        [SerializeField] private Joint joint;
        [SerializeField] private float simulatedThrowForce;

        //This line would be replaced by DI
        [SerializeField] private ScoreController scoreController;

        private int layerMask;
        private IRollable objectSelected;
        private Rigidbody selectedRigidBody;
        private Rigidbody rigidbody;
        private ThrowState throwState = ThrowState.Waiting;
        private Vector3 mOffset;
        private float mZCoord;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            layerMask = 1 << layerId;
            scoreController.AddThrowButtonListener(ForceThrow);
        }

        private void Update()
        {
            HandleMouse();
        }

        private void FixedUpdate()
        {
            if (objectSelected != null && throwState!=ThrowState.Rolling)
            {
                rigidbody.MovePosition(Utils.GetMouseAsWorldPoint() + mOffset);
            }
        }

        private void HandleMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CanPickUpDie())
                {
                    (objectSelected, selectedRigidBody) = Utils.DetectObjectUnderCursor(layerMask);
                    if (objectSelected != null)
                    {
                        transform.position = selectedRigidBody.transform.position;
                        rigidbody.position = transform.position;

                        joint.connectedBody = selectedRigidBody;
                        mOffset = transform.position - Utils.GetMouseAsWorldPoint();
                    }
                }
            }
            else if (CanThrowDie())
            {
                SetupThrow();
            }
        }

        private bool CanThrowDie()
        {
            return objectSelected != null && throwState == ThrowState.Waiting && Input.GetMouseButtonUp(0);
        }

        private bool CanPickUpDie()
        {
            return throwState == ThrowState.Waiting && objectSelected == null;
        }

        private void ResetObjectGravity()
        {
            selectedRigidBody.useGravity = false;
            selectedRigidBody.useGravity = true;
        }

        private void SetupThrow()
        {
            throwState = ThrowState.Rolling;
            scoreController.SetRollingText();

            joint.connectedBody = null;
            ResetObjectGravity();

            objectSelected.StartRolling();
            objectSelected.RollingStopped += OnRollingStopped;
        }

        private void ForceThrow()
        {
            if (throwState != ThrowState.Waiting) return;

            if (objectSelected == null)
            {
                objectSelected = FindObjectsOfType<MonoBehaviour>().OfType<IRollable>().First();
                selectedRigidBody = objectSelected.Rigidbody;
            }

            var throwDirection = new Vector3(Random.value-0.5f, Random.value-0.5f, Random.value-0.5f).normalized;
            objectSelected.SimulateThrow(throwDirection, simulatedThrowForce);
        }

        private void OnRollingStopped(bool rollSuccess)
        {
            throwState = rollSuccess ? ThrowState.Success : ThrowState.Fail;
            
            if (throwState == ThrowState.Success)
            {
                scoreController.UpdateScore(objectSelected.GetScore());
            }

            objectSelected.RollingStopped -= OnRollingStopped;
            objectSelected = null;
            throwState = ThrowState.Waiting;
        }
    }

//This would be typically replaced by a state machine
    public enum ThrowState
    {
        Waiting = 0,
        Rolling,
        Success,
        Fail
    }
}
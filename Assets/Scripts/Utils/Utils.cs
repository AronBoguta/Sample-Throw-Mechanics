using UnityEngine;

namespace DiceRoll
{
    public class Utils
    {
        private static float mZCoord;
        
        public static (IRollable, Rigidbody) DetectObjectUnderCursor(int layerMask)
        {
            IRollable objectSelected;
            Rigidbody selectedRigidBody;
        
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 1000.0f, layerMask))
            {
                objectSelected = hitInfo.transform.gameObject.GetComponent<IRollable>();
                if (objectSelected != null)
                {
                    selectedRigidBody = hitInfo.rigidbody;
                    mZCoord = hitInfo.distance;
                    return (objectSelected, selectedRigidBody);
                }
            }

            return (null, null);
        }
    
        public static Vector3 GetMouseAsWorldPoint()
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = mZCoord;
        
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }
    }
}


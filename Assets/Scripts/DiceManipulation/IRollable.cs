using System;
using UnityEngine;

namespace DiceRoll
{
    public interface IRollable
    {
        public Action<bool> RollingStopped { get; set; }
        public void StartRolling();
        public int GetScore();
        public void SimulateThrow(Vector3 direction, float force);
        public Rigidbody Rigidbody { get; }
    }
}
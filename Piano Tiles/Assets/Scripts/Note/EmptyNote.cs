using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.InGame.Note
{
    public class EmptyNote : Note
    {
        // Primitive attributes
        private int m_length;

        public override void Init(int length = 1, int pos = -1)
        {
            base.Init(length, pos);
            m_length = length;
        }

        public override void UnlockNote()
        {
            CompleteNote();
        }

        public override float GetLength()
        {
            return m_length * k_noteLengthUnit;
        }

        public override void OnTouchEnter(Vector2 touchPosition)
        {
            
        }

        public override void OnTouchStay(Vector2 touchPosition)
        {

        }

        public override void OnTouchExit()
        {

        }
    }
}

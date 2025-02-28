using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.InGame.Note
{
    public class NormalNote : Note
    {
        protected override void Start()
        {
            base.Start();
            GetComponent<BoxCollider2D>().size += Vector2.up * k_errorNoteSize;
        }

        public override float GetLength()
        {
            return k_noteLengthUnit;
        }

        public override void OnTouchEnter(Vector2 touchPosition)
        {
            if (m_canPress)
            {
                GetComponent<SpriteRenderer>().color = m_pressedColor;
                m_canPress = false;
                CompleteNote();
                GameManager.Instance.AddScore();
            }
        }

        public override void OnTouchStay(Vector2 touchPosition)
        {
            
        }

        public override void OnTouchExit()
        {
            
        }
    }
}

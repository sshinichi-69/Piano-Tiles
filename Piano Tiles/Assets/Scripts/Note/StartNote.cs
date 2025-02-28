using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.InGame.Note
{
    public class StartNote : Note
    {
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
                GameManager.Instance.PlayGame();
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

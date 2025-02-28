using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PianoTiles.InGame.Note
{
    public class LongNote : Note
    {
        // Primitive attributes
        private int m_length;
        private int m_canStayIdx = 0;

        // Unity attributes
        [SerializeField] private GameObject m_secondElemPrefab;
        [SerializeField] private GameObject m_thirdElemPrefab;

        public override void Init(int length = 1, int pos = -1)
        {
            // set position and length attribute
            base.Init(length, pos);
            // set note length
            float noteLength = k_noteLengthUnit;
            m_length = length;
            for (int i = 1; i < m_length - 1; i++)
            {
                Vector3 secondElemPos = transform.position + Vector3.up * i * noteLength;
                GameObject secondElem = Instantiate(m_secondElemPrefab, secondElemPos, Quaternion.identity);
                secondElem.transform.SetParent(transform);
            }
            Vector3 thirdElemPos = transform.position + Vector3.up * (m_length - 1) * noteLength;
            GameObject thirdElem = Instantiate(m_thirdElemPrefab, thirdElemPos, Quaternion.identity);
            thirdElem.transform.SetParent(transform);
            // set collider length
            GetComponent<BoxCollider2D>().offset = new Vector2(0, (k_noteLengthUnit / 2) * (m_length - 1));
            GetComponent<BoxCollider2D>().size = new Vector2(1, k_noteLengthUnit * m_length);
            GetComponent<BoxCollider2D>().size += Vector2.up * k_errorNoteSize;
        }

        public override float GetLength()
        {
            return m_length * k_noteLengthUnit;
        }

        public override void OnTouchEnter(Vector2 touchPosition)
        {
            if (m_canPress)
            {
                if (IsCastChildNote(touchPosition, 0, true))
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().color = m_pressedColor;
                    m_canPress = false;
                    m_canStayIdx++;
                    CompleteNote();
                    GameManager.Instance.AddScore();
                }
            }
        }

        public override void OnTouchStay(Vector2 touchPosition)
        {
            if (m_canStayIdx > 0 && m_canStayIdx < transform.childCount)
            {
                if (IsCastChildNote(touchPosition, m_canStayIdx, false))
                {
                    transform.GetChild(m_canStayIdx).GetComponent<SpriteRenderer>().color = m_pressedColor;
                    m_canStayIdx++;
                    GameManager.Instance.AddScore();
                }
            }
        }

        public override void OnTouchExit()
        {
            if (m_canStayIdx > 0)
            {
                m_canStayIdx = transform.childCount;
            }
        }

        private bool IsCastChildNote(Vector2 pos, int childNoteIdx, bool isEnter)
        {
            Transform childNoteTransform = transform.GetChild(childNoteIdx);
            Vector3 a = childNoteTransform.position - childNoteTransform.localScale / 2 - Vector3.up * k_errorNoteSize;
            Vector3 b = childNoteTransform.position + childNoteTransform.localScale / 2 + Vector3.up * k_errorNoteSize;
            //if (isEnter)
            //{
            //    Vector2 p = (Vector2)transform.position;
            //    var box = GetComponent<BoxCollider2D>();
            //    Vector2 a1 = p + box.offset - box.size / 2;
            //    Vector2 b1 = p + box.offset + box.size / 2;
            //    Debug.Log($"{pos} <---> {a}; {b} <---> {a1}; {b1}");
            //}
            if ((a.x <= pos.x && pos.x <= b.x) && (a.y <= pos.y && pos.y <= b.y))
            {
                return true;
            }
            return false;
        }
    }
}

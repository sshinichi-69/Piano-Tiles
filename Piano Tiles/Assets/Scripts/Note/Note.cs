using UnityEngine;

namespace PianoTiles.InGame.Note
{
    public abstract class Note : MonoBehaviour
    {
        // Primitive attributes
        protected int m_pos; // 0 -> 3
        protected bool m_canPress = false;

        // Constant
        protected static float k_noteLengthUnit = 1.25f;
        protected static float k_errorNoteSize = 0;

        // Unity attributes
        [SerializeField] protected Color m_notPressedColor;
        protected Color m_pressedColor;

        protected void Awake()
        {
            m_pressedColor = m_notPressedColor + (Color.white - m_notPressedColor) * 0.75f;
        }

        protected virtual void Start()
        {
            float velocity = transform.parent.GetComponent<NoteGroup>().Velocity;
            k_errorNoteSize = velocity * NoteManager.Instance.NoteDelayTime * 2;
        }

        public abstract void OnTouchEnter(Vector2 touchPosition);
        public abstract void OnTouchStay(Vector2 touchPosition);
        public abstract void OnTouchExit();
        public abstract float GetLength();

        public virtual void Init(int length = 1, int pos = -1)
        {
            SetupPos(pos);
        }

        public virtual void UnlockNote()
        {
            m_canPress = true;
        }

        protected void SetupPos(int pos)
        {
            m_pos = pos;
            transform.position += Vector3.right * (pos - 1.5f);
        }

        protected void CompleteNote()
        {
            transform.parent.GetComponent<NoteGroup>().OnChildNoteComplete();
        }

        // Get, set
        public int Pos { get { return m_pos; } }
        public static float NoteLengthUnit { get { return k_noteLengthUnit; } }
    }
}

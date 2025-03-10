using PianoTiles.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.InGame.Note
{
    public class NoteGroup : MonoBehaviour
    {
        // Primitive attributes
        private List<Note> notes;
        private int m_nCompleteNote = 0;
        private float m_velocity = 0;

        // Constant
        private float k_lossPos = -4.25f;

        // Unity attributes
        [SerializeField] private StartNote m_startNotePrefab;
        [SerializeField] private EmptyNote m_emptyNotePrefab;
        [SerializeField] private NormalNote m_normalNotePrefab;
        [SerializeField] private LongNote m_longNotePrefab;

        private void Awake()
        {
            k_lossPos = -4f / Screen.width * Screen.height / 2;

            notes = new();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GameState == GameStateType.PLAYING)
            {
                transform.Translate(m_velocity * Time.deltaTime * Vector3.down);
                if (transform.position.y <= k_lossPos - GetLength())
                {
                    DestroyObject();
                }
                else if (transform.position.y <= k_lossPos && m_nCompleteNote < transform.childCount)
                {
                    GameManager.Instance.EndGame();
                }
            }
        }

        // Public method
        public void Init(NoteGroupConfig noteGroupConfig, List<int> banPositions, float velocity)
        {
            m_velocity = velocity * 1.25f;
            // calc note positions
            List<int> positions = new() { 0, 1, 2, 3 };
            foreach (int banPos in banPositions)
            {
                positions.Remove(banPos);
            }
            {
                int pos;
                switch (noteGroupConfig.notes.Count)
                {
                    case 1:
                        pos = positions[Random.Range(0, positions.Count)];
                        positions.Clear();
                        positions.Add(pos);
                        break;
                    case 2:
                        if (positions.Count == 4)
                        {
                            pos = Random.Range(0, 2);
                            positions.Remove(pos);
                            positions.Remove(pos + 2);
                        }
                        else
                        {
                            int firstNoteIdx = 0;
                            int secondNoteIdx = 1;
                            if (positions.Contains(firstNoteIdx) && positions.Contains(firstNoteIdx + 2))
                            {
                                positions.Remove(secondNoteIdx);
                                positions.Remove(secondNoteIdx + 2);
                            }
                            else
                            {
                                positions.Remove(firstNoteIdx);
                                positions.Remove(firstNoteIdx + 2);
                            }
                        }
                        break;
                }
            }
            // generate notes
            foreach (NoteConfig noteConfig in noteGroupConfig.notes)
            {
                GameObject note = null;
                int noteLength = 1;
                switch (noteConfig.type)
                {
                    case NoteType.START:
                        note = Instantiate(m_startNotePrefab.gameObject, transform.position, Quaternion.identity);
                        break;
                    case NoteType.EMPTY:
                        note = Instantiate(m_emptyNotePrefab.gameObject, transform.position, Quaternion.identity);
                        noteLength = noteConfig.length;
                        break;
                    case NoteType.NORMAL:
                        note = Instantiate(m_normalNotePrefab.gameObject, transform.position, Quaternion.identity);
                        break;
                    case NoteType.LONG:
                        note = Instantiate(m_longNotePrefab.gameObject, transform.position, Quaternion.identity);
                        noteLength = noteConfig.length;
                        break;
                }
                int posIdx = Random.Range(0, positions.Count);
                if (noteConfig.type == NoteType.EMPTY)
                {
                    note.GetComponent<Note>().Init(noteLength);
                }
                else
                {
                    note.GetComponent<Note>().Init(noteLength, positions[posIdx]);
                    positions.RemoveAt(posIdx);
                }
                note.transform.SetParent(transform);
                notes.Add(note.GetComponent<Note>());
            }
        }

        public float GetLength()
        {
            float maxLength = 0;
            foreach (var note in notes)
            {
                float noteLength = note.GetLength();
                if (noteLength > maxLength)
                {
                    maxLength = noteLength;
                }
            }
            return maxLength;
        }

        public List<int> GetPos()
        {
            List<int> pos = new();
            foreach (Note note in notes)
            {
                if (note.Pos != -1)
                {
                    pos.Add(note.Pos);
                }
            }
            return pos;
        }

        public void UnlockNote()
        {
            foreach (Note note in notes)
            {
                note.UnlockNote();
            }
        }

        public void OnChildNoteComplete()
        {
            m_nCompleteNote++;
            if (m_nCompleteNote == notes.Count)
            {
                NoteManager.Instance.OnNoteComplete();
            }
        }

        public void DestroyObject()
        {
            NoteManager.Instance.RemoveNoteGroup();
            Destroy(gameObject);
        }

        public float Velocity { get { return m_velocity; } set { m_velocity = value * 1.25f; } }
    }
}

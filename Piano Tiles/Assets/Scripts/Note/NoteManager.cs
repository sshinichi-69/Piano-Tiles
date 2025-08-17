using PianoTiles.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PianoTiles.InGame.Note
{
    public class NoteManager : MonoBehaviour
    {
        private static NoteManager m_instance;

        // Primitive attributes
        private List<NoteGroup> m_groups;
        private int m_noteGroupIdx = 0;
        private int m_noteGroupPressedIdx = 0;
        private float m_distanceLastNoteToGenNotePos = 0;
        private List<Note> m_holdingNotes;
        private bool m_isUnlockGeneratedNote = false;
        private int m_noteVelocityIdx = 0;

        // Constants
        private Vector3 k_noteCreatedPos;
        private Vector3 k_firstNoteCreatedPos;
        private const float m_noteDelayTime = 0.1f;

        // Unity attributes
        private SongNoteConfig m_songNoteConfig;
        [SerializeField] private NoteGroup m_noteGroupPrefab;
        [SerializeField] private NoteGroupConfig m_startNoteGroupConfig;

        private void Awake()
        {
            k_noteCreatedPos = Vector3.up * 10f;
            k_firstNoteCreatedPos = Vector3.up * -1.25f;
            m_distanceLastNoteToGenNotePos = (k_noteCreatedPos - k_firstNoteCreatedPos).y;
            m_holdingNotes = new List<Note>();
            m_groups = new List<NoteGroup>();

            m_songNoteConfig = SharedData.Instance.GetSong();
            

            if (m_instance != null)
            {
                m_instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            CreateStartNote();
            SetupNote();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GameState != GameStateType.ENDING)
            {
                if (Input.touchCount > 0)
                {
                    List<Note> newHoldingNote = new();
                    foreach (Touch touch in Input.touches)
                    {
                        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);
                        if (hitCollider != null && hitCollider.gameObject.CompareTag("Note"))
                        {
                            Note touchNote = hitCollider.gameObject.GetComponent<Note>();
                            switch (touch.phase)
                            {
                                case TouchPhase.Began:
                                    touchNote.OnTouchEnter(touchPosition);
                                    break;
                                case TouchPhase.Stationary:
                                case TouchPhase.Moved:
                                    touchNote.OnTouchStay(touchPosition);
                                    newHoldingNote.Add(touchNote);
                                    break;
                                case TouchPhase.Ended:
                                    touchNote.OnTouchExit();
                                    break;
                            }
                        }
                    }
                    foreach (Note note in m_holdingNotes)
                    {
                        if (!newHoldingNote.Contains(note))
                        {
                            note.OnTouchExit();
                        }
                    }
                    m_holdingNotes = newHoldingNote;
                }
            }
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.GameState == GameStateType.PLAYING)
            {
                if (transform.childCount > 0)
                {
                    float velocity = transform.GetChild(transform.childCount - 1).GetComponent<NoteGroup>().Velocity;
                    m_distanceLastNoteToGenNotePos += Time.deltaTime * velocity;
                    if (m_distanceLastNoteToGenNotePos >= Note.NoteLengthUnit)
                    {
                        CreateNoteGroup();
                    }
                }
            }
        }

        public void OnNoteComplete()
        {
            // change note velocity if it reach new tempo
            if (m_noteVelocityIdx < m_songNoteConfig.velocityNotesIdx.Count - 1)
            {
                if (m_noteGroupIdx - transform.childCount + m_noteGroupPressedIdx >= m_songNoteConfig.velocityNotesIdx[m_noteVelocityIdx + 1])
                {
                    SetNewNoteGroupVelocity();
                }
            }
            // modify press note pointer to the next
            m_noteGroupPressedIdx++;
            // allow to press the next note
            if (m_noteGroupPressedIdx < m_groups.Count)
            {
                m_groups[m_noteGroupPressedIdx].UnlockNote();
            }
            else
            {
                m_isUnlockGeneratedNote = true;
            }
        }

        public void RemoveNoteGroup()
        {
            m_groups.RemoveAt(0);
            m_noteGroupPressedIdx--;
            if (m_noteGroupIdx >= m_songNoteConfig.groups.Count && transform.childCount == 1)
            {
                GameManager.Instance.EndGame();
            }
        }

        public void EndGame()
        {
            foreach (NoteGroup noteGroup in m_groups)
            {
                noteGroup.transform.Translate(Vector3.up * Note.NoteLengthUnit / 2);
            }
        }

        private void SetupNote()
        {
            while (m_distanceLastNoteToGenNotePos > 0 && m_noteGroupIdx < m_songNoteConfig.groups.Count)
            {
                CreateNoteGroup();
            }
            m_groups[0].UnlockNote();
        }

        private void CreateStartNote()
        {
            GameObject noteGroup = Instantiate(m_noteGroupPrefab.gameObject, k_firstNoteCreatedPos, Quaternion.identity);
            noteGroup.GetComponent<NoteGroup>().Init(m_startNoteGroupConfig, new List<int>(), m_songNoteConfig.velocities[m_noteVelocityIdx]);
            noteGroup.transform.SetParent(transform);
            m_groups.Add(noteGroup.GetComponent<NoteGroup>());
            m_distanceLastNoteToGenNotePos -= noteGroup.GetComponent<NoteGroup>().GetLength();
        }

        private void CreateNoteGroup()
        {
            if (m_noteGroupIdx < m_songNoteConfig.groups.Count)
            {
                NoteGroup lastNoteGroup = m_groups[^1];
                Vector3 noteGroupPos = lastNoteGroup.transform.position + Vector3.up * lastNoteGroup.GetLength();
                List<int> banPos = lastNoteGroup.GetPos();

                GameObject noteGroup = Instantiate(m_noteGroupPrefab.gameObject, noteGroupPos, Quaternion.identity);
                noteGroup.GetComponent<NoteGroup>().Init(m_songNoteConfig.groups[m_noteGroupIdx], banPos, m_songNoteConfig.velocities[m_noteVelocityIdx]);
                noteGroup.transform.SetParent(transform);
                m_groups.Add(noteGroup.GetComponent<NoteGroup>());
                m_noteGroupIdx++;
                m_distanceLastNoteToGenNotePos -= noteGroup.GetComponent<NoteGroup>().GetLength();

                if (m_isUnlockGeneratedNote)
                {
                    noteGroup.GetComponent<NoteGroup>().UnlockNote();
                    m_isUnlockGeneratedNote = false;
                }
            }
        }

        private void SetNewNoteGroupVelocity()
        {
            if (m_noteVelocityIdx < m_songNoteConfig.velocityNotesIdx.Count - 1)
            {
                m_noteVelocityIdx++;
                float newVelocity = m_songNoteConfig.velocities[m_noteVelocityIdx];
                foreach (Transform noteGroup in transform)
                {
                    noteGroup.GetComponent<NoteGroup>().Velocity = newVelocity;
                }
            }
        }

        public float NoteDelayTime { get { return m_noteDelayTime; } }
        public SongNoteConfig Song { get { return m_songNoteConfig; } }

        public static NoteManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindFirstObjectByType<NoteManager>();
                }
                return m_instance;
            }
        }
    }
}

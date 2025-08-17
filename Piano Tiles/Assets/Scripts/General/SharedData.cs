using PianoTiles.Config;
using PianoTiles.Local;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles
{
    public class SharedData : MonoBehaviour
    {
        private static SharedData m_instance;

        private Dictionary<int, int> m_scores;
        private int m_selectingSongId = 0;

        [SerializeField] private SongsConfig m_songs;

        private void Awake()
        {
            LoadScore();

            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void LoadScore()
        {
            SavedData data = SaveSystem.Load();

            m_scores = new Dictionary<int, int>();
            for (int i = 0; i < data.songIds.Length; i++)
            {
                m_scores.Add(data.songIds[i], data.scores[i]);
            }
            if (data.songIds.Length < m_songs.songs.Count)
            {
                int dataLength = data.songIds.Length;
                int songsLength = m_songs.songs.Count;
                for (int i = dataLength; i < songsLength; i++)
                {
                    m_scores.Add(i, 0);
                }
                SaveData();
            }
        }

        public void SaveData()
        {
            SaveSystem.Save();
        }

        public SongNoteConfig GetSong()
        {
            foreach (SongNoteConfig song in m_songs.songs)
            {
                if (song.id == m_selectingSongId)
                {
                    return song;
                }
            }
            return null;
        }

        public void SetScore(int score)
        {
            if (score > m_scores[m_selectingSongId])
            {
                m_scores[m_selectingSongId] = score;
            }
        }

        public Dictionary<int, int> Scores { get { return m_scores; } }

        public int SelectingSongId { get { return m_selectingSongId; } set { m_selectingSongId = value; } }

        public SongsConfig Songs { get { return m_songs; } }

        public static SharedData Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindFirstObjectByType<SharedData>();
                }
                return m_instance;
            }
        }
    }
}

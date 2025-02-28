using PianoTiles.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.Local
{
    [System.Serializable]
    public class SavedData
    {
        public int[] songIds;
        public int[] scores;

        public SavedData(bool isInitial = false)
        {
            if (isInitial)
            {
                List<SongNoteConfig> songs = SharedData.Instance.Songs.songs;
                songIds = new int[songs.Count];
                scores = new int[songs.Count];

                int idx = 0;
                foreach (SongNoteConfig song in songs)
                {
                    songIds[idx] = song.id;
                    scores[idx] = 0;
                    idx++;
                }
            }
            else
            {
                Dictionary<int, int> scoreList = SharedData.Instance.Scores;
                songIds = new int[scoreList.Count];
                scores = new int[scoreList.Count];

                int idx = 0;
                foreach (var score in SharedData.Instance.Scores)
                {
                    songIds[idx] = score.Key;
                    scores[idx] = score.Value;
                    idx++;
                }
            }
        }
    }
}

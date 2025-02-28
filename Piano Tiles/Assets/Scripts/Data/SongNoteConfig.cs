using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.Config
{
    [CreateAssetMenu(fileName = "New Song Config", menuName = "Create Config/Song Config")]
    public class SongNoteConfig : ScriptableObject
    {
        public int id;
        public string author;
        public AudioClip music;
        public List<int> velocityNotesIdx;
        public List<float> velocities;
        public List<NoteGroupConfig> groups;
    }
}

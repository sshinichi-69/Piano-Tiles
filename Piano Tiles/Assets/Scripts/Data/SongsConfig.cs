using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.Config
{
    [CreateAssetMenu(fileName = "New Songs Config", menuName = "Create Config/Songs Config")]
    public class SongsConfig : ScriptableObject
    {
        public List<SongNoteConfig> songs;
    }
}

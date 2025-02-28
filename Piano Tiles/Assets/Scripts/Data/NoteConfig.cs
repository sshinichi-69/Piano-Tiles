using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.Config
{
    [CreateAssetMenu(fileName = "New Song Config", menuName = "Create Config/Note Config")]
    public class NoteConfig : ScriptableObject
    {
        public NoteType type;
        public int length;
    }
}

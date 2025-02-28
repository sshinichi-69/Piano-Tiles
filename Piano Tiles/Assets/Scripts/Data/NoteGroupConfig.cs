using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.Config
{
    [CreateAssetMenu(fileName = "New Song Config", menuName = "Create Config/Note Group Config")]
    public class NoteGroupConfig : ScriptableObject
    {
        public List<NoteConfig> notes;
    }
}

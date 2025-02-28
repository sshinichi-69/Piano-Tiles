using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PianoTiles.Config;
using System.IO;
using System;
using UnityEditor;

namespace PianoTiles.Tool
{
    public class CreateSongConfig : MonoBehaviour
    {
        /*
        // Start is called before the first frame update
        void Start()
        {
            //CreateSong("Lost In The Memories", 0, "Pi");
            //CreateSong("Christmas Snow", 1, "Rainy");
            //CreateSong("Sitting Next To You", 2, "Handsome");
            //CreateSong("Days In A Green Hill", 3, "Fireflies");
        }
        
        private void CreateSong(string name, int id, string author)
        {
            string line;
            List<NoteGroupConfig> noteGroupConfigs = new List<NoteGroupConfig>();

            string address = "Assets/Config/Song/" + name + ".asset";
            if (File.Exists(address))
            {
                AssetDatabase.DeleteAsset(address);
            }
            SongNoteConfig asset = ScriptableObject.CreateInstance<SongNoteConfig>();
            asset.velocityNotesIdx = new List<int>();
            asset.velocities = new List<float>();

            try
            {
                string midiTxt = "./Assets/Tools/" + name + ".txt";
                StreamReader sr = new StreamReader(midiTxt);
                line = sr.ReadLine();
                int idx = 0;
                while (line != null)
                {
                    if (line.StartsWith("Tempo"))
                    {
                        asset.velocityNotesIdx.Add(idx);
                        int tempo = int.Parse(line.Split('=')[1]);
                        asset.velocities.Add(1 / (tempo / Mathf.Pow(10, 6)) * 4);
                    }
                    else
                    {
                        NoteConfig noteConfigAsset = GetNoteConfig(line);
                        NoteGroupConfig noteGroupConfig = GetNoteGroupConfig(line, noteConfigAsset);
                        noteGroupConfigs.Add(noteGroupConfig);
                        idx++;
                    }

                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Debug.LogError("Exception: " + e.Message);
            }

            asset.id = id;
            asset.author = author;
            asset.groups = new List<NoteGroupConfig>();
            foreach (NoteGroupConfig noteGroupConfig in noteGroupConfigs)
            {
                asset.groups.Add(noteGroupConfig);
            }

            string path = AssetDatabase.GenerateUniqueAssetPath(address);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private NoteConfig GetNoteConfig(string name)
        {
            if (!name.StartsWith("Empty Note"))
            {
                name = name.Substring(2);
            }
            string address = "Assets/Config/Note/" + name + ".asset";
            if (File.Exists(address))
            {
                return AssetDatabase.LoadAssetAtPath<NoteConfig>(address);
            }
            NoteConfig asset = ScriptableObject.CreateInstance<NoteConfig>();
            string[] words = name.Split(' ');

            string type = words[0];
            switch (type)
            {
                case "Empty":
                    asset.type = NoteType.EMPTY;
                    asset.length = int.Parse(words[words.Length - 1]);
                    break;
                case "Normal":
                    asset.type = NoteType.NORMAL;
                    break;
                case "Long":
                    asset.type = NoteType.LONG;
                    asset.length = int.Parse(words[words.Length - 1]);
                    break;
            }

            string path = AssetDatabase.GenerateUniqueAssetPath(address);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

        private NoteGroupConfig GetNoteGroupConfig(string name, NoteConfig noteConfigAsset)
        {
            string address = "Assets/Config/Note Group/" + name + ".asset";
            if (File.Exists(address))
            {
                return AssetDatabase.LoadAssetAtPath<NoteGroupConfig>(address);
            }
            NoteGroupConfig asset = ScriptableObject.CreateInstance<NoteGroupConfig>();
            int nNotes = 1;
            if (int.TryParse(name.Split(' ')[0], out int result))
            {
                nNotes = result;
            }

            asset.notes = new List<NoteConfig>();
            for (int i = 0; i < nNotes; i++)
            {
                asset.notes.Add(noteConfigAsset);
            }

            string path = AssetDatabase.GenerateUniqueAssetPath(address);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }
        // */
    }
}

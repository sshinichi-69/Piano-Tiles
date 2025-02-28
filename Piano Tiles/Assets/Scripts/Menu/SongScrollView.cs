using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PianoTiles.Menu
{
    public class SongScrollView : MonoBehaviour
    {
        [SerializeField] private GameObject m_content;
        [SerializeField] private Song m_songUiPrefab;

        // Start is called before the first frame update
        void Start()
        {
            float yPos = 125f;
            foreach (var song in SharedData.Instance.Scores)
            {
                GameObject songUi = Instantiate(m_songUiPrefab.gameObject);
                songUi.GetComponent<RectTransform>().anchoredPosition += Vector2.down * yPos;
                songUi.transform.SetParent(m_content.transform, false);
                songUi.GetComponent<Song>().Id = song.Key;
                songUi.GetComponent<Song>().SetScore(song.Value);
                songUi.GetComponent<Song>().SetName(SharedData.Instance.Songs.songs[song.Key].name);
                songUi.GetComponent<Song>().SetAuthor(SharedData.Instance.Songs.songs[song.Key].author);
                yPos += 225f;
            }
        }
    }
}

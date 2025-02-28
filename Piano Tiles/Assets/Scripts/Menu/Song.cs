using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PianoTiles.Menu
{
    public class Song : MonoBehaviour
    {
        private int m_id = 0;

        [SerializeField] private Text m_nameText;
        [SerializeField] private Text m_authorText;
        [SerializeField] private Text m_scoreText;

        public void OnPlayButtonClicked()
        {
            MenuManager.Instance.Play(m_id);
        }

        public void SetScore(int score)
        {
            if (score > int.Parse(m_scoreText.text))
            {
                m_scoreText.text = score.ToString();
            }
        }

        public void SetName(string name)
        {
            m_nameText.text = name;
        }

        public void SetAuthor(string author)
        {
            m_authorText.text = author;
        }

        public int Id { get { return m_id; } set { m_id = value; } }
    }
}

using PianoTiles.InGame.Note;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PianoTiles.InGame
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager m_instance;

        // Primitive attributes
        private GameStateType m_gameState;
        private int m_score;

        // Unity attributes
        private AudioSource m_audioSource;
        [SerializeField] private Text m_scoreText;
        [SerializeField] private GameObject m_endGamePanel;

        private void Awake()
        {
            Camera.main.orthographicSize = Screen.height / (Screen.width / 2f);

            m_gameState = GameStateType.STARTING;
        }

        // Start is called before the first frame update
        void Start()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_audioSource.clip = NoteManager.Instance.Song.music;
            m_audioSource.clip.LoadAudioData();

            m_instance = this;
        }

        public void PlayGame()
        {
            m_gameState = GameStateType.PLAYING;
            m_audioSource.Play();
        }

        public void EndGame()
        {
            m_gameState = GameStateType.ENDING;
            m_audioSource.Stop();
            NoteManager.Instance.EndGame();
            StartCoroutine(ShowEndGamePanel());
        }

        public void AddScore()
        {
            m_score++;
            m_scoreText.text = m_score.ToString();
        }

        public void Restart()
        {
            SceneManager.LoadScene("Scenes/InGame");
        }

        public void Exit()
        {
            SceneManager.LoadScene("Scenes/Menu");
        }

        private IEnumerator ShowEndGamePanel()
        {
            yield return new WaitForSeconds(3);
            m_endGamePanel.SetActive(true);
            SharedData.Instance.SetScore(m_score);
        }

        // Get, set
        public GameStateType GameState { get { return m_gameState; } }

        public static GameManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<GameManager>();
                }
                return m_instance;
            }
        }
    }
}

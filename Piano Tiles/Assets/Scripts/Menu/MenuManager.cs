using UnityEngine;
using UnityEngine.SceneManagement;

namespace PianoTiles.Menu
{
    public class MenuManager : MonoBehaviour
    {
        private static MenuManager m_instance;

        private void Awake()
        {
            Camera.main.orthographicSize = Screen.height / (Screen.width / 2);

            m_instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SaveData();
                if (Application.platform == RuntimePlatform.Android)
                {
                    AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                    activity.Call<bool>("moveTaskToBack", true);
                }
                else
                {
                    Application.Quit();
                }
            }
        }

        public void Play(int id)
        {
            SharedData.Instance.SelectingSongId = id;
            SceneManager.LoadScene("Scenes/InGame");
        }

        public void SaveData()
        {
            SharedData.Instance.SaveData();
            Debug.Log("Save data successfully!");
        }

        public static MenuManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<MenuManager>();
                }
                return m_instance;
            }
        }
    }
}

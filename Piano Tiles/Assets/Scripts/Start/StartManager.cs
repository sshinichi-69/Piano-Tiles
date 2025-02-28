using PianoTiles.Local;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PianoTiles.Start
{
    public class StartManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SaveSystem.Save();
            SceneManager.LoadScene("Scenes/Menu");
        }
    }
}

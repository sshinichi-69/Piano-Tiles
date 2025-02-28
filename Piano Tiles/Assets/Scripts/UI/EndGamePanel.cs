using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PianoTiles.InGame.Ui
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField] private Text m_gameScore;
        [SerializeField] private Text m_score;

        private void OnEnable()
        {
            m_score.text = m_gameScore.text;
        }
    }
}

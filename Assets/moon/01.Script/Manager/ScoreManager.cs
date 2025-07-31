using System;
using UnityEngine;

namespace moon._01.Script.Manager
{
    public class ScoreManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private int _score;
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Start()
        {
            _gameManager.SpawnManager.OnScoreChangeEvent += PlusScore;
        }

        private void PlusScore(int score)
            => _score += score;
        
        public int GetScore() => _score;

        public void ResetScoreManager()
        {
            _score = 0;
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

public class InGameState : MonoBehaviour
{
    [SerializeField] private PlayerEntity _playerEntity;
    [SerializeField] private InvaderSwarm _invaderSwarm;
    [SerializeField] private Obstacle _obstacle;

    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            if (value <= 0)
                return;

            _score = value;
            _viewport.ChangeScore(_score);
        }
    }

    private static Viewport _viewport;
    [SerializeField] private int _score;
    [Header("Obstacles")]

    private Vector2 _playerDefaultPosition;
        
    private void Start()
    {
        _viewport = Viewport.Instance;

        _playerDefaultPosition = _playerEntity.transform.position;
        _playerEntity.Initialize(_playerDefaultPosition, new KeyboardController());
        _invaderSwarm.Initialize(_playerEntity.transform);
        _obstacle.Generate();

        _score = 0;
        _viewport.Initialize(this);

        StartCoroutine(OnLose(() => _playerEntity.IsAlive == false || _invaderSwarm.LoseState));
        StartCoroutine(OnWin(() => _invaderSwarm.WinState == true));
    }
    
    [Serializable]
    public struct Obstacle
    {
        [SerializeField] private Transform _center;
        [SerializeField] private int _totalCount;
        [SerializeField] private float _spacing;
        [SerializeField] private ObstacleEntity _obstaclePrefab;

        public void Generate()
        {
            if (_center == null || _obstaclePrefab == null)
                return;

            int halfCount = _totalCount / 2;
            float startShift = _totalCount % 2 == 0 ? halfCount - 0.5f : halfCount;

            Vector2 position = (Vector2)_center.position + _spacing * startShift * Vector2.left;

            for (int i = 0; i < _totalCount; i++)
            {
                ObstacleEntity obstacle = Instantiate(_obstaclePrefab, position, Quaternion.identity);
                obstacle.Initialize(position, null);
                position += _spacing * Vector2.right;
            }
        }
    }

    public static void UpdateLives(ref int current, int max)
    {
        current = Mathf.Clamp(current, 0, max);
        _viewport.ChangeHealth(current);
    }

    private IEnumerator OnLose(Func<bool> func)
    {
        yield return new WaitUntil(func);

        Debug.Log("Lose");
        _viewport.GameOverScreenEnabled();

        OnEnd();
    }

    private IEnumerator OnWin(Func<bool> func)
    {
        yield return new WaitUntil(func);

        Debug.Log("Win");
        _viewport.WinScreenEnabled(true);

        OnEnd();
    }

    private void OnEnd()
    {
        Time.timeScale = 0f;
        AudioController.Instance.StopPlaying();
    }

    public void Continue()
    {
        _invaderSwarm.Initialize(_playerEntity.transform);
        _viewport.ResetScreens();
    }
}

using UnityEngine;

public class InvaderSwarm : MonoBehaviour
{
    public bool WinState => Count <= 0;
    public bool LoseState => _currentY < _minY;
    public int Count
    {
        get
        {
            return _count;
        }
        private set
        {
            _count = value;
        }
    }
    private int _count;

    [SerializeField] private InvanderEntity _invanderPrefab;
    [SerializeField] private InvaderType[] _invaderTypes;
    [SerializeField] private int _columnCount = 11;
    [SerializeField] private Vector2 _spacing;
    [SerializeField] private SwarmShooting _swarmShootingPrefab;
    [SerializeField] private float speedFactor = 10f;
    [SerializeField] private InGameState _inGameState;

    private SwarmController _swarmController;
    private float minX;
    private InvanderEntity[,] _invaders;
    private int _rowCount;
    private bool _isMovingRight = true;
    private float _maxX;
    private float _currentX;
    private float _xIncrement;
    private int _tempKillCount;
    private float _minY;
    private float _currentY;
    private GameObject _swarm;

    internal void IncreaseDeathCount()
    {
        _count--;

        _tempKillCount++;
        if (_tempKillCount < _invaders.Length / AudioController.Instance.PitchChangeStep)
        {
            return;
        }

        AudioController.Instance.IncreasePitch();
        _tempKillCount = 0;
    }

    internal InvanderEntity GetInvader(int row, int column)
    {
        if (row < 0 || column < 0
            || row >= _invaders.GetLength(0) || column >= _invaders.GetLength(1))
        {
            return null;

        }

        return _invaders[row, column];
    }

    private void MoveInvaders(float x, float y)
    {
        _swarmController.SetDirection(new Vector2(x, y));
    }

    private void ChangeDirection()
    {
        _isMovingRight = !_isMovingRight;
        MoveInvaders(0, -_spacing.y);

        _currentY -= _spacing.y;
    }

    public void Initialize(Transform player)
    {
        _swarmController = new SwarmController();

        _currentY = transform.position.y;
        _minY = player.position.y;

        minX = transform.position.x;

        if (_swarm != null)
        {
            Destroy(_swarm);
        }

        _swarm = new GameObject { name = "Swarm" };
        Vector2 currentPos = transform.position;

        foreach (var invaderType in _invaderTypes)
        {
            _rowCount += invaderType.RowCount;
        }

        _maxX = minX + 2f * _spacing.x * _columnCount;
        _currentX = minX;
        _invaders = new InvanderEntity[_rowCount, _columnCount];

        int rowIndex = 0;
        foreach (InvaderType invaderType in _invaderTypes)
        {
            for (int i = 0, len = invaderType.RowCount; i < len; i++)
            {
                for (int j = 0; j < _columnCount; j++)
                {
                    InvanderEntity invader = Instantiate(_invanderPrefab, _swarm.transform);
                    invader.Initialize(currentPos, _swarmController);
                    invader.SetType(invaderType);
                    if (invader.OnDie == null)
                    {
                        invader.OnDie = delegate
                        {
                            _inGameState.Score += invaderType.Points;
                            IncreaseDeathCount();
                        };
                    }

                    _invaders[rowIndex, j] = invader;
                    Count++;

                    currentPos.x += _spacing.x;
                }

                currentPos.x = minX;
                currentPos.y -= _spacing.y;

                rowIndex++;
            }
        }

        for (int i = 0; i < _columnCount; i++)
        {
            SwarmShooting swarmShooting = Instantiate(_swarmShootingPrefab, _swarm.transform);
            swarmShooting._column = i;
            swarmShooting._currentRow = _rowCount - 1;
            swarmShooting.Setup(this);
        }
    }

    public void Update()
    {
        _xIncrement = speedFactor * Time.deltaTime;
        if (_isMovingRight)
        {
            _currentX += _xIncrement;
            if (_currentX < _maxX)
            {
                MoveInvaders(_xIncrement, 0);
            }
            else
            {
                ChangeDirection();
            }
        }
        else
        {
            _currentX -= _xIncrement;
            if (_currentX > minX)
            {
                MoveInvaders(-_xIncrement, 0);
            }
            else
            {
                ChangeDirection();
            }
        }
    }
}

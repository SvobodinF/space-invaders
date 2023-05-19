using UnityEngine;

public class SwarmShooting : Shooting
{
    private InvaderSwarm _invaderSwarm;
    internal int _currentRow;
    internal int _column;

    [SerializeField] private float _minTime;
    [SerializeField] private float _maxTime;

    private float _timer;
    private float _currentTime;
    private InvanderEntity _followedTarget;

    internal void Setup(InvaderSwarm invaderSwarm)
    {
        _invaderSwarm = invaderSwarm;
        _currentTime = Random.Range(_minTime, _maxTime);
        _followedTarget = _invaderSwarm.GetInvader(_currentRow, _column);
    }

    private void Update()
    {
        if (_followedTarget == null)
        {
            Setup(_invaderSwarm);
            return;
        }
        transform.position = _followedTarget.transform.position;

        _timer += Time.deltaTime;
        if (_timer < _currentTime)
        {
            return;
        }

        OnShoot(_followedTarget.GetBullet());
        _timer = 0f;
        _currentTime = Random.Range(_minTime, _maxTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<Bullet>() == false)
            return;

        _currentRow = _currentRow - 1;

        if (_currentRow < 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Setup(_invaderSwarm);
        }
    }
}
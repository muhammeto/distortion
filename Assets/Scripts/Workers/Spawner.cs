﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject trianglePrefab = null;
    [SerializeField] private float startSpawnTime = 1;
    [SerializeField] private float spawnTime = 3;
    [SerializeField] private bool isRandomSpawner= false;
    [SerializeField] private List<Vector2> spawnPoses;

    private bool _isStarted;
    private float _startTimeCounter,_spawnTimeCounter;
    private GameState _state;

    private void Start()
    {
        _state = GameState.Forward;
        _isStarted = false;
        _startTimeCounter = startSpawnTime;
        _spawnTimeCounter = 0;
    }
    private void Update()
    {
        if (_state == GameState.Pause) return;
        if (!_isStarted)
        {
            _startTimeCounter -= Time.deltaTime;
            if (_startTimeCounter <= 0f)
            {
                _isStarted = true;
            }
        }
        if (_isStarted)
        {
            _spawnTimeCounter -= Time.deltaTime;
            if (_spawnTimeCounter <= 0f)
            {
                Spawn();
            }
        }
    }
    private void Spawn()
    {
        _spawnTimeCounter = spawnTime; 
        if (isRandomSpawner)
        {
            Instantiate(trianglePrefab, new Vector2(Random.Range(-20f,-15f),Random.Range(-6f,6f)), trianglePrefab.transform.rotation);
            return;
        }
        foreach(Vector2 pos in spawnPoses)
        {
            Instantiate(trianglePrefab, pos, trianglePrefab.transform.rotation);
        }

    }
    public void ChangeState(GameState state)
    {
        _state = state;
    }
    public void Add(Vector2 pos)
    {
        spawnPoses.Add(pos);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (Vector2 pos in spawnPoses)
        {
            Gizmos.DrawSphere(pos, 0.25f);
        }
    }
}
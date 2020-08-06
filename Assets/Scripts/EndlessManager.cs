using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EndlessManager : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.ChromaticAberration chromatic;
    private UnityEngine.Rendering.Universal.LensDistortion distortion;
    [SerializeField] private VolumeProfile postProcess = null;
    [SerializeField] private GameObject rewindText = null;
    [SerializeField] private GameObject losePanel = null;
    [SerializeField] private GameObject circlePrefab = null;
    [SerializeField] private float spawnRate;
    [SerializeField] private Vector2 spawnPoint;
    private int currentCircles = 0;
    private AudioSource rewindSource;
    private bool isForward = true;
    private List<CircleMove> circles;
    

    void Start()
    {
        circles = new List<CircleMove>();
        rewindSource = GetComponent<AudioSource>();
        postProcess.TryGet(out chromatic);
        postProcess.TryGet(out distortion);
        chromatic.intensity.value = 0;
        distortion.intensity.value = 0;
        InvokeRepeating("Spawn", 0, spawnRate);
    }

    private void Spawn()
    {
        CircleMove current = Instantiate(circlePrefab, spawnPoint, Quaternion.identity).GetComponent<CircleMove>();
        if (!isForward)
        {
            current.ChangeState(-1);
        }
        circles.Add(current);
    }

    public void IncreaseCount()
    {
        currentCircles++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            for (int i = 0; i < circles.Count; i++)
            {
                circles[i].ChangeState(isForward?-1:1);
            }
            if (isForward)
            {
                rewindText.SetActive(true);
                isForward = false;
                rewindSource.Play();
                StartCoroutine(ChromaticIncrease(1f));
                StartCoroutine(DistortionDecrease(-0.6f));
            }
            else
            {
                rewindText.SetActive(false);
                isForward = true;
                rewindSource.Stop();
                StartCoroutine(ChromaticDecrease(0));
                StartCoroutine(DistortionIncrease(0));
            }
        }
    }
    IEnumerator ChromaticIncrease(float value)
    {
        while (value - chromatic.intensity.value > 0.01)
        {
            chromatic.intensity.value += 0.005f;
            yield return null;
        }
    }
    IEnumerator ChromaticDecrease(float value)
    {
        while (chromatic.intensity.value - value > 0.01)
        {
            chromatic.intensity.value -= 0.005f;
            yield return null;
        }
    }
    IEnumerator DistortionIncrease(float value)
    {
        while (value - distortion.intensity.value > 0.01)
        {
            distortion.intensity.value += 0.005f;
            yield return null;
        }
    }
    IEnumerator DistortionDecrease(float value)
    {
        while (distortion.intensity.value - value > 0.01)
        {
            distortion.intensity.value -= 0.005f;
            yield return null;
        }
    }
}

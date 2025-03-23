using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pauser : MonoBehaviour
{
    [SerializeField] bool PauseWhenActive;

    [SerializeField] UnityEvent OnPause;
    [SerializeField] UnityEvent OnUnPause;

    public bool IsPaused { get; private set; }

    private void OnEnable()
    {
        if (PauseWhenActive) SetPaused(true);   
    }

    private void OnDisable()
    {
        if (PauseWhenActive) SetPaused(false);
    }

    public void SetPaused(bool paused)
    {
        if (IsPaused == paused) return;

        if (IsPaused = paused)
        {
            Time.timeScale = 0f;
            OnPause?.Invoke();
        }
        else
        {
            Time.timeScale = 1f;
            OnUnPause?.Invoke();
        }
    }
}

using DG.Tweening;
using MarkusSecundus.Utils.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem ExplosionParticles;
    [SerializeField] UnityEvent OnExplosionStart;
    [SerializeField] UnityEvent OnExplosion;
    [SerializeField] float UntilExplosionCallback_seconds = 0.5f;
    [SerializeField] Renderer ExplosionMesh;
    [SerializeField] float ExplosionBuildup_seconds = 0.4f;
    [SerializeField] float ExplosionStay_seconds = 0.6f;
    [SerializeField] Renderer ToTint;
    [SerializeField] Color ExplosionColorTint = Color.black;
    [SerializeField] float TintDuration_seconds = 0.6f;
    [SerializeField] UnityEvent OnExplosionFinished;
    public void RunExplosionEffect()
    {
        ExplosionParticles?.Play();

        if(ToTint) ToTint.material.DOColor(ExplosionColorTint, TintDuration_seconds);

        OnExplosionStart?.Invoke();
        this.InvokeWithDelay(() => OnExplosion?.Invoke(), UntilExplosionCallback_seconds);
        if (ExplosionMesh)
        {
            var originalScale = ExplosionMesh.transform.localScale;
            ExplosionMesh.transform.localScale = Vector3.zero;
            ExplosionMesh.enabled = true;

            ExplosionMesh.transform.DOScale(originalScale, ExplosionBuildup_seconds).OnComplete(() => this.InvokeWithDelay(() =>
            {
                ExplosionMesh.enabled = false;
                OnExplosionFinished?.Invoke();
            }, ExplosionStay_seconds));
        }
    }
}

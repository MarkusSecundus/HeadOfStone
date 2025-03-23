using MarkusSecundus.Utils.Behaviors.Automatization;
using MarkusSecundus.Utils.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    const int StartWave = 0;

    public int CurrentWave { get; private set; } = StartWave;

    [SerializeField] SpawnEntry[] Spawns;

    [SerializeField] UnityEvent<int> OnWaveStart;

    [SerializeField] SerializableDictionary<int, UnityEvent<int>> OnSpecificWaveStart;

    [System.Serializable]
    public struct SpawnEntry
    {
        public PlaceObjectsOnPoints Spawner;
        public int StartingWave;
        public float StartingAmount;
        public float MultiplicativeModifier;
        public float AdditiveModifier;

        public float CurrentAmount { get; set; }
    }

    public void StartNextWave()
    {
        ++CurrentWave;
        for(int t = 0; t < Spawns.Length; ++t)
        {
            var spawn = Spawns[t];
            {
                if (spawn.StartingWave > CurrentWave) continue;
                if(spawn.StartingWave == CurrentWave)
                    spawn.CurrentAmount = spawn.StartingAmount;
                else
                    spawn.CurrentAmount = spawn.CurrentAmount * spawn.MultiplicativeModifier + spawn.AdditiveModifier;
                
                spawn.Spawner.PlaceObjectsRandomVolume((int)spawn.CurrentAmount);
            }
            Spawns[t] = spawn;
        }
        OnWaveStart?.Invoke(CurrentWave);
        if (OnSpecificWaveStart.TryGetValue(CurrentWave, out var onWaveCallback)) onWaveCallback?.Invoke(CurrentWave);
    }

}

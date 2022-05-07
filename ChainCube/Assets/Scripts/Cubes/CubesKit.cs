using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New presets", menuName = "2048/Presets")]
public class CubesKit : ScriptableObject
{
    [SerializeField] private CubePreset[] presets;

    public CubePreset this[int i] 
    { 
        get 
        {
            return presets[i]; 
        } 
    }
    public int Lenght => presets.Length;
}

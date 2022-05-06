using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New cube", menuName = "2048/Cube")]
public class CubePreset : ScriptableObject
{
    [SerializeField] private int number;
    [SerializeField] private Color color;

    public int Number => number;
    public Color Color => color;
}
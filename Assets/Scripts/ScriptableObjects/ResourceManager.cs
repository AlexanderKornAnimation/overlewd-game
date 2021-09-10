using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ResourceManager", menuName = "Resource Manager", order = 51)]
public class ResourceManager : ScriptableObject
{
    [SerializeField]
    private ResourceLoader resourceLoader;
}

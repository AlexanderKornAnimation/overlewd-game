using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private ResourceLoader resourceLoader;

    private void Awake()
    {
        bool has = resourceLoader.HasNetworkConection();
        bool wifi = resourceLoader.NetworkTypeWIFI();
        bool mobile = resourceLoader.NetworkTypeMobile();
        int q = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

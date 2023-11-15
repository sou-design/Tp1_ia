
// Start is called before the first frame update
using UnityEngine;
using Cinemachine;

public class Zoom : MonoBehaviour
{

    void Update()
    {

        
            //GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = ++;
            GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 5;
        
    }
}


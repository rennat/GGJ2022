using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestScript : MonoBehaviour
{
    public Animation chopAnim;
    public float harvestTime = 3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (harvestTime <= 0)
        {
            PlayerInfo.instance.AddWood(50);
            Destroy(gameObject);
        }
    }

    public void Chopped()
    {
        chopAnim.Play();
    }

    public void Harvesting()
    {
        harvestTime -= Time.deltaTime * PlayerInfo.instance.playerMineSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestScript : MonoBehaviour
{
    public Animation chopAnim;
    public float harvestTime = 3f;
    private bool soundPlaying;

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
        if (!soundPlaying)
        {
            StartCoroutine(ChopSFX());
        }
        harvestTime -= Time.deltaTime * PlayerInfo.instance.playerMineSpeed;
    }

    IEnumerator ChopSFX()
    {
        soundPlaying = true;
        while (harvestTime > 0)
        {
            AudioManager.Chop();
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }

    public void StopSound()
    {
        StopAllCoroutines();
        soundPlaying = false;
    }
}

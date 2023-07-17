using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        //UnHilight();
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.2f);
        UnHilight();
    }
    public ItemSO item;
    public void Hilight()
    {
        outline.enabled = true;
    }
    public void UnHilight()
    {
        outline.enabled = false;
    }
    private void DestroyItem()
    {
        Debug.Log("Destroy item");
        Destroy(this.gameObject);
    }
}

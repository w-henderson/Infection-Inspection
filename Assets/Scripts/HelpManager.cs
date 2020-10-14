using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    public List<RectTransform> pages;
    public int currentPage = 0;

    public void NextPage()
    {
        pages[currentPage].localScale = Vector3.zero;
        if (currentPage + 1 == pages.Count)
        {
            currentPage = 0;
        }
        else
        {
            currentPage++;
        }
        pages[currentPage].localScale = Vector3.one;
    }

    public void PreviousPage()
    {
        pages[currentPage].localScale = Vector3.zero;
        if (currentPage == 0)
        {
            currentPage = pages.Count - 1;
        }
        else
        {
            currentPage--;
        }
        pages[currentPage].localScale = Vector3.one;
    }
}

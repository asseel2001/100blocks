using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BonusManager : MonoBehaviour
{
    public List<GameObject> bonusList;


    void Start()
    {
        GameEvents.ShowBonusScreen += showBonusScreen;
    }

    private void OnDisable()
    {
        GameEvents.ShowBonusScreen -= showBonusScreen;
    }



    private void showBonusScreen(Config.SquareColor color)
    {
        GameObject obj = null;

        foreach(var Bonus in bonusList)
        {
            var bonusComp=Bonus.GetComponent<Bonus>();
            if(bonusComp.color == color)
            {
                obj = Bonus;
                Bonus.SetActive(true);

            }
        }
        StartCoroutine(DeActivateBonus(obj));
    }

    private IEnumerator DeActivateBonus(GameObject obj)
    {
        yield return new WaitForSeconds(2);
        obj.SetActive(false);
    }
}

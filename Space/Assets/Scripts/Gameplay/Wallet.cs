using UnityEngine;
using System.Collections;

public class Wallet : MonoBehaviour {
    public bool player;
    private int starBucks;

	// Use this for initialization
	void Start () {
        //players start from the bottom and got to work to earn money for weapons better than the ones we start them with. 
        if (player)
        {
            starBucks = 0;
        }
        //give vendors some cash to purchase weapons that the player wants to sell
        else
        {
            starBucks = 400;
        }
	}
	
    public void MakeItRain(int sB, Wallet otherPerson)
    {
        otherPerson.GetPaid(sB);
        starBucks -= sB;
    }
    public void GetPaid(int sB)
    {
        starBucks += sB;
    }
    public void DepreciateForBuyBack(int value)
    {
        value /= 4;
    }
    public string ToString()
    {
        string thisWallet = "";
        if (player)
        {
            thisWallet += "true,";
        }
        thisWallet += starBucks;

        return thisWallet;
    }
}

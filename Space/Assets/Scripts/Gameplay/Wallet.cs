using UnityEngine;
using System.Collections;

public class Wallet : MonoBehaviour {
    public bool player;
    private int starBucks;

	// Use this for initialization
	void Start () {
        //give the player some funds to start and buy a couple weapons 
        if (player)
        {
            starBucks = 1000;
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

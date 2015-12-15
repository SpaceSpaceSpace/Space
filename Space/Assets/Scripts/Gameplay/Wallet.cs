using UnityEngine;
using System.Collections;

public class Wallet : MonoBehaviour {
    public bool player;
    private int spaceBux;

	// Use this for initialization
	void Start () {
        //players start from the bottom and got to work to earn money for weapons better than the ones we start them with. 
        if (player)
        {
            spaceBux = 0;
        }
        //give vendors some cash to purchase weapons that the player wants to sell
        else
        {
            spaceBux = 400;
        }
	}
	
    public bool MakeItRain(int sB, Wallet otherPerson)
    {
        if (sB > spaceBux && player)
			return false;
		else
		{
        otherPerson.Reward(sB);
        spaceBux -= sB;
			return true;
		}

    }
    public void Reward(int sB)
    {
        spaceBux += sB;
        
    }
    public void GetPaid(int sB)
    {
        spaceBux += sB;
    }
    public void DepreciateForBuyBack(int value)
    {
        value /= 4;
    }
    public float GetSpaceBux()
    {
        return spaceBux;
    }
    public string ToWords()
    {
        string thisWallet = "";
        if (player)
        {
            thisWallet += "true,";
        }
        thisWallet += spaceBux;

        return thisWallet;
    }
}

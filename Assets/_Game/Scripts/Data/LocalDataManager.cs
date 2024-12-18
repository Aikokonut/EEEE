using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalDataManager : DataManager
{
    protected override void Awake(){
        base.Awake();
        var init = PlayerPrefs.GetInt("INIT", 0);
        if(init == 0){
            PlayerPrefs.SetInt("INIT", 0);
            this.SetDiscData(new List<int>{0, 1});
        }
    }
    public override List<int> GetDiscData()
    {
        var rawData = PlayerPrefs.GetString("DISC");
        return rawData.Split(' ').Select(x => int.Parse(x)).ToList();
        
    }

    public override void SetDiscData(List<int> discData)
    {
        var rawData = String.Join(' ', discData);
        PlayerPrefs.SetString("DISC", rawData);
    }
}

using UnityEngine;
using System.Collections.Generic;
[ExecuteInEditMode]

public class NamingPattern : MonoBehaviour
{

    [Header("disable this component when done")]
    [Header("duplicating")]
    [Header("Important:")]
    public string namingPattern;
    public int nameEnding;
    public bool TriggerNameChange = true;

    [Tooltip("avoid touching")]
    public int FirstEndInTens=10;
    [Tooltip("avoid touching")]
    public int FirstEndInHundreds = 100;
    int numberIncrement;

    void Awake()
    {
        TriggerNameChange = true;
    }
    void Update()
    {
        if (TriggerNameChange)
        {
            ChangeName();
        }
    }

    public void ChangeName()
    {
        numberIncrement = nameEnding;
        if (namingPattern != "")
        {
            gameObject.name = namingPattern + nameEnding;
            numberIncrement += 1;
            nameEnding = numberIncrement;
        }
        else
        {
            gameObject.name = gameObject.name + nameEnding;
        }
        if (numberIncrement > 10)
        {
            List<char> SplittedName = new List<char>();
            int finalIndex = -1;
            foreach (char item in gameObject.name)
            {
                SplittedName.Add(item);
                finalIndex += 1;
            }
            gameObject.name = "";
            for (int i = 0; i < finalIndex - 2; i++)
            {
                gameObject.name += SplittedName[i];
            }
            gameObject.name += FirstEndInTens;
            FirstEndInTens = FirstEndInTens + 1;
            nameEnding = FirstEndInTens;
        }
        if (numberIncrement>100)
        {
            List<char> SplittedName = new List<char>();
            int finalIndex = -1;
            foreach (char item in gameObject.name)
            {
                SplittedName.Add(item);
                finalIndex = finalIndex + 1;
            }
            gameObject.name = "";
            for (int i = 0; i < finalIndex-3; i++)
            {
                gameObject.name = gameObject.name + SplittedName[i];
            }
            gameObject.name = gameObject.name + FirstEndInHundreds;
            FirstEndInHundreds = FirstEndInHundreds + 1;
            nameEnding = FirstEndInHundreds;
        }
        TriggerNameChange = false;
    } 
}
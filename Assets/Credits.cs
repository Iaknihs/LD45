using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ChangeText1", 7.0f);
    }

    void ChangeText1()
    {
        GetComponent<UnityEngine.UI.Text>().text = "A Ludum Dare 45 Game.";
        Invoke("ChangeText2", 5.0f);
        GetComponent<AudioSource>().Play();
    }

    void ChangeText2()
    {
        GetComponent<UnityEngine.UI.Text>().text = "Made in 72 hours.";
        Invoke("ChangeText3", 5.0f);
        GetComponent<AudioSource>().Play();
    }

    void ChangeText3()
    {
        GetComponent<UnityEngine.UI.Text>().text = "By 1 person.";
        Invoke("ChangeText4", 4.0f);
        GetComponent<AudioSource>().Play();
    }

    void ChangeText4()
    {
        GetComponent<UnityEngine.UI.Text>().text = "Made By:\nDaniel Schruff\nldjam.com/users/iak\ndanielschruff.com";
        GetComponent<AudioSource>().Play();
    }

}

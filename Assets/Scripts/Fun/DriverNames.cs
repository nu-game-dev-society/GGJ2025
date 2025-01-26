using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverNames
{
    const string PlayerName = "PlayerName";
    public static string[] ExampleDriverNames = new string[] {
        "Seabass-tian Vettel",
        "Lewis Ham-whale-ton",
        "Max Ver-splash-en",
        "Charles LeClam",
        "Yuki Tunada",
        "Alain Prawn-st",
        "Nigel ManSailfish",
        "Michael Shoal-macher",
        "Alex AlBarracuda",
        "Robert Krill-bica",
        "George Mussel",
        "Kimi Reef-konen",
        "Eddie Eel-vine",
        "Ayrton Sea-na",
        "Lando Norwhal",
        "Kimi AnTern-elli",
        "Fernando A-lobster",
        "Pierre Bass-ly",
        "Oscar Pias-squid",
        "Kevin Mag-kraken",
        "Nicholas La-reef-i",
        "Pastor Mal-do-nautical",
        "Timo Dock",
        "Esteban Ocean",
        "Liam Lobster",
        "Isaac Haddock-jar",
        "Nico Hull-kenberg",
        "Zhou Guan Yacht",
        "Flipper Massa",
    };
    public static string GetRandom()
    {
        return ExampleDriverNames[
            Random.Range(0, ExampleDriverNames.Length)
            ];
    }
    public static string GetPlayerDriverName()
    {
        string name = string.Empty;
        if (PlayerPrefs.HasKey(PlayerName))
        {
            name = PlayerPrefs.GetString(PlayerName);
        }
        return name?.Length > 0 ? name : GetRandom();
    }
    public static void SetPlayerDriverName(string name)
    {
        if (PlayerPrefs.HasKey(PlayerName))
            PlayerPrefs.DeleteKey(PlayerName);
        PlayerPrefs.SetString(PlayerName, name?.Length > 0 ? name : GetRandom());
    }
}

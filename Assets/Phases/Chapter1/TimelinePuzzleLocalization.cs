using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Localization data for the Timeline Puzzle
/// Contains all text strings and keys for multilingual support
/// </summary>
public class TimelinePuzzleLocalization : MonoBehaviour
{
    private Dictionary<string, string> portugueseStrings = new Dictionary<string, string>();
    private Dictionary<string, string> englishStrings = new Dictionary<string, string>();
    private Dictionary<string, string> spanishStrings = new Dictionary<string, string>();

    private void Awake()
    {
        InitializeLocalization();
    }

    private void InitializeLocalization()
    {
        // Portuguese strings
        portugueseStrings = new Dictionary<string, string>
        {
            // Header
            { "Puzzle_Title", "Linha do Tempo do Nascimento" },
            { "Puzzle_Complete", "Parabéns! Você completou o puzzle!" },

            // Event Cards
            { "Timeline.Event1", "César Augusto decreta recenseamento" },
            { "Timeline.Event2", "José e Maria viajam para Belém" },
            { "Timeline.Event3", "Não há lugar na hospedaria" },
            { "Timeline.Event4", "Jesus nasce e é colocado na manjedoura" },
            { "Timeline.Event5", "Anjos aparecem aos pastores" },
            { "Timeline.Event6", "Os pastores vão até Belém" },
            { "Timeline.Event7", "Encontram Maria, José e o bebê" },
            { "Timeline.Event8", "Pastores glorificam a Deus" },

            // Feedback Messages
            { "Timeline.Correct", "Correto!" },
            { "Timeline.Incorrect", "Incorreto. Tente novamente!" },
            { "Timeline.Hint1", "Dica: O recenseamento veio primeiro, causando a viagem" },
            { "Timeline.Hint2", "Dica: Os anjos anunciaram aos pastores após o nascimento" },
            { "Timeline.TryAgain", "Ordem incorreta. Tente novamente!" },

            // Button Texts
            { "Timeline.Check", "Verificar" },
            { "Timeline.Close", "Fechar" },
            { "Timeline.Rewards", "Recompensas: 50 moedas + Selo de Belém" }
        };

        // English strings
        englishStrings = new Dictionary<string, string>
        {
            // Header
            { "Puzzle_Title", "Nativity Timeline" },
            { "Puzzle_Complete", "Congratulations! You completed the puzzle!" },

            // Event Cards
            { "Timeline.Event1", "Caesar Augustus decrees census" },
            { "Timeline.Event2", "Joseph and Mary travel to Bethlehem" },
            { "Timeline.Event3", "No room in the inn" },
            { "Timeline.Event4", "Jesus is born and placed in the manger" },
            { "Timeline.Event5", "Angels appear to the shepherds" },
            { "Timeline.Event6", "Shepherds go to Bethlehem" },
            { "Timeline.Event7", "They find Mary, Joseph and the baby" },
            { "Timeline.Event8", "Shepherds glorify God" },

            // Feedback Messages
            { "Timeline.Correct", "Correct!" },
            { "Timeline.Incorrect", "Incorrect. Try again!" },
            { "Timeline.Hint1", "Hint: The census came first, causing the journey" },
            { "Timeline.Hint2", "Hint: The angels announced to the shepherds after the birth" },
            { "Timeline.TryAgain", "Incorrect order. Try again!" },

            // Button Texts
            { "Timeline.Check", "Check" },
            { "Timeline.Close", "Close" },
            { "Timeline.Rewards", "Rewards: 50 coins + Bethlehem Stamp" }
        };

        // Spanish strings
        spanishStrings = new Dictionary<string, string>
        {
            // Header
            { "Puzzle_Title", "Cronología del Nacimiento" },
            { "Puzzle_Complete", "¡Felicidades! Completaste el puzzle!" },

            // Event Cards
            { "Timeline.Event1", "César Augusto decrea el censo" },
            { "Timeline.Event2", "José y María viajan a Belén" },
            { "Timeline.Event3", "No hay lugar en la posada" },
            { "Timeline.Event4", "Jesús nace y es colocado en el pesebre" },
            { "Timeline.Event5", "Ángeles se aparecen a los pastores" },
            { "Timeline.Event6", "Los pastores van a Belén" },
            { "Timeline.Event7", "Encuentran a María, José y el bebé" },
            { "Timeline.Event8", "Los pastores glorifican a Dios" },

            // Feedback Messages
            { "Timeline.Correct", "¡Correcto!" },
            { "Timeline.Incorrect", "Incorrecto. ¡Inténtalo de nuevo!" },
            { "Timeline.Hint1", "Pista: El censo vino primero, causando el viaje" },
            { "Timeline.Hint2", "Pista: Los ángeles anunciaron a los pastores después del nacimiento" },
            { "Timeline.TryAgain", "Orden incorrecta. ¡Inténtalo de nuevo!" },

            // Button Texts
            { "Timeline.Check", "Verificar" },
            { "Timeline.Close", "Cerrar" },
            { "Timeline.Rewards": "Recompensas: 50 monedas + Sello de Belén" }
        };
    }

    public string GetString(string key, string language = "Portuguese")
    {
        switch (language.ToLower())
        {
            case "english":
                return englishStrings.TryGetValue(key, out string english) ? english : key;
            case "spanish":
                return spanishStrings.TryGetValue(key, out string spanish) ? spanish : key;
            default: // Portuguese
                return portugueseStrings.TryGetValue(key, out string portuguese) ? portuguese : key;
        }
    }

    public Dictionary<string, string> GetAllStrings(string language = "Portuguese")
    {
        switch (language.ToLower())
        {
            case "english":
                return englishStrings;
            case "spanish":
                return spanishStrings;
            default: // Portuguese
                return portugueseStrings;
        }
    }
}
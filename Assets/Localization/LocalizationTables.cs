using UnityEngine;
using System.Collections.Generic;

namespace CristoAdventure.Localization
{
    /// <summary>
    /// Complete localization tables for Cristo Adventure Phase 1.1: Bethlehem - Basilica of the Nativity
    /// Contains all localization strings in Portuguese, English, and Spanish
    /// Created by: Agent-12 (Narrative Designer)
    /// Date: 14/02/2026
    /// </summary>
    public static class LocalizationTables
    {
        // Supported languages
        public enum Language
        {
            Portuguese,
            English,
            Spanish
        }

        // Localization keys enum for type safety
        public enum Key
        {
            // Phase Info
            Phase_Title,

            // POI-001: History Plaque
            POI001_Title,
            POI001_Content_Line1,
            POI001_Content_Line2,
            POI001_Content_Line3,

            // POI-002: Silver Star
            POI002_Title,
            POI002_Content_Line1,
            POI002_Content_Line2,
            POI002_Content_Line3,

            // POI-003: Manger
            POI003_Title,
            POI003_Content_Line1,
            POI003_Content_Line2,
            POI003_Content_Line3,

            // POI-004: Father Elias (NPC)
            NPC001_Name,
            NPC001_Greeting,
            NPC001_Option1,
            NPC001_Option2,
            NPC001_Option3,
            NPC001_Response1,
            NPC001_Response2,
            NPC001_Response3,

            // POI-005: Photo Spot
            POI005_Prompt,

            // POI-006: Luke 2:7
            POI006_Reference,
            POI006_Verse,

            // Puzzle: Timeline
            Puzzle_Title,
            Puzzle_Instructions,
            Puzzle_Event1,
            Puzzle_Event2,
            Puzzle_Event3,
            Puzzle_Event4,
            Puzzle_Event5,
            Puzzle_Event6,
            Puzzle_Event7,
            Puzzle_Event8,
            Puzzle_Hint1,
            Puzzle_Hint2,
            Puzzle_Success,

            // UI Strings
            UI_Interact,
            UI_Backpack,
            UI_Pause,
            UI_Settings,
            UI_PhaseComplete,
            UI_StarsEarned
        }

        // Complete localization dictionary
        public static readonly Dictionary<Key, Dictionary<Language, string>> Data =
            new Dictionary<Key, Dictionary<Language, string>>
        {
            // ========== PHASE INFO ==========
            {
                Key.Phase_Title,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Fase 1.1: Belém - Basílica da Natividade" },
                    { Language.English, "Phase 1.1: Bethlehem - Basilica of the Nativity" },
                    { Language.Spanish, "Fase 1.1: Belén - Basílica de la Natividad" }
                }
            },

            // ========== POI-001: HISTORY PLAQUE ==========
            {
                Key.POI001_Title,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "A Basílica da Natividade" },
                    { Language.English, "The Basilica of the Nativity" },
                    { Language.Spanish, "La Basílica de la Natividad" }
                }
            },
            {
                Key.POI001_Content_Line1,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "A Basílica da Natividade foi construída em 327 d.C. por ordem de Santa Helena, mãe do Imperador Constantino. Ela viajou à Terra Santa para encontrar os locais sagrados relacionados à vida de Jesus." },
                    { Language.English, "The Basilica of the Nativity was built in 327 AD by order of Saint Helena, mother of Emperor Constantine. She traveled to the Holy Land to find sacred sites related to Jesus' life." },
                    { Language.Spanish, "La Basílica de la Natividad fue construida en 327 d.C. por orden de Santa Elena, madre del Emperador Constantino. Ella viajó a Tierra Santa para encontrar los lugares sagrados relacionados con la vida de Jesús." }
                }
            },
            {
                Key.POI001_Content_Line2,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "A basílica foi construída sobre a gruta onde Jesus nasceu. É uma das igrejas cristãs mais antigas do mundo em uso contínuo, com mais de 1.600 anos de história." },
                    { Language.English, "The basilica was built over the cave where Jesus was born. It is one of the oldest Christian churches in continuous use, with over 1,600 years of history." },
                    { Language.Spanish, "La basílica fue construida sobre la gruta donde Jesús nació. Es una de las iglesias cristianas más antiguas en uso continuo, con más de 1.600 años de historia." }
                }
            },
            {
                Key.POI001_Content_Line3,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Em 2012, foi declarada Patrimônio Mundial da UNESCO." },
                    { Language.English, "In 2012, it was declared a UNESCO World Heritage Site." },
                    { Language.Spanish, "En 2012, fue declarada Patrimonio de la Humanidad por la UNESCO." }
                }
            },

            // ========== POI-002: SILVER STAR ==========
            {
                Key.POI002_Title,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "A Estrela de Prata" },
                    { Language.English, "The Silver Star" },
                    { Language.Spanish, "La Estrella de Plata" }
                }
            },
            {
                Key.POI002_Content_Line1,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Esta Estrela de Prata de 14 pontas marca o local exato onde Jesus nasceu." },
                    { Language.English, "This 14-pointed Silver Star marks the exact spot where Jesus was born." },
                    { Language.Spanish, "Esta Estrella de Plata de 14 puntas marca el lugar exacto donde Jesús nació." }
                }
            },
            {
                Key.POI002_Content_Line2,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "A estrella está incrustada no chão da gruta da Natividade, cercada por lamparinas que queimam perpetuamente." },
                    { Language.English, "The star is set into the floor of the Nativity Cave, surrounded by perpetually burning lamps." },
                    { Language.Spanish, "La estrella está incrustada en el suelo de la gruta de la Natividad, rodeada de lámparas que arden perpetuamente." }
                }
            },
            {
                Key.POI002_Content_Line3,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Peregrinos de todo o mundo vêm aqui para orar e reverenciar este santo lugar." },
                    { Language.English, "Pilgrims from all over the world come here to pray and revere this holy place." },
                    { Language.Spanish, "Peregrinos de todo el mundo vienen aquí para orar y venerar este santo lugar." }
                }
            },

            // ========== POI-003: MANGER ==========
            {
                Key.POI003_Title,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "O Significado da Manjedoura" },
                    { Language.English, "The Meaning of the Manger" },
                    { Language.Spanish, "El Significado del Pesebre" }
                }
            },
            {
                Key.POI003_Content_Line1,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Jesus foi colocado em uma manjedoura - um cocho para animais - após seu nascimento. Este detalhe destaca a humildade do nascimento de Jesus." },
                    { Language.English, "Jesus was placed in a manger - a feeding trough for animals - after his birth. This detail highlights the humility of Jesus' birth." },
                    { Language.Spanish, "Jesús fue colocado en un pesebre - un comedero para animales - después de su nacimiento. Este detalle destaca la humildad del nacimiento de Jesús." }
                }
            },
            {
                Key.POI003_Content_Line2,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "O Rei dos reis nasceu não em um palácio, mas em um estábulo simples, envolto em panos e colocado onde os animais comiam." },
                    { Language.English, "The King of kings was born not in a palace, but in a simple stable, wrapped in cloths and placed where animals ate." },
                    { Language.Spanish, "El Rey de reyes no nació en un palacio, sino en un establo sencillo, envuelto en pañales y colocado donde comían los animales." }
                }
            },
            {
                Key.POI003_Content_Line3,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Este ato simboliza a proximidade de Deus com os humildes e simples, e como Jesus veio para todos, sem distinção." },
                    { Language.English, "This act symbolizes God's closeness to the humble and simple, and how Jesus came for everyone, without distinction." },
                    { Language.Spanish, "Este acto simboliza la cercanía de Dios con los humildes y sencillos, y cómo Jesús vino para todos, sin distinción." }
                }
            },

            // ========== POI-004: FATHER ELIAS (NPC) ==========
            {
                Key.NPC001_Name,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Padre Elias" },
                    { Language.English, "Father Elias" },
                    { Language.Spanish, "Padre Elias" }
                }
            },
            {
                Key.NPC001_Greeting,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Paz esteja com você, peregrino. Bem-vindo à Basílica da Natividade." },
                    { Language.English, "Peace be with you, pilgrim. Welcome to the Basilica of the Nativity." },
                    { Language.Spanish, "La paz esté contigo, peregrino. Bienvenido a la Basílica de la Natividad." }
                }
            },
            {
                Key.NPC001_Option1,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "O que há de especial neste lugar?" },
                    { Language.English, "What is special about this place?" },
                    { Language.Spanish, "¿Qué tiene de especial este lugar?" }
                }
            },
            {
                Key.NPC001_Option2,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Conte-me sobre a estrela." },
                    { Language.English, "Tell me about the star." },
                    { Language.Spanish, "Háblame de la estrella." }
                }
            },
            {
                Key.NPC001_Option3,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Por que Belém é importante?" },
                    { Language.English, "Why is Bethlehem important?" },
                    { Language.Spanish, "¿Por qué es importante Belén?" }
                }
            },
            {
                Key.NPC001_Response1,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Este é um dos lugares mais sagrados do cristianismo. Aqui, nesta gruta, Deus se fez homem e habitou entre nós. Sinta a presença sagrada deste local." },
                    { Language.English, "This is one of the most sacred places in Christianity. Here, in this cave, God became man and dwelt among us. Feel the sacred presence of this place." },
                    { Language.Spanish, "Este es uno de los lugares más sagrados del cristianismo. Aquí, en esta gruta, Dios se hizo hombre y habitó entre nosotros. Siente la presencia sagrada de este lugar." }
                }
            },
            {
                Key.NPC001_Response2,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "A Estrela de Prata tem 14 pontas e marca o local exato do nascimento. Peregrinos de todo o mundo vêm aqui para orar e reverenciar este santo lugar." },
                    { Language.English, "The Silver Star has 14 points and marks the exact spot of the birth. Pilgrims from all over the world come here to pray and revere this holy place." },
                    { Language.Spanish, "La Estrella de Plata tiene 14 puntas y marca el lugar exacto del nacimiento. Peregrinos de todo el mundo vienen aquí para orar y venerar este santo lugar." }
                }
            },
            {
                Key.NPC001_Response3,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Belém significa 'Casa de Pão' em hebraico. É aqui que nasceu Aquele que se chamaria o Pão da Vida. Profecias de séculos antes previram este local." },
                    { Language.English, "Bethlehem means 'House of Bread' in Hebrew. Here was born He who would call himself the Bread of Life. Centuries-old prophecies foretold this place." },
                    { Language.Spanish, "Belén significa 'Casa del Pan' en hebreo. Aquí nació Aquel que se llamaría a sí mismo el Pan de Vida. Profecías de siglos atrás predijeron este lugar." }
                }
            },

            // ========== POI-005: PHOTO SPOT ==========
            {
                Key.POI005_Prompt,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Vista panorâmica da Praça da Manjedoura. Pressione F para tirar uma foto." },
                    { Language.English, "Panoramic view of Manger Square. Press F to take a photo." },
                    { Language.Spanish, "Vista panorámica de la Plaza del Pesebre. Presiona F para tomar una foto." }
                }
            },

            // ========== POI-006: LUKE 2:7 ==========
            {
                Key.POI006_Reference,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Lucas 2:7" },
                    { Language.English, "Luke 2:7" },
                    { Language.Spanish, "Lucas 2:7" }
                }
            },
            {
                Key.POI006_Verse,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "E ela deu à luz seu filho primogênito, e o envolveu em panos, e o deitou numa manjedoura, porque não havia lugar para eles na hospedaria." },
                    { Language.English, "And she brought forth her firstborn son, and wrapped him in swaddling clothes, and laid him in a manger; because there was no room for them in the inn." },
                    { Language.Spanish, "Y dio a luz a su hijo primogénito, y lo envolvió en pañales, y lo acostó en un pesebre, porque no había lugar para ellos en el mesón." }
                }
            },

            // ========== PUZZLE: TIMELINE ==========
            {
                Key.Puzzle_Title,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Linha do Tempo do Nascimento" },
                    { Language.English, "Nativity Timeline" },
                    { Language.Spanish, "Cronología del Nacimiento" }
                }
            },
            {
                Key.Puzzle_Instructions,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Ordene os eventos do nascimento de Jesus na sequência correta segundo Lucas 2:1-20" },
                    { Language.English, "Order the events of Jesus' birth in the correct sequence according to Luke 2:1-20" },
                    { Language.Spanish, "Ordena los eventos del nacimiento de Jesús en la secuencia correcta según Lucas 2:1-20" }
                }
            },
            {
                Key.Puzzle_Event1,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "César Augusto decreta o recenseamento" },
                    { Language.English, "Caesar Augustus decrees the census" },
                    { Language.Spanish, "César Augusto decreta el censo" }
                }
            },
            {
                Key.Puzzle_Event2,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "José e Maria viajam para Belém" },
                    { Language.English, "Joseph and Mary travel to Bethlehem" },
                    { Language.Spanish, "José y María viajan a Belén" }
                }
            },
            {
                Key.Puzzle_Event3,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Não há lugar na hospedaria" },
                    { Language.English, "No room at the inn" },
                    { Language.Spanish, "No hay lugar en la posada" }
                }
            },
            {
                Key.Puzzle_Event4,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Jesus nasce e é colocado na manjedoura" },
                    { Language.English, "Jesus is born and placed in the manger" },
                    { Language.Spanish, "Jesús nace y es colocado en el pesebre" }
                }
            },
            {
                Key.Puzzle_Event5,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Anjos aparecem aos pastores" },
                    { Language.English, "Angels appear to the shepherds" },
                    { Language.Spanish, "Ángeles aparecen a los pastores" }
                }
            },
            {
                Key.Puzzle_Event6,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Os pastores vão até Belém" },
                    { Language.English, "The shepherds go to Bethlehem" },
                    { Language.Spanish, "Los pastores van a Belén" }
                }
            },
            {
                Key.Puzzle_Event7,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Encontram Maria, José e o bebê" },
                    { Language.English, "They find Mary, Joseph and the baby" },
                    { Language.Spanish, "Encuentran a María, José y al bebé" }
                }
            },
            {
                Key.Puzzle_Event8,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Pastores glorificam a Deus" },
                    { Language.English, "Shepherds glorify God" },
                    { Language.Spanish, "Pastores glorifican a Dios" }
                }
            },
            {
                Key.Puzzle_Hint1,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "O recenseamento veio primeiro, causando a viagem" },
                    { Language.English, "The census came first, causing the journey" },
                    { Language.Spanish, "El censo vino primero, causando el viaje" }
                }
            },
            {
                Key.Puzzle_Hint2,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Os anjos anunciaram aos pastores após o nascimento" },
                    { Language.English, "The angels announced to shepherds after the birth" },
                    { Language.Spanish, "Los ángeles anunciaron a los pastores después del nacimiento" }
                }
            },
            {
                Key.Puzzle_Success,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Parabéns! Você completou a linha do tempo do nascimento!" },
                    { Language.English, "Congratulations! You completed the Nativity Timeline!" },
                    { Language.Spanish, "¡Felicidades! Completaste la cronología del nacimiento!" }
                }
            },

            // ========== UI STRINGS ==========
            {
                Key.UI_Interact,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Pressione E para interagir" },
                    { Language.English, "Press E to interact" },
                    { Language.Spanish, "Presiona E para interactuar" }
                }
            },
            {
                Key.UI_Backpack,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Mochila" },
                    { Language.English, "Backpack" },
                    { Language.Spanish, "Mochila" }
                }
            },
            {
                Key.UI_Pause,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Pausar" },
                    { Language.English, "Pause" },
                    { Language.Spanish, "Pausar" }
                }
            },
            {
                Key.UI_Settings,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Configurações" },
                    { Language.English, "Settings" },
                    { Language.Spanish, "Configuración" }
                }
            },
            {
                Key.UI_PhaseComplete,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Fase Completa!" },
                    { Language.English, "Phase Complete!" },
                    { Language.Spanish, "¡Fase Completada!" }
                }
            },
            {
                Key.UI_StarsEarned,
                new Dictionary<Language, string>
                {
                    { Language.Portuguese, "Estrelas ganhas" },
                    { Language.English, "Stars earned" },
                    { Language.Spanish, "Estrellas ganadas" }
                }
            }
        };

        /// <summary>
        /// Get localized string by key and language
        /// </summary>
        public static string Get(Key key, Language language)
        {
            if (Data.ContainsKey(key) && Data[key].ContainsKey(language))
            {
                return Data[key][language];
            }

            Debug.LogWarning($"Localization not found for Key: {key}, Language: {language}");
            return string.Empty;
        }

        /// <summary>
        /// Get localized string by key using current system language
        /// </summary>
        public static string Get(Key key)
        {
            Language language = GetSystemLanguage();
            return Get(key, language);
        }

        /// <summary>
        /// Convert Unity's SystemLanguage to our Language enum
        /// </summary>
        public static Language GetSystemLanguage()
        {
            switch (Application.systemLanguage)
            {
                case UnityEngine.SystemLanguage.Portuguese:
                    return Language.Portuguese;
                case UnityEngine.SystemLanguage.Spanish:
                    return Language.Spanish;
                default:
                    return Language.English;
            }
        }

        /// <summary>
        /// Get all text for a POI content plaque (multi-line content)
        /// </summary>
        public static string[] GetContentLines(Key line1Key, Key line2Key, Key line3Key, Language language)
        {
            return new string[]
            {
                Get(line1Key, language),
                Get(line2Key, language),
                Get(line3Key, language)
            };
        }

        /// <summary>
        /// Get all puzzle events as an array
        /// </summary>
        public static string[] GetPuzzleEvents(Language language)
        {
            return new string[]
            {
                Get(Key.Puzzle_Event1, language),
                Get(Key.Puzzle_Event2, language),
                Get(Key.Puzzle_Event3, language),
                Get(Key.Puzzle_Event4, language),
                Get(Key.Puzzle_Event5, language),
                Get(Key.Puzzle_Event6, language),
                Get(Key.Puzzle_Event7, language),
                Get(Key.Puzzle_Event8, language)
            };
        }

        /// <summary>
        /// Get all puzzle hints as an array
        /// </summary>
        public static string[] GetPuzzleHints(Language language)
        {
            return new string[]
            {
                Get(Key.Puzzle_Hint1, language),
                Get(Key.Puzzle_Hint2, language)
            };
        }

        /// <summary>
        /// Get NPC dialogue options as an array
        /// </summary>
        public static string[] GetNPCOptions(Language language)
        {
            return new string[]
            {
                Get(Key.NPC001_Option1, language),
                Get(Key.NPC001_Option2, language),
                Get(Key.NPC001_Option3, language)
            };
        }

        /// <summary>
        /// Get NPC responses as an array
        /// </summary>
        public static string[] GetNPCResponses(Language language)
        {
            return new string[]
            {
                Get(Key.NPC001_Response1, language),
                Get(Key.NPC001_Response2, language),
                Get(Key.NPC001_Response3, language)
            };
        }
    }
}

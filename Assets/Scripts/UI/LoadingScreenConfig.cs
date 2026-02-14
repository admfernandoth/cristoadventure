using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LoadingScreenConfig : ScriptableObject
{
    [Header("Loading Tips")]
    public List<LoadingTip> tips = new List<LoadingTip>();

    [Header("Phase Splash Screens")]
    public List<PhaseSplash> phaseSplashes = new List<PhaseSplash>();

    [Header("Animation Settings")]
    public float tipRotationInterval = 5f;
    public float fadeDuration = 0.5f;
    public float iconRotationSpeed = 180f;

    [Header("Color Scheme")]
    public Color primaryColor = new Color(0.2f, 0.6f, 1f);
    public Color backgroundColor = new Color(0.1f, 0.1f, 0.2f);
    public Color textColor = Color.white;
    public Color errorColor = Color.red;

    [Header("UI Layout Settings")]
    public Vector2 progressBarSize = new Vector2(400, 30);
    public Vector2 progressBarPadding = new Vector2(0, -100);
    public Vector2 tipTextPosition = new Vector2(0, -200);
    public Vector2 phaseSplashPosition = new Vector2(0, 50);
    public Vector2 chapterTextPosition = new Vector2(0, -50);
}

[System.Serializable]
public class LoadingTip
{
    public string id;
    [TextArea(3, 5)]
    public string english;
    [TextArea(3, 5)]
    public string spanish;
    [TextArea(3, 5)]
    public string french;
}

[System.Serializable]
public class PhaseSplash
{
    public string phaseName;
    public Sprite phaseArt;
    public string chapterName;
    public float displayDuration = 3f;
}

// Editor script for creating the configuration
#if UNITY_EDITOR
using UnityEditor;

public class LoadingScreenConfigCreator
{
    [MenuItem("Assets/Create/UI/Loading Screen Config")]
    public static void CreateLoadingScreenConfig()
    {
        LoadingScreenConfig config = ScriptableObject.CreateInstance<LoadingScreenConfig>();

        // Add some default tips
        config.tips.AddRange(new List<LoadingTip>
        {
            new LoadingTip
            {
                id = "tip1",
                english = "Use WASD or Arrow Keys to move your character",
                spanish = "Usa WASD o las Flechas para mover tu personaje",
                french = "Utilisez WASD ou les touches fléchées pour déplacer votre personnage"
            },
            new LoadingTip
            {
                id = "tip2",
                english = "Press Space to jump and interact with objects",
                spanish = "Presiona Espacio para saltar e interactuar con objetos",
                french = "Appuyez sur Espace pour sauter et interagir avec les objets"
            },
            new LoadingTip
            {
                id = "tip3",
                english = "Collect artifacts to unlock new abilities",
                spanish = "Recolecta artefactos para desbloquear nuevas habilidades",
                french = "Collectez des artefacts pour débloquer de nouvelles capacités"
            },
            new LoadingTip
            {
                id = "tip4",
                english = "The Cristo Temple was built in the 15th century",
                spanish = "El Templo de Cristo fue construido en el siglo XV",
                french = "Le Temple de Cristo a été construit au XVe siècle"
            },
            new LoadingTip
            {
                id = "tip5",
                english = "Press ESC to pause the game at any time",
                spanish = "Presiona ESC para pausar el juego en cualquier momento",
                french = "Appuyez sur ESC pour mettre en pause le jeu à tout moment"
            },
            new LoadingTip
            {
                id = "tip6",
                english = "Use the mouse to look around and aim",
                spanish = "Usa el ratón para mirar alrededor y apuntar",
                french = "Utilisez la souris pour regarder autour de vous et viser"
            }
        });

        // Add default phase splashes
        config.phaseSplashes.AddRange(new List<PhaseSplash>
        {
            new PhaseSplash
            {
                phaseName = "Temple Entrance",
                chapterName = "Chapter 1",
                displayDuration = 3f
            },
            new PhaseSplash
            {
                phaseName = "Ancient Hallways",
                chapterName = "Chapter 2",
                displayDuration = 3f
            },
            new PhaseSplash
            {
                phaseName = "Sacred Chambers",
                chapterName = "Chapter 3",
                displayDuration = 3f
            }
        });

        string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/LoadingScreenConfig.asset");
        AssetDatabase.CreateAsset(config, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"LoadingScreenConfig created at {assetPath}");
    }
}
#endif
using UnityEngine;

public class UIPrefabsSetup : MonoBehaviour
{
    /// <summary>
    /// This class provides information for setting up Unity UI prefabs
    /// Create these prefabs in Unity and attach the corresponding scripts:
    ///
    /// 1. PhaseHUD.prefab
    ///    - Top left: Coin counter (coin icon + number)
    ///    - Top center: Phase name "1.1 Belém"
    ///    - Top right: Mini map circle
    ///    - Bottom left: Backpack button
    ///    - Bottom center: Interact prompt (hidden by default)
    ///    - Bottom right: Pause button
    ///    - Attach PhaseHUD.cs
    ///
    /// 2. InfoPanel.prefab
    ///    - Semi-transparent dark panel
    ///    - Title text (TMP)
    ///    - Content text (TMP, scrollable)
    ///    - Icon image (left side)
    ///    - Close button (X)
    ///    - Attach InfoPanel.cs
    ///
    /// 3. RelicViewPanel.prefab
    ///    - Larger panel with golden border
    ///    - Relic image (center)
    ///    - Relic name (TMP, gold)
    ///    - Description (TMP)
    ///    - Verse reference (TMP, italic)
    ///    - Close button
    ///    - Attach RelicViewPanel.cs
    ///
    /// 4. CompletionScreen.prefab
    ///    - Full screen overlay
    ///    - "Phase Complete!" title
    ///    - Stars display (1-3 star icons)
    ///    - Stats (time, POIs visited)
    ///    - Rewards summary
    ///    - Continue/Next Phase button
    ///    - Attach CompletionScreen.cs
    ///
    /// Use StyleGuide.md colors:
    /// - Primary: #3182ce (blue)
    /// - Gold: #d69e2e
    /// - Text: #2d3748
    /// </summary>

    [ContextMenu("Show Prefab Setup Instructions")]
    public void ShowInstructions()
    {
        Debug.Log(@"
PHASE 1.1 UI PREFAB SETUP INSTRUCTIONS:

1. PHASEHUD.PREFAB:
   - Create Canvas with Screen Space - Overlay mode
   - Position: (0, 0, 0), Scale (1, 1, 1)
   - Add PhaseHUD.cs script

   TOP LAYOUT:
   - Coin Counter: GameObject at (-400, 250, 0)
     * Image for coin icon
     * TMP Text for count

   - Phase Name: TMP Text at (0, 250, 0)
     * Text: ""1.1 Belém""
     * Anchored to center top

   - Mini Map: Circle Image at (400, 250, 0)
     - Circular sprite
     - Fixed size

   BOTTOM LAYOUT:
   - Backpack Button: Button at (-400, -250, 0)
     - Icon image
     - Add ClickListener

   - Interact Prompt: GameObject at (0, -250, 0) (disabled by default)
     * TMP Text

   - Pause Button: Button at (400, -250, 0)
     - Pause icon
     - Add ClickListener

2. INFOPANEL.PREFAB:
   - Panel with semi-transparent dark background
   - RectMask for scrollable content
   - Add InfoPanel.cs script
   - Anchor to center of screen
   - Size: (600, 400)

3. RELICVIEWPANEL.PREFAB:
   - Panel with golden border
   - Add RelicViewPanel.cs script
   - Anchor to center
   - Size: (500, 600)

4. COMPLETIONSCREEN.PREFAB:
   - Full screen canvas
   - Background dimmed overlay
   - Add CompletionScreen.cs script
   - Anchor to center
   - Full screen coverage

COLORS:
Primary Blue: #3182ce
Gold: #d69e2e
Text Dark: #2d3748
        ");
    }
}
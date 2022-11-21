using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelsoftGames.PixelUI;
using TMPro;

public class DialogueWriter : MonoBehaviour
{
    [SerializeField]
    private UITypewriter typewriter = null;
    [SerializeField]
    private DialogueScript script = null;
    [SerializeField]
    private MeshRenderer textMeshRenderer = null;
  
    private SpriteRenderer dialogueBubble = null;
    private int currentLine = -1;
    private float maxWidth = 0;
    private float maxHeight = 0;

    private SpriteRenderer DialogueBubble
    {
        get
        {
            if (!dialogueBubble)
            {
                dialogueBubble = GetComponent<SpriteRenderer>();
            }
            return dialogueBubble;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        if (typewriter && DialogueBubble)
        {
            var textMesh = typewriter.GetComponent<RectTransform>();
            var bounds = textMeshRenderer.bounds;
            if (bounds.size.x > maxWidth || bounds.size.y > maxHeight)
            {
                if (bounds.size.x > maxWidth) { maxWidth = bounds.size.x; }
                if (bounds.size.y > maxHeight) { maxHeight = bounds.size.y; }
                
                dialogueBubble.size = new Vector2(maxWidth + 0.5f, maxHeight + 0.5f);
            }
        }
    }
    public void SetDialogueLine(int line)
    {
        if (!script)
        {
            Debug.LogWarning("Dialog writer has no script!");
            return;
        }

        if (!typewriter)
        {
            Debug.LogWarning("Dialog writer has no typewriter!");
            return;
        }

        if (line != currentLine && line < script.Lines.Count && line >= 0)
        {
            currentLine = line;
            typewriter.SetText(script.Lines[currentLine]);
            maxWidth = 0;
            maxHeight = 0;
        }
    }
    public void WriteNextLine()
    {
        SetDialogueLine(currentLine++);
    }

    public void HideLine()
    {
        if (!typewriter)
        {
            Debug.LogWarning("Dialog writer has no typewriter!");
            return;
        }

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void ShowLine()
    {
        if (!typewriter)
        {
            Debug.LogWarning("Dialog writer has no typewriter!");
            return;
        }

        if (!gameObject.activeSelf && DialogueBubble)
        {
            gameObject.SetActive(true);
            dialogueBubble.size = new Vector2(0, 0);
        }
    }
}

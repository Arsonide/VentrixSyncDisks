using UnityEngine;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Snooping;

public class SnoopHighlighterPool : BasePoolManager<SnoopHighlighter>
{
    public static MaterialPropertyBlock FullAlphaBlock = null;

    public static SnoopHighlighterPool Instance;

    protected override void SetupManager()
    {
        base.SetupManager();
        FullAlphaBlock = new MaterialPropertyBlock();
        FullAlphaBlock.SetFloat("_AlphaVal", 1f);
    }

    protected override SnoopHighlighter CreateBaseObject()
    {
        GameObject highlighterObject = new GameObject(nameof(SnoopHighlighter));
        SnoopHighlighter highlighter = highlighterObject.AddComponent<SnoopHighlighter>();
        
        Transform t = highlighterObject.transform;
        t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        t.localScale = Vector3.one;

        Object.DontDestroyOnLoad(highlighterObject);
        highlighterObject.SetActive(false);

        return highlighter;
    }
}
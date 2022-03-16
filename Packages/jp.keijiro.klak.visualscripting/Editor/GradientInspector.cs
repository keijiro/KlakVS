using UnityEditor;
using UnityEngine;
using Unity.VisualScripting;

namespace Klak.VisualScripting {

[Inspector(typeof(Gradient))]
public class GradientInspector : Inspector
{
    public GradientInspector(Metadata metadata) : base(metadata) { }

    public override void Initialize()
    {
        metadata.instantiate = true;
        metadata.instantiator = () => DefaultGradient();
        base.Initialize();
    }

    protected override float GetHeight(float width, GUIContent label)
      => EditorGUIUtility.singleLineHeight;

    protected override void OnGUI(Rect position, GUIContent label)
    {
        position = BeginLabeledBlock(metadata, position, label);

        var newValue = EditorGUI.GradientField(position, (Gradient)metadata.value);

        if (EndBlock(metadata))
        {
            metadata.RecordUndo();
            metadata.value = newValue;
        }
    }

    static Gradient DefaultGradient()
    {
        var colorKeys = new GradientColorKey[]
          { new GradientColorKey(Color.white, 0),
            new GradientColorKey(Color.gray, 1) };

        var alphaKeys = new GradientAlphaKey[]
          { new GradientAlphaKey(1, 0),
            new GradientAlphaKey(0, 1) };

        var grad = new Gradient();
        grad.SetKeys(colorKeys, alphaKeys);

        return grad;
    }
}

} // namespace Klak.VisualScripting

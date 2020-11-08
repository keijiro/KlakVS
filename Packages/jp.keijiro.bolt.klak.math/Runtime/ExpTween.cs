using Ludiq;
using UnityEngine;

namespace Bolt.Addons.Klak.Math {

[UnitCategory("Klak/Tween"), UnitTitle("ExpTween (float)")]
public sealed class ExpTweenFloat : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Current { get; private set; }
    [DoNotSerialize] public ValueInput Target { get; private set; }
    [DoNotSerialize] public ValueInput Speed { get; private set; }
    [DoNotSerialize] public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Current = ValueInput<float>(nameof(Current), 0);
        Target = ValueInput<float>(nameof(Target), 0);
        Speed = ValueInput<float>(nameof(Speed), 5);
        Output = ValueOutput<float>(nameof(Output), Process);
    }

    float Process(Flow flow)
      => Mathf.Lerp(flow.GetValue<float>(Target),
                    flow.GetValue<float>(Current),
                    Mathf.Exp(-flow.GetValue<float>(Speed) * Time.deltaTime));

    #endregion
}

[UnitCategory("Klak/Tween"), UnitTitle("ExpTween (Vector 3)")]
public sealed class ExpTweenVector3 : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Current { get; private set; }
    [DoNotSerialize] public ValueInput Target { get; private set; }
    [DoNotSerialize] public ValueInput Speed { get; private set; }
    [DoNotSerialize] public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Current = ValueInput<Vector3>(nameof(Current), Vector3.zero);
        Target = ValueInput<Vector3>(nameof(Target), Vector3.zero);
        Speed = ValueInput<float>(nameof(Speed), 5);
        Output = ValueOutput<Vector3>(nameof(Output), Process);
    }

    Vector3 Process(Flow flow)
      => Vector3.Lerp(flow.GetValue<Vector3>(Target),
                      flow.GetValue<Vector3>(Current),
                      Mathf.Exp(-flow.GetValue<float>(Speed) * Time.deltaTime));

    #endregion
}


} // namespace Bolt.Addons.Klak.Math

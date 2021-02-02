using Unity.VisualScripting;
using UnityEngine;

namespace Klak.VisualScripting {

[UnitCategory("Klak/Tween"), UnitTitle("ExpTween (float)")]
[RenamedFrom("Bolt.Addons.Klak.Base.ExpTweenFloat")]
public sealed class ExpTweenFloat : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Current { get; private set; }
    [DoNotSerialize] public ValueInput Target { get; private set; }
    [DoNotSerialize] public ValueInput Speed { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

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
[RenamedFrom("Bolt.Addons.Klak.Base.ExpTweenVector3")]
public sealed class ExpTweenVector3 : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Current { get; private set; }
    [DoNotSerialize] public ValueInput Target { get; private set; }
    [DoNotSerialize] public ValueInput Speed { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

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

[UnitCategory("Klak/Tween"), UnitTitle("ExpTween (Quaternion)")]
[RenamedFrom("Bolt.Addons.Klak.Base.ExpTweenQuaternion")]
public sealed class ExpTweenQuaternion : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Current { get; private set; }
    [DoNotSerialize] public ValueInput Target { get; private set; }
    [DoNotSerialize] public ValueInput Speed { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Current = ValueInput<Quaternion>(nameof(Current), Quaternion.identity);
        Target = ValueInput<Quaternion>(nameof(Target), Quaternion.identity);
        Speed = ValueInput<float>(nameof(Speed), 5);
        Output = ValueOutput<Quaternion>(nameof(Output), Process);
    }

    Quaternion Process(Flow flow)
      => Quaternion.Lerp(flow.GetValue<Quaternion>(Target),
                         flow.GetValue<Quaternion>(Current),
                         Mathf.Exp(-flow.GetValue<float>(Speed) * Time.deltaTime));

    #endregion
}

} // namespace Klak.VisualScripting

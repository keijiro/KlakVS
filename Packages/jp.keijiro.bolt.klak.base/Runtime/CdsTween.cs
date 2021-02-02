using Unity.VisualScripting;
using UnityEngine;

namespace Bolt.Addons.Klak.Base {

[UnitCategory("Klak/Tween"), UnitTitle("CdsTween (float)")]
[RenamedFrom("Bolt.Addons.Klak.Math.CdsTweenFloat")]
public sealed class CdsTweenFloat : Unit, IGraphElementWithData
{
    #region Data class

    public sealed class Data : IGraphElementData
    {
        public float Velocity { get; set; }
        public float Value { get; set; }
    }

    public IGraphElementData CreateData() => new Data();

    #endregion

    #region Unit I/O

    [DoNotSerialize, PortLabelHidden]
    public ControlInput Enter { get; private set; }

    [DoNotSerialize] public ValueInput Current { get; private set; }
    [DoNotSerialize] public ValueInput Target { get; private set; }
    [DoNotSerialize] public ValueInput Speed { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ControlOutput Exit { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Enter = ControlInput(nameof(Enter), OnEnter);
        Exit = ControlOutput(nameof(Exit));
        Succession(Enter, Exit);

        Current = ValueInput<float>(nameof(Current), 0);
        Target = ValueInput<float>(nameof(Target), 0);
        Speed = ValueInput<float>(nameof(Speed), 5);
        Output = ValueOutput<float>(nameof(Output), GetOutput);
    }

    float GetOutput(Flow flow)
      => flow.stack.GetElementData<Data>(this).Value;

    ControlOutput OnEnter(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        var current = flow.GetValue<float>(Current);
        var target = flow.GetValue<float>(Target);
        var speed = flow.GetValue<float>(Speed);

        var dt = Time.deltaTime;
        var n1 = data.Velocity - (current - target) * (speed * speed * dt);
        var n2 = 1 + speed * dt;

        data.Velocity = n1 / (n2 * n2);
        data.Value = current + data.Velocity * dt;

        return Exit;
    }

    #endregion
}

[UnitCategory("Klak/Tween"), UnitTitle("CdsTween (Vector 3)")]
[RenamedFrom("Bolt.Addons.Klak.Math.CdsTweenVector3")]
public sealed class CdsTweenVector3 : Unit, IGraphElementWithData
{
    #region Data class

    public sealed class Data : IGraphElementData
    {
        public Vector3 Velocity { get; set; }
        public Vector3 Value { get; set; }
    }

    public IGraphElementData CreateData() => new Data();

    #endregion

    #region Unit I/O

    [DoNotSerialize, PortLabelHidden]
    public ControlInput Enter { get; private set; }

    [DoNotSerialize] public ValueInput Current { get; private set; }
    [DoNotSerialize] public ValueInput Target { get; private set; }
    [DoNotSerialize] public ValueInput Speed { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ControlOutput Exit { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Enter = ControlInput(nameof(Enter), OnEnter);
        Exit = ControlOutput(nameof(Exit));
        Succession(Enter, Exit);

        Current = ValueInput<Vector3>(nameof(Current), Vector3.zero);
        Target = ValueInput<Vector3>(nameof(Target), Vector3.zero);
        Speed = ValueInput<float>(nameof(Speed), 5);
        Output = ValueOutput<Vector3>(nameof(Output), GetOutput);
    }

    Vector3 GetOutput(Flow flow)
      => flow.stack.GetElementData<Data>(this).Value;

    ControlOutput OnEnter(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        var current = flow.GetValue<Vector3>(Current);
        var target = flow.GetValue<Vector3>(Target);
        var speed = flow.GetValue<float>(Speed);

        var dt = Time.deltaTime;
        var n1 = data.Velocity - (current - target) * (speed * speed * dt);
        var n2 = 1 + speed * dt;

        data.Velocity = n1 / (n2 * n2);
        data.Value = current + data.Velocity * dt;

        return Exit;
    }

    #endregion
}

[UnitCategory("Klak/Tween"), UnitTitle("CdsTween (Quaternion)")]
[RenamedFrom("Bolt.Addons.Klak.Math.CdsTweenQuaternion")]
public sealed class CdsTweenQuaternion : Unit, IGraphElementWithData
{
    #region Data class

    public sealed class Data : IGraphElementData
    {
        public Vector4 Velocity { get; set; }
        public Quaternion Value { get; set; }
    }

    public IGraphElementData CreateData() => new Data();

    #endregion

    #region Unit I/O

    [DoNotSerialize, PortLabelHidden]
    public ControlInput Enter { get; private set; }

    [DoNotSerialize] public ValueInput Current { get; private set; }
    [DoNotSerialize] public ValueInput Target { get; private set; }
    [DoNotSerialize] public ValueInput Speed { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ControlOutput Exit { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Enter = ControlInput(nameof(Enter), OnEnter);
        Exit = ControlOutput(nameof(Exit));
        Succession(Enter, Exit);

        Current = ValueInput<Quaternion>(nameof(Current), Quaternion.identity);
        Target = ValueInput<Quaternion>(nameof(Target), Quaternion.identity);
        Speed = ValueInput<float>(nameof(Speed), 5);
        Output = ValueOutput<Quaternion>(nameof(Output), GetOutput);
    }

    Quaternion GetOutput(Flow flow)
      => flow.stack.GetElementData<Data>(this).Value;

    ControlOutput OnEnter(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        var current = flow.GetValue<Quaternion>(Current).ToVector4();
        var target = flow.GetValue<Quaternion>(Target).ToVector4();
        var speed = flow.GetValue<float>(Speed);

        // We can use either of target/-target. Use closer one.
        if (Vector4.Dot(current, target) < 0) target = -target;

        var dt = Time.deltaTime;
        var n1 = data.Velocity - (current - target) * (speed * speed * dt);
        var n2 = 1 + speed * dt;

        data.Velocity = n1 / (n2 * n2);
        data.Value = (current + data.Velocity * dt).ToNormalizedQuaternion();

        return Exit;
    }

    #endregion
}

static class Vector4QuaternionExtensions
{
    public static Vector4 ToVector4(this Quaternion q)
    {
        return new Vector4(q.x, q.y, q.z, q.w);
    }

    public static Quaternion ToNormalizedQuaternion(this Vector4 v)
    {
        v = Vector4.Normalize(v);
        return new Quaternion(v.x, v.y, v.z, v.w);
    }
}

} // namespace Bolt.Addons.Klak.Base

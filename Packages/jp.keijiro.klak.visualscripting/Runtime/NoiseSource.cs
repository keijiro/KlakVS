using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;

namespace Klak.VisualScripting {

static class NoiseUtil
{
    public static float Fbm(float x, float y, uint octave)
    {
        var p = math.float2(x, y);
        var f = 0.0f;
        var w = 0.5f;
        for (var i = 0; i < octave; i++)
        {
            f += w * noise.snoise(p);
            p *= 2.0f;
            w *= 0.5f;
        }
        return f;
    }
}

[UnitCategory("Klak/Noise"), UnitTitle("Noise Source (float)")]
[RenamedFrom("Bolt.Addons.Klak.Base.NoiseSourceFloat")]
public sealed class NoiseSourceFloat : Unit, IGraphElementWithData
{
    #region Data class

    public sealed class Data : IGraphElementData
    {
        public float Time { get; set; }
        public float Value { get; set; }
    }

    public IGraphElementData CreateData() => new Data();

    #endregion

    #region Unit I/O

    [DoNotSerialize, PortLabelHidden]
    public ControlInput Enter { get; private set; }

    [DoNotSerialize] public ValueInput Frequency { get; private set; }
    [DoNotSerialize] public ValueInput Octave { get; private set; }
    [DoNotSerialize] public ValueInput Amplitude { get; private set; }
    [DoNotSerialize] public ValueInput Seed { get; private set; }

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

        Frequency = ValueInput<float>(nameof(Frequency), 1);
        Octave = ValueInput<uint>(nameof(Octave), 1);
        Amplitude = ValueInput<float>(nameof(Amplitude), 1);
        Seed = ValueInput<uint>(nameof(Seed), 1);

        Output = ValueOutput<float>(nameof(Output), GetOutput);
    }

    float GetOutput(Flow flow)
      => flow.stack.GetElementData<Data>(this).Value;

    ControlOutput OnEnter(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        var frequency = flow.GetValue<float>(Frequency);
        var octave = flow.GetValue<uint>(Octave);
        var amplitude = flow.GetValue<float>(Amplitude);
        var seed = flow.GetValue<uint>(Seed);

        var time = data.Time + Time.deltaTime * frequency;
        var n = NoiseUtil.Fbm(XXHash.Float(seed, 0u, -1000, 1000), time, octave);

        data.Time = time;
        data.Value = n * amplitude / 0.75f;

        return Exit;
    }

    #endregion
}

[UnitCategory("Klak/Noise"), UnitTitle("Noise Source (Vector 3)")]
[RenamedFrom("Bolt.Addons.Klak.Base.NoiseSourceVector3")]
public sealed class NoiseSourceVector3 : Unit, IGraphElementWithData
{
    #region Data class

    public sealed class Data : IGraphElementData
    {
        public float Time { get; set; }
        public Vector3 Value { get; set; }
    }

    public IGraphElementData CreateData() => new Data();

    #endregion

    #region Unit I/O

    [DoNotSerialize, PortLabelHidden]
    public ControlInput Enter { get; private set; }

    [DoNotSerialize] public ValueInput Frequency { get; private set; }
    [DoNotSerialize] public ValueInput Octave { get; private set; }
    [DoNotSerialize] public ValueInput Amplitude { get; private set; }
    [DoNotSerialize] public ValueInput Seed { get; private set; }

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

        Frequency = ValueInput<float>(nameof(Frequency), 1);
        Octave = ValueInput<uint>(nameof(Octave), 1);
        Amplitude = ValueInput<Vector3>(nameof(Amplitude), Vector3.one);
        Seed = ValueInput<uint>(nameof(Seed), 1);

        Output = ValueOutput<Vector3>(nameof(Output), GetOutput);
    }

    Vector3 GetOutput(Flow flow)
      => flow.stack.GetElementData<Data>(this).Value;

    ControlOutput OnEnter(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        var frequency = flow.GetValue<float>(Frequency);
        var octave = flow.GetValue<uint>(Octave);
        var amplitude = flow.GetValue<Vector3>(Amplitude);
        var seed = flow.GetValue<uint>(Seed);

        var time = data.Time + Time.deltaTime * frequency;

        var nx = NoiseUtil.Fbm(XXHash.Float(seed, 0u, -1000, 1000), time, octave);
        var ny = NoiseUtil.Fbm(XXHash.Float(seed, 1u, -1000, 1000), time, octave);
        var nz = NoiseUtil.Fbm(XXHash.Float(seed, 2u, -1000, 1000), time, octave);
        var nv = Vector3.Scale(new Vector3(nx, ny, nz), amplitude) / 0.75f;

        data.Time = time;
        data.Value = nv;

        return Exit;
    }

    #endregion
}

[UnitCategory("Klak/Noise"), UnitTitle("Noise Source (Quaternion)")]
[RenamedFrom("Bolt.Addons.Klak.Base.NoiseSourceQuaternion")]
public sealed class NoiseSourceQuaternion : Unit, IGraphElementWithData
{
    #region Data class

    public sealed class Data : IGraphElementData
    {
        public float Time { get; set; }
        public Quaternion Value { get; set; }
    }

    public IGraphElementData CreateData() => new Data();

    #endregion

    #region Unit I/O

    [DoNotSerialize, PortLabelHidden]
    public ControlInput Enter { get; private set; }

    [DoNotSerialize] public ValueInput Frequency { get; private set; }
    [DoNotSerialize] public ValueInput Octave { get; private set; }
    [DoNotSerialize] public ValueInput Angles { get; private set; }
    [DoNotSerialize] public ValueInput Seed { get; private set; }

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

        Frequency = ValueInput<float>(nameof(Frequency), 1);
        Octave = ValueInput<uint>(nameof(Octave), 1);
        Angles = ValueInput<Vector3>(nameof(Angles), Vector3.one);
        Seed = ValueInput<uint>(nameof(Seed), 1);

        Output = ValueOutput<Quaternion>(nameof(Output), GetOutput);
    }

    Quaternion GetOutput(Flow flow)
      => flow.stack.GetElementData<Data>(this).Value;

    ControlOutput OnEnter(Flow flow)
    {
        var data = flow.stack.GetElementData<Data>(this);

        var frequency = flow.GetValue<float>(Frequency);
        var octave = flow.GetValue<uint>(Octave);
        var angles = flow.GetValue<Vector3>(Angles);
        var seed = flow.GetValue<uint>(Seed);

        var time = data.Time + Time.deltaTime * frequency;

        var nx = NoiseUtil.Fbm(XXHash.Float(seed, 0u, -1000, 1000), time, octave);
        var ny = NoiseUtil.Fbm(XXHash.Float(seed, 1u, -1000, 1000), time, octave);
        var nz = NoiseUtil.Fbm(XXHash.Float(seed, 2u, -1000, 1000), time, octave);
        var nv = Vector3.Scale(new Vector3(nx, ny, nz), angles) / 0.75f;

        data.Time = time;
        data.Value = Quaternion.Euler(nv);

        return Exit;
    }

    #endregion
}

} // namespace Klak.VisualScripting

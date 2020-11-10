using Ludiq;
using UnityEngine;

namespace Bolt.Addons.Klak.Base {

static class XXHash
{
    const uint PRIME32_1 = 2654435761U;
    const uint PRIME32_2 = 2246822519U;
    const uint PRIME32_3 = 3266489917U;
    const uint PRIME32_4 = 668265263U;
    const uint PRIME32_5 = 374761393U;

    static uint rotl32(uint x, int r)
      => (x << r) | (x >> 32 - r);

    public static uint Calculate(uint seed, uint data)
    {
        uint h32 = seed + PRIME32_5;
        h32 += 4U;
        h32 += data * PRIME32_3;
        h32 = rotl32(h32, 17) * PRIME32_4;
        h32 ^= h32 >> 15;
        h32 *= PRIME32_2;
        h32 ^= h32 >> 13;
        h32 *= PRIME32_3;
        h32 ^= h32 >> 16;
        return h32;
    }

    public static int Int(uint seed, uint data, int min, int max)
      => (int)(Calculate(seed, data) >> 1) % (max - min) + min;

    public static float Float(uint seed, uint data, float min, float max)
      => Calculate(seed, data) / (float)uint.MaxValue * (max - min) + min;

    public static Vector3 Direction(uint seed, uint data)
    {
        // http://corysimon.github.io/articles/uniformdistn-on-sphere/
        var theta = XXHash.Float(seed, data, 0, Mathf.PI * 2);
        var phi = Mathf.Acos(XXHash.Float(seed, data + 0x10000000, -1, 1));
        var sin_phi = Mathf.Sin(phi);
        var x = sin_phi * Mathf.Cos(theta);
        var y = sin_phi * Mathf.Sin(theta);
        var z = Mathf.Cos(phi);
        return new Vector3(x, y, z);
    }
}

[UnitCategory("Klak/Math"), UnitTitle("XXHash (int)")]
[RenamedFrom("Bolt.Addons.Klak.Math.XXHashInt")]
public sealed class XXHashInt : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Seed { get; private set; }
    [DoNotSerialize] public ValueInput Data { get; private set; }
    [DoNotSerialize] public ValueInput Min { get; private set; }
    [DoNotSerialize] public ValueInput Max { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Seed = ValueInput<uint>(nameof(Seed), 0);
        Data = ValueInput<uint>(nameof(Data), 0);
        Min = ValueInput<int>(nameof(Min), 0);
        Max = ValueInput<int>(nameof(Max), 10);
        Output = ValueOutput<int>(nameof(Output), GetOutput);
    }

    int GetOutput(Flow flow)
      => XXHash.Int(flow.GetValue<uint>(Seed), flow.GetValue<uint>(Data),
                    flow.GetValue<int>(Min), flow.GetValue<int>(Max));

    #endregion
}

[UnitCategory("Klak/Math"), UnitTitle("XXHash (float)")]
[RenamedFrom("Bolt.Addons.Klak.Math.XXHashFloat")]
public sealed class XXHashFloat : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Seed { get; private set; }
    [DoNotSerialize] public ValueInput Data { get; private set; }
    [DoNotSerialize] public ValueInput Min { get; private set; }
    [DoNotSerialize] public ValueInput Max { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Seed = ValueInput<uint>(nameof(Seed), 0);
        Data = ValueInput<uint>(nameof(Data), 0);
        Min = ValueInput<float>(nameof(Min), 0);
        Max = ValueInput<float>(nameof(Max), 1);
        Output = ValueOutput<float>(nameof(Output), GetOutput);
    }

    float GetOutput(Flow flow)
      => XXHash.Float(flow.GetValue<uint>(Seed), flow.GetValue<uint>(Data),
                      flow.GetValue<float>(Min), flow.GetValue<float>(Max));

    #endregion
}

[UnitCategory("Klak/Math"), UnitTitle("XXHash (Vector 3)")]
[RenamedFrom("Bolt.Addons.Klak.Math.XXHashVector3")]
public sealed class XXHashVector3 : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Seed { get; private set; }
    [DoNotSerialize] public ValueInput Data { get; private set; }
    [DoNotSerialize] public ValueInput Min { get; private set; }
    [DoNotSerialize] public ValueInput Max { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Seed = ValueInput<uint>(nameof(Seed), 0);
        Data = ValueInput<uint>(nameof(Data), 0);
        Min = ValueInput<Vector3>(nameof(Min), Vector3.zero);
        Max = ValueInput<Vector3>(nameof(Max), Vector3.one);
        Output = ValueOutput<Vector3>(nameof(Output), GetOutput);
    }

    Vector3 GetOutput(Flow flow)
    {
        var seed = flow.GetValue<uint>(Seed);
        var data = flow.GetValue<uint>(Data);
        var min = flow.GetValue<Vector3>(Min);
        var max = flow.GetValue<Vector3>(Max);
        var x = XXHash.Float(seed, data + 0x00000000, min.x, max.x);
        var y = XXHash.Float(seed, data + 0x10000000, min.y, max.y);
        var z = XXHash.Float(seed, data + 0x20000000, min.z, max.z);
        return new Vector3(x, y, z);
    }

    #endregion
}

[UnitCategory("Klak/Math"), UnitTitle("XXHash (Direction)")]
[RenamedFrom("Bolt.Addons.Klak.Math.XXHashDirection")]
public sealed class XXHashDirection : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Seed { get; private set; }
    [DoNotSerialize] public ValueInput Data { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Seed = ValueInput<uint>(nameof(Seed), 0);
        Data = ValueInput<uint>(nameof(Data), 0);
        Output = ValueOutput<Vector3>(nameof(Output), GetOutput);
    }

    Vector3 GetOutput(Flow flow)
      => XXHash.Direction(flow.GetValue<uint>(Seed),
                          flow.GetValue<uint>(Data));

    #endregion
}

[UnitCategory("Klak/Math"), UnitTitle("XXHash (Rotation)")]
[RenamedFrom("Bolt.Addons.Klak.Math.XXHashRotation")]
public sealed class XXHashRotation : Unit
{
    #region Unit I/O

    [DoNotSerialize] public ValueInput Seed { get; private set; }
    [DoNotSerialize] public ValueInput Data { get; private set; }

    [DoNotSerialize, PortLabelHidden]
    public ValueOutput Output { get; private set; }

    #endregion

    #region Unit implementation

    protected override void Definition()
    {
        Seed = ValueInput<uint>(nameof(Seed), 0);
        Data = ValueInput<uint>(nameof(Data), 0);
        Output = ValueOutput<Quaternion>(nameof(Output), GetOutput);
    }

    Quaternion GetOutput(Flow flow)
      => Quaternion.FromToRotation
           (Vector3.forward,
            XXHash.Direction(flow.GetValue<uint>(Seed),
                             flow.GetValue<uint>(Data)));

    #endregion
}

} // namespace Bolt.Addons.Klak.Base

using Ludiq;
using InputSystem = UnityEngine.InputSystem;

namespace Bolt.Addons.Klak.Base {

[UnitCategory("Klak/Input"), UnitTitle("On Keyboard Input (NIS)")]
public sealed class OnKeyboardInput : MachineEventUnit<EmptyEventArgs>
{
    protected override string hookName => EventHooks.Update;

    [DoNotSerialize] public ValueInput Key { get; private set; }
    [DoNotSerialize] public ValueInput Action { get; private set; }

    protected override void Definition()
    {
        base.Definition();
        Key = ValueInput(nameof(Key), InputSystem.Key.Space);
        Action = ValueInput(nameof(Action), PressState.Down);
    }

    protected override bool ShouldTrigger(Flow flow, EmptyEventArgs args)
    {
        var key = flow.GetValue<InputSystem.Key>(Key);
        var control = InputSystem.Keyboard.current[key];
        var action = flow.GetValue<PressState>(Action);

        switch (action)
        {
            case PressState.Down: return control.wasPressedThisFrame;
            case PressState.Up: return control.wasReleasedThisFrame;
            case PressState.Hold: return control.isPressed;
            default: throw new UnexpectedEnumValueException<PressState>(action);
        }
    }
}

} // namespace Bolt.Addons.Klak.Base

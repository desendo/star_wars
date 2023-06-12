namespace Signals
{
    public readonly struct StartNewGameRequest
    {
    }
    public readonly struct StartNextLevelRequest
    {
    }
    public readonly struct TogglePauseRequest
    {
    }
    public readonly struct SwitchWeaponByIdRequest
    {
        public string WeaponId { get; }

        public SwitchWeaponByIdRequest(string weaponId)
        {
            WeaponId = weaponId;
        }
    }
}
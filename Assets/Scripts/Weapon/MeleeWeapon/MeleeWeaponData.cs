using Player;
public class MeleeWeaponData : WeaponData
{
    public float atkAngle;

    public MeleeWeaponData() { }

    public MeleeWeaponData(MeleeWeaponData data)
    {
        this.atkAngle = data.atkAngle;
        this.damage = data.damage;
        this.weaponName = data.weaponName;
        this.prefab = data.prefab;
    }

}


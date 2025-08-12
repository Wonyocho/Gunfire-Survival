public static class WeaponFactory
{
    public static IWeapon Create(StartWeaponType type)
    {
        switch (type)
        {
            // SMG
            case StartWeaponType.Mzi: return new Mzi();
            case StartWeaponType.Thumbson: return new Thumbson();
            case StartWeaponType.KM_1A: return new KM_1A();
            case StartWeaponType.FFSh_41: return new FFSh_41();
            case StartWeaponType.Mzi_Pro: return new Mzi_Pro();
            case StartWeaponType.MPG55: return new MPG55();
            case StartWeaponType.PX90: return new PX90();
            case StartWeaponType.SPX_7: return new SPX_7();
            case StartWeaponType.VX_9: return new VX_9();
            
            // AR
            case StartWeaponType.KR_2: return new KR_2();
            case StartWeaponType.AKX_47: return new AKX_47();
            case StartWeaponType.STAR: return new STAR();
            case StartWeaponType.HR_416: return new HR_416();
            case StartWeaponType.A16E4: return new A16E4();
            case StartWeaponType.AK_109: return new AK_109();
            case StartWeaponType.ACX: return new ACX();
            case StartWeaponType.AWG: return new AWG();
            case StartWeaponType.ACX_7: return new ACX_7();
            
            // SR
            case StartWeaponType.MR_70: return new MR_70();
            case StartWeaponType.SR_88: return new SR_88();
            case StartWeaponType.micro_14: return new micro_14();
            case StartWeaponType.MR_25: return new MR_25();
            case StartWeaponType.R_24: return new R_24();
            case StartWeaponType.R338: return new R338();
            case StartWeaponType.R82A1: return new R82A1();
            case StartWeaponType.AWX: return new AWX();
            
            // MG
            case StartWeaponType.KM_73: return new KM_73();
            case StartWeaponType.LMG_60: return new LMG_60();
            case StartWeaponType.MG42: return new MG42();
            case StartWeaponType.MG249: return new MG249();
            case StartWeaponType.MGX_5: return new MGX_5();
            case StartWeaponType.LSR_762: return new LSR_762();
            
            default: return new Mzi();
        }
    }
}

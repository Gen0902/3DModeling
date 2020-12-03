using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildProperties
{
    public bool ground = false;
    public bool roof = false;
    public bool corner = false;
    public EBuildingStyle style = EBuildingStyle.None;

    public bool Match(BuildProperties compareProps)
    {
        return (ground == compareProps.ground && roof == compareProps.roof && corner == compareProps.corner 
            && (style == EBuildingStyle.None || compareProps.style == EBuildingStyle.None || style == compareProps.style));
    }

    public override string ToString()
    {
        return $"BuildProp - ground:{ground}, roof:{roof}, corner:{corner}, style:{style.ToString()}";
    }
}

public enum EBuildingStyle
{
    None,
    Round,
    Square,
    Store,
}

public enum EDirection
{
    Self,
    Right,
    Top,
    Back,
    Left,
    Down,
    Front,
}

public enum EOrientation
{
    Front,
    Right,
    Left,
    Back
}

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

    public bool Match(BuildProperties comparedProps)
    {
        return (ground == comparedProps.ground && roof == comparedProps.roof && corner == comparedProps.corner 
            && (style == EBuildingStyle.None || comparedProps.style == EBuildingStyle.None || style == comparedProps.style));
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

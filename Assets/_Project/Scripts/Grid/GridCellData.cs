[System.Serializable]
public struct GridCellData
{
    public int height;
    public GridState[] states;
    public ContentColor[] colors;  
    public Direction[] directions;

    public GridCellData(int height)
    {
        this.height = height;
        states = new GridState[height];
        colors = new ContentColor[height];
        directions = new Direction[height];
        for (int i = 0; i < height; i++)
        {
            states[i] = GridState.Empty;
            colors[i] = ContentColor.Blue; 
            directions[i] = Direction.Up;
        }
    }

    public void SetHeight(int newHeight)
    {
        GridState[] newStates = new GridState[newHeight];
        ContentColor[] newColors = new ContentColor[newHeight]; 
        Direction[] newDirections = new Direction[newHeight];

        for (int i = 0; i < newHeight; i++)
        {
            newStates[i] = i < states.Length ? states[i] : GridState.Empty;
            newColors[i] = i < colors.Length ? colors[i] : ContentColor.Blue; 
            newDirections[i] = i < directions.Length ? directions[i] : Direction.Up;
        }

        states = newStates;
        colors = newColors;
        directions = newDirections;
        height = newHeight;
    }
}
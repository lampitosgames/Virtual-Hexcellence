/// <summary>
/// The PathCells hold state for individual cells in the AIController's pathGrid
/// </summary>
public class PathCell : HexCell {
    //Pathing algorithm fields
    public PathCell parent = null;
    public int g = int.MaxValue;
    //Is there an enemy standing on this cell?
    public bool hasEnemy = false;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="q">column</param>
    /// <param name="r">row</param>
    /// <param name="h">height</param>
    public PathCell(int q, int r, int h) : base(q, r, h) { }

    /// <summary>
    /// Comparitor
    /// </summary>
    /// <param name="o">other AICell</param>
    /// <returns></returns>
    public bool Equals(PathCell o) {
        return (q == o.q && r == o.r && h == o.h);
    }
}

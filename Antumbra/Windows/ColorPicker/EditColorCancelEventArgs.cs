using System.ComponentModel;
using System.Drawing;

namespace Antumbra.Glow
{
  public class EditColorCancelEventArgs : CancelEventArgs
  {
    #region Public Constructors

    public EditColorCancelEventArgs(Color color, int colorIndex)
    {
      this.Color = color;
      this.ColorIndex = colorIndex;
    }

    #endregion

    #region Protected Constructors

    protected EditColorCancelEventArgs()
    { }

    #endregion

    #region Public Properties

    public Color Color { get; protected set; }

    public int ColorIndex { get; protected set; }

    #endregion
  }
}

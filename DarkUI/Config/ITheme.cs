using System.Drawing;

namespace DarkUI.Config
{
    public interface ITheme
    {
        Sizes Sizes { get; }
        
        Colors Colors { get; }
    }
}

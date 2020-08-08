using System.Drawing;

namespace DarkUI.Config
{
    public class DarkTheme : ITheme
    {
        public Sizes Sizes { get; } = new Sizes();

        public Colors Colors { get; } = new Colors();

        public DarkTheme()
        {
            Colors.GreyBackground = Color.FromArgb(60, 63, 65);
            Colors.HeaderBackground = Color.FromArgb(57, 60, 62);
            Colors.BlueBackground = Color.FromArgb(66, 77, 95);
            Colors.DarkBlueBackground = Color.FromArgb(52, 57, 66);
            Colors.DarkBackground = Color.FromArgb(43, 43, 43);
            Colors.MediumBackground = Color.FromArgb(49, 51, 53);
            Colors.LightBackground = Color.FromArgb(69, 73, 74);
            Colors.LighterBackground = Color.FromArgb(95, 101, 102);
            Colors.LightestBackground = Color.FromArgb(178, 178, 178);
            Colors.LightBorder = Color.FromArgb(81, 81, 81);
            Colors.DarkBorder = Color.FromArgb(51, 51, 51);
            Colors.LightText = Color.FromArgb(220, 220, 220);
            Colors.DisabledText = Color.FromArgb(153, 153, 153);
            Colors.BlueHighlight = Color.FromArgb(104, 151, 187);
            Colors.BlueSelection = Color.FromArgb(75, 110, 175);
            Colors.GreyHighlight = Color.FromArgb(122, 128, 132);
            Colors.GreySelection = Color.FromArgb(92, 92, 92);
            Colors.DarkGreySelection = Color.FromArgb(82, 82, 82);
            Colors.DarkBlueBorder = Color.FromArgb(51, 61, 78);
            Colors.LightBlueBorder = Color.FromArgb(86, 97, 114);
            Colors.ActiveControl = Color.FromArgb(159, 178, 196);

            Sizes.Padding = 10;
            Sizes.ScrollBarSize = 15;
            Sizes.ArrowButtonSize = 15;
            Sizes.MinimumThumbSize = 11;
            Sizes.CheckBoxSize = 12;
            Sizes.RadioButtonSize = 12;
            Sizes.ToolWindowHeaderSize = 25;
            Sizes.DocumentTabAreaSize = 24;
            Sizes.ToolWindowTabAreaSize = 21;

        }
    }
}

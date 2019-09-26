using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public static partial class ToolStripSpring
    {
        /// <summary>
        /// This method is intended to be invoked from within the GetPreferredSize(Size constrainingSize)
        /// </summary>
        /// <example>
        /// public class MyToolStripSpringControl : ToolStripControlHost, IToolStripSpring
        /// {
        ///     public override Size GetPreferredSize(Size constrainingSize)
        ///     {
        ///         var basePreferredSize = base.GetPreferredSize(constrainingSize);
        ///         var preferredSize = ToolStripSpring.GetPreferredSize(constrainingSize, this, DefaultSize, basePreferredSize);
        ///         return preferredSize;
        ///     }
        /// }
        /// </example>
        /// <param name="constrainingSize">the constrainginSize passed into the original GetPrefferedSize invocation</param>
        /// <param name="toolStripControlHost">the control we are calculating the preferred size of</param>
        /// <param name="defaultSize">the result of calling this.DefaultSize</param>
        /// <param name="basePreferredSize">the result of calling base.GetPreferredSize(constrainingSize)</param>
        /// <returns>The size that should be returned</returns>
        public static Size GetPreferredSize(
            Size constrainingSize,
            ToolStripControlHost toolStripControlHost,
            Size defaultSize,
            Size basePreferredSize)
        {
            if (toolStripControlHost is IToolStripSpring
                && ((IToolStripSpring)toolStripControlHost).Spring == false)
            {
                return basePreferredSize;
            }

            // Use the default size if the text box is on the overflow menu
            // or is on a vertical ToolStrip.
            if (toolStripControlHost.IsOnOverflow || toolStripControlHost.Owner.Orientation == Orientation.Vertical)
            {
                return defaultSize;
            }

            // Declare a variable to store the total available width as 
            // it is calculated, starting with the display width of the 
            // owning ToolStrip.
            int width = toolStripControlHost.Owner.DisplayRectangle.Width;

            // Subtract the width of the overflow button if it is displayed. 
            if (toolStripControlHost.Owner.OverflowButton.Visible)
            {
                width = width
                    - toolStripControlHost.Owner.OverflowButton.Width
                    - toolStripControlHost.Owner.OverflowButton.Margin.Horizontal;
            }

            // Declare a variable to maintain a count of ToolStripSpringTextBox 
            // items currently displayed in the owning ToolStrip. 
            int springBoxCount = 0;

            foreach (ToolStripItem item in toolStripControlHost.Owner.Items)
            {
                // Ignore items on the overflow menu.
                if (item.IsOnOverflow)
                {
                    continue;
                }

                if (item is IToolStripSpring && ((IToolStripSpring)item).Spring)
                {
                    // For ToolStripSpringTextBox items, increment the count and 
                    // subtract the margin width from the total available width.
                    springBoxCount++;
                    width -= item.Margin.Horizontal;
                }
                else
                {
                    // For all other items, subtract the full width from the total
                    // available width.
                    width = width - item.Width - item.Margin.Horizontal;
                }
            }

            // If there are multiple ToolStripSpringTextBox items in the owning
            // ToolStrip, divide the total available width between them. 
            if (springBoxCount > 1)
            {
                width /= springBoxCount;
            }

            // If the available width is less than the default width, use the
            // default width, forcing one or more items onto the overflow menu.
            if (width < defaultSize.Width)
            {
                width = defaultSize.Width;
            }

            // Retrieve the preferred size from the base class, but change the
            // width to the calculated width. 
            //Size size = base.GetPreferredSize(constrainingSize);
            Size size = new Size { Height = 22 };
            size.Width = width;
            return size;
        }
    }
}

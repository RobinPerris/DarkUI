using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.DataGridView;
using System.Collections;
using System.Drawing.Design;

using System.Reflection;
using System.Drawing.Drawing2D;
using DarkUI.Config;

namespace DarkUI.Controls
{
    public partial class DarkDataGridView : UserControl, ISupportInitialize
    {
        private class DragDropMetaData { };

        private const int _borderWidth = 1;
        private readonly DataGridView _base = new DataGridView();
        private readonly DarkScrollBar _vScrollBar = new DarkScrollBar { ScrollOrientation = DarkScrollOrientation.Vertical };
        private readonly DarkScrollBar _hScrollBar = new DarkScrollBar { ScrollOrientation = DarkScrollOrientation.Horizontal };
        private bool _isInit = false;
        private ScrollBars _scrollBars = ScrollBars.Both;
        private int __dragPosition = -1;

        private readonly static DataGridViewCellStyle _cellStyleUnfocusedEven = GetCellStyle(false, false, false);
        private readonly static DataGridViewCellStyle _cellStyleUnfocusedOdd = GetCellStyle(false, true, false);
        private readonly static DataGridViewCellStyle _cellStyleFocusedEven = GetCellStyle(true, false, false);
        private readonly static DataGridViewCellStyle _cellStyleFocusedOdd = GetCellStyle(true, true, false);
        private readonly static DataGridViewCellStyle _cellStyleHeader = GetCellStyle(true, true, true);
        private readonly static PropertyInfo _dataGridViewDoubleBuffered = typeof(DataGridView).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);

        private static DataGridViewCellStyle GetCellStyle(bool isFocused, bool isOdd, bool isHeader)
        {
            return new DataGridViewCellStyle
            {
                // Darker colors:
                // BackColor = isOdd ? Colors.MediumBackground : Colors.DarkBackground,
                BackColor = isHeader ? Colors.MediumBackground :
                        (isOdd ? Colors.GreyBackground : Colors.HeaderBackground),
                ForeColor = Colors.LightText,
                SelectionBackColor = isFocused ? Colors.BlueSelection : Colors.GreySelection,
                SelectionForeColor = Colors.LightText,
            };
        }

        private const int _dragDrawSideMargin = 3;
        private const int _dragDrawHeight = 2;

        private static readonly Brush _dragDrawBrush = new HatchBrush(HatchStyle.Percent50, Color.Transparent, Color.LightGray);
        private static readonly StringFormat _stringFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };

        private int _scrollSize => DarkUI.Config.Consts.ScrollBarSize;

        public DarkDataGridView()
        {
            Name = "DarkDataGridView";
            OutlineColor = Colors.LightBorder;

            // Configure inner data grid view
            _base.Name = "baseView";
            _base.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _base.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _base.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            _base.RowHeadersVisible = false;
            _base.AllowUserToResizeRows = false;
            _base.BorderStyle = BorderStyle.None;
            _base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _base.ScrollBars = ScrollBars.None;
            _base.EnableHeadersVisualStyles = false;
            _base.AllowDrop = true;
            _dataGridViewDoubleBuffered.SetValue(_base, true, null);

            _base.BackgroundColor = Colors.GreyBackground;
            _base.BackColor = _base.BackgroundColor;
            _base.GridColor = Colors.DarkBorder;
            _base.DefaultCellStyle = _cellStyleUnfocusedEven;
            _base.AlternatingRowsDefaultCellStyle = _cellStyleUnfocusedOdd;
            _base.ColumnHeadersDefaultCellStyle = _cellStyleHeader;
            _base.RowHeadersDefaultCellStyle = _cellStyleHeader;

            _base.CellValueChanged += BaseCellValueChanged;
            _base.MouseWheel += BaseMouseWheel;
            _base.KeyDown += BaseKeyDown;
            _base.MouseMove += BaseMouseMove;
            _base.DragEnter += BaseDragEnter;
            _base.DragOver += BaseDragOver;
            _base.DragLeave += BaseDragLeave;
            _base.DragDrop += BaseDragDrop;
            _base.Paint += BasePaint;
            _base.GotFocus += BaseGotFocus;
            _base.LostFocus += BaseLostFocus;
            _base.RowsAdded += delegate { UpdateScrollBarLayout(); };
            _base.RowsRemoved += delegate { UpdateScrollBarLayout(); };
            _base.ColumnStateChanged += delegate { UpdateScrollBarLayout(); };
            _base.ColumnWidthChanged += delegate { UpdateScrollBarLayout(); };
            _base.ColumnAdded += delegate { UpdateScrollBarLayout(); };
            _base.ColumnRemoved += delegate { UpdateScrollBarLayout(); };
            _base.Scroll += BaseScrolled;

            // Configure scroll bars
            _vScrollBar.BackColor = Colors.MediumBackground;
            _vScrollBar.Minimum = 0;
            _vScrollBar.Maximum = 0;
            _vScrollBar.ValueChanged += _vScrollBar_ValueChanged;

            _hScrollBar.BackColor = Colors.MediumBackground;
            _hScrollBar.Minimum = 0;
            _hScrollBar.Maximum = 0;
            _hScrollBar.ValueChanged += _hScrollBar_ValueChanged;

            UpdateScrollBarLayout();

            // Add controls
            Controls.Add(_base);
            Controls.Add(_vScrollBar);
            Controls.Add(_hScrollBar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _base?.Dispose();
                _hScrollBar?.Dispose();
                _vScrollBar?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void BaseCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Raise a data event update if necessary
            var dataSource = DataSource as IBindingList;
            if (dataSource != null)
            {
                object obj = dataSource[e.RowIndex];
                if (!(obj is INotifyPropertyChanged))
                    dataSource[e.RowIndex] = obj;
            }
        }

        private void BaseMouseWheel(object sender, MouseEventArgs e)
        {
            _vScrollBar.ScrollBy(e.Delta < 0 ? 1 : -1);
        }

        private void BaseKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.V) && (e.Modifiers == Keys.Control) && AllowUserToPasteCells)
            {
                string clipboardString = System.Windows.Forms.Clipboard.GetDataObject().GetData(DataFormats.UnicodeText) as string;
                if (clipboardString == null)
                    return;

                var columns = Columns.Cast<DataGridViewColumn>()
                    .Where(column => column.Visible)
                    .OrderBy(column => column.DisplayIndex).ToList();


                // Get upper left point of selection
                int startColumn = int.MaxValue;
                int startRow = int.MaxValue;
                foreach (DataGridViewCell cell in SelectedCells)
                {
                    startColumn = Math.Min(startColumn, columns.IndexOf(cell.OwningColumn));
                    startRow = Math.Min(startRow, cell.RowIndex);
                }
                if ((startColumn == int.MaxValue) || (startRow == int.MaxValue))
                    return;

                // Paste string
                string[] rowSeparator = new string[] { "\r", "\n" };
                string[] columnSeperator = new string[] { "\t" };
                string[] pastedRows = clipboardString.Split(rowSeparator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < pastedRows.GetLength(0); ++i)
                {
                    if ((startRow + i) >= Rows.Count)
                        break;

                    var row = Rows[startRow + i];
                    string[] pastedCells = pastedRows[i].Split(columnSeperator, StringSplitOptions.None);
                    for (int j = 0; j < pastedCells.GetLength(0); ++j)
                    {
                        if ((startColumn + j) >= columns.Count)
                            break;
                        var cell = row.Cells[columns[startColumn + j].Name];
                        if ((cell.ReadOnly) || (cell.OwningColumn is DataGridViewButtonColumn))
                            continue;
                        cell.Value = pastedCells[j];
                        cell.Selected = true;
                    }
                }
            }
        }

        private int _dragPosition
        {
            get { return __dragPosition; }
            set
            {
                if (value == _dragPosition)
                    return;
                __dragPosition = value;
                _base.Invalidate();
            }
        }

        private int GetCellDisplayY(int rowIndex)
        {
            Rectangle cellBounds = GetCellDisplayRectangle(0, Math.Max(0, Math.Min(Rows.Count - 1, rowIndex)), false);
            if (rowIndex >= Rows.Count)
                return cellBounds.Bottom;
            else
                return cellBounds.Y;
        }

        private void BaseMouseMove(object sender, MouseEventArgs e)
        {
            if (AllowUserToDragDropRows && !IsCurrentCellInEditMode &&
                (e.Button == MouseButtons.Left) && (ModifierKeys == Keys.None))
            {
                var info = HitTest(e.Location);
                switch (info.Type)
                {
                    case DataGridViewHitTestType.Cell:
                    case DataGridViewHitTestType.RowHeader:
                        if (Rows[info.RowIndex].Cells[info.ColumnIndex].Selected)
                            DoDragDrop(new DragDropMetaData(), DragDropEffects.Move);
                        break;
                }
            }
        }

        private void BaseDragEnter(object sender, DragEventArgs e)
        {
            // Don't call the base to not cancel the drag drop operation
            e.Effect = DragDropEffects.All;
        }

        private void BaseDragOver(object sender, DragEventArgs e)
        {
            // Is it from the data grid view itself?
            if (e.Data.GetData(typeof(DragDropMetaData)) == null)
            {
                _dragPosition = -1;
                return;
            }

            // Do drag drop if possible
            Point localPoint = PointToClient(new Point(e.X, e.Y));
            var info = HitTest(localPoint);
            switch (info.Type)
            {
                case DataGridViewHitTestType.Cell:
                case DataGridViewHitTestType.RowHeader:
                    _dragPosition = info.RowIndex;
                    break;
                case DataGridViewHitTestType.None:
                    _dragPosition = Rows.Count;
                    break;
                default:
                    _dragPosition = -1;
                    break;
            }
        }

        private void BaseDragLeave(object sender, EventArgs e)
        {
            // End drag and drop if necessary
            _dragPosition = -1;
        }

        private void BaseDragDrop(object sender, DragEventArgs e)
        {
            // Get drag data
            var metaData = e.Data.GetData(typeof(DragDropMetaData)) as DragDropMetaData;
            if ((metaData == null) || (_dragPosition < 0))
                return;

            IList rows = EditableRowCollection;

            object[] previousRows = Rows.Cast<DataGridViewRow>()
                .Where(row => row.Index < _dragPosition)
                .Except(SelectedRows.Cast<DataGridViewRow>())
                .Select(row => rows[row.Index])
                .ToArray();

            object[] followingRows = Rows.Cast<DataGridViewRow>()
                .Where(row => row.Index >= _dragPosition)
                .Except(SelectedRows.Cast<DataGridViewRow>())
                .Select(row => rows[row.Index])
                .ToArray();

            // Remove unselected rows
            foreach (var row in previousRows)
                rows.Remove(row);
            foreach (var row in followingRows)
                rows.Remove(row);

            // Insert rows
            for (int i = 0; i < previousRows.GetLength(0); ++i)
                rows.Insert(i, previousRows[i]);
            for (int i = 0; i < followingRows.GetLength(0); ++i)
                rows.Add(followingRows[i]);


            /*var selectedRows = SelectedRows.Cast<DataGridViewRow>().ToList();
             foreach (var row in selectedRows)
                 row.Selected = false;
             foreach (var row in selectedRows)
                 row.Selected = true;*/

            _dragPosition = -1;
        }

        private void BasePaint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw drag
            if ((_dragPosition >= 0) && (_dragPosition <= Rows.Count))
            {
                int positionY = GetCellDisplayY(_dragPosition);

                e.Graphics.FillRectangle(_dragDrawBrush, _dragDrawSideMargin,
                    positionY - _dragDrawHeight / 2, Width - _dragDrawSideMargin * 2, _dragDrawHeight);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateScrollBarLayout();
        }

        private void BaseLostFocus(object sender, EventArgs e)
        {
            _base.DefaultCellStyle = _cellStyleUnfocusedEven;
            _base.AlternatingRowsDefaultCellStyle = _cellStyleUnfocusedOdd;
        }

        private void BaseGotFocus(object sender, EventArgs e)
        {
            _base.DefaultCellStyle = _cellStyleFocusedEven;
            _base.AlternatingRowsDefaultCellStyle = _cellStyleFocusedOdd;
        }

        private void _hScrollBar_ValueChanged(object sender, ScrollValueEventArgs e)
        {
            _base.HorizontalScrollingOffset = Math.Max(0, Math.Min(TotalColumnsWidth - 1, e.Value));
        }

        private void _vScrollBar_ValueChanged(object sender, ScrollValueEventArgs e)
        {
            _base.FirstDisplayedScrollingRowIndex = Math.Max(0, Math.Min(_base.Rows.Count - 1, e.Value));
        }

        private void BaseScrolled(object sender, ScrollEventArgs e)
        {
            if (_hScrollBar.Value != _base.HorizontalScrollingOffset)
                _hScrollBar.Value = _base.HorizontalScrollingOffset;
            if (_vScrollBar.Value != _base.FirstDisplayedScrollingRowIndex)
                _vScrollBar.Value = _base.FirstDisplayedScrollingRowIndex;
        }

        public int CountVisibleRows(int startRow, int controlHeight)
        {
            int remainingHeight = controlHeight - _base.ColumnHeadersHeight;
            if (remainingHeight <= 0)
                return 0;

            // Check for frozen rows
            int result = 0;
            foreach (DataGridViewRow row in _base.Rows)
            {
                if (!row.State.HasFlag(DataGridViewElementStates.Visible))
                    continue;
                if (!row.State.HasFlag(DataGridViewElementStates.Frozen))
                    break;

                remainingHeight -= row.Height;
                if (remainingHeight <= 0)
                    return result;
                ++result;
            }

            // Check for visible rows
            foreach (var row in _base.Rows.Cast<DataGridViewRow>().Skip(startRow))
            {
                if (!row.State.HasFlag(DataGridViewElementStates.Visible))
                    continue;

                remainingHeight -= row.Height;
                if (remainingHeight <= 0)
                    return result;
                ++result;
            }

            return result;
        }

        private bool _updateScrollBarLayout = false;

        private void UpdateScrollBarLayout()
        {
            if (_isInit || _updateScrollBarLayout) // Don't update recursively
                return;

            try
            {
                _updateScrollBarLayout = true;

                Size size = ClientSize;

                // Do update twice to ensure proper updating if columns auto resize during the first update
                for (int i = 0; i < 2; ++i)
                {
                    Rectangle baseBounds = new Rectangle(_borderWidth, _borderWidth, size.Width - _borderWidth * 2, size.Height - _borderWidth * 2);
                    Rectangle hScrollBarBounds = new Rectangle(_borderWidth, size.Height - _scrollSize - _borderWidth, size.Width - _borderWidth * 2, _scrollSize);
                    Rectangle vScrollBarBounds = new Rectangle(size.Width - _scrollSize - _borderWidth, _borderWidth, _scrollSize, size.Height - _borderWidth * 2);

                    // Setup rows
                    int rowCount = _base.Rows.Count;
                    int rowInView = CountVisibleRows(_base.FirstDisplayedScrollingRowIndex, size.Height - _scrollSize);
                    bool rowScrollVisible = _scrollBars.HasFlag(ScrollBars.Vertical) && (rowInView < rowCount);
                    if (rowScrollVisible)
                    {
                        _vScrollBar.ViewSize = rowInView + 1;
                        _vScrollBar.Maximum = Math.Max(rowInView + 2, rowCount);

                        baseBounds.Width -= _scrollSize;
                        hScrollBarBounds.Width -= _scrollSize;
                    }

                    // Setup columns
                    int columnsPixelCount = TotalColumnsWidth;
                    int columnsPixelInView = size.Width - 2 * _borderWidth;
                    bool columnScrollVisible = _scrollBars.HasFlag(ScrollBars.Horizontal) && (columnsPixelInView < columnsPixelCount);
                    if (columnScrollVisible)
                    {
                        _hScrollBar.ViewSize = columnsPixelInView;
                        _hScrollBar.Maximum = Math.Max(columnsPixelInView + 1, columnsPixelCount);

                        baseBounds.Height -= _scrollSize;
                        vScrollBarBounds.Height -= _scrollSize;
                    }

                    // Update layout
                    _base.Bounds = baseBounds;

                    if (rowScrollVisible)
                        _vScrollBar.Bounds = vScrollBarBounds;
                    _vScrollBar.Visible = rowScrollVisible;

                    if (columnScrollVisible)
                        _hScrollBar.Bounds = hScrollBarBounds;
                    _hScrollBar.Visible = columnScrollVisible;
                }
            }
            finally
            {
                _updateScrollBarLayout = false;
            }
        }

        public IList EditableRowCollection
        {
            get
            {
                if (DataSource == null)
                    return Rows;
                else
                    return (IList)DataSource;
            }
        }

        public int TotalColumnsWidth
        {
            get
            {
                int totalColumnsWidth = 0;
                foreach (DataGridViewColumn column in _base.Columns)
                    totalColumnsWidth += column.Width;
                return totalColumnsWidth;
            }
        }

        [ReadOnly(true)]
        public Color OutlineColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DefaultValue(ScrollBars.Both)]
        public ScrollBars ScrollBars
        {
            get { return _scrollBars; }
            set
            {
                if (_scrollBars == value)
                    return;
                _scrollBars = value;
                UpdateScrollBarLayout();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell this[int columnIndex, int rowIndex]
        {
            get { return _base[columnIndex, rowIndex]; }
            set { _base[columnIndex, rowIndex] = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell this[string columnName, int rowIndex]
        {
            get { return _base[columnName, rowIndex]; }
            set { _base[columnName, rowIndex] = value; }
        }

        public void BeginInit()
        {
            ((ISupportInitialize)_base).BeginInit();
            _isInit = true;
        }

        public void EndInit()
        {
            ((ISupportInitialize)_base).EndInit();
            _isInit = false;
            UpdateScrollBarLayout();
        }

        // Forward functions and settings of the normal data grid view. (automatically generated perfect forwarding)
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdjustedTopLeftHeaderBorderStyle { get { return _base.AdjustedTopLeftHeaderBorderStyle; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdvancedCellBorderStyle { get { return _base.AdvancedCellBorderStyle; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdvancedColumnHeadersBorderStyle { get { return _base.AdvancedColumnHeadersBorderStyle; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdvancedRowHeadersBorderStyle { get { return _base.AdvancedRowHeadersBorderStyle; } }
        [DefaultValue(true)]
        public bool AllowUserToPasteCells { get; set; } = true;
        [DefaultValue(true)]
        public bool AllowUserToDragDropRows { get; set; } = true;
        [DefaultValue(true)]
        public bool AllowUserToAddRows { get { return _base.AllowUserToAddRows; } set { _base.AllowUserToAddRows = value; } }
        [DefaultValue(true)]
        public bool AllowUserToDeleteRows { get { return _base.AllowUserToDeleteRows; } set { _base.AllowUserToDeleteRows = value; } }
        [DefaultValue(false)]
        public bool AllowUserToOrderColumns { get { return _base.AllowUserToOrderColumns; } set { _base.AllowUserToOrderColumns = value; } }
        [DefaultValue(true)]
        public bool AllowUserToResizeColumns { get { return _base.AllowUserToResizeColumns; } set { _base.AllowUserToResizeColumns = value; } }
        [DefaultValue(false)]
        public bool AllowUserToResizeRows { get { return _base.AllowUserToResizeRows; } set { _base.AllowUserToResizeRows = value; } }
        [Browsable(false)]
        [DefaultValue(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool AutoGenerateColumns { get { return _base.AutoGenerateColumns; } set { _base.AutoGenerateColumns = value; } }
        public new bool AutoSize { get { return _base.AutoSize; } set { _base.AutoSize = value; } }
        [DefaultValue(DataGridViewAutoSizeColumnsMode.None)]
        public DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode { get { return _base.AutoSizeColumnsMode; } set { _base.AutoSizeColumnsMode = value; } }
        [DefaultValue(DataGridViewAutoSizeRowsMode.None)]
        public DataGridViewAutoSizeRowsMode AutoSizeRowsMode { get { return _base.AutoSizeRowsMode; } set { _base.AutoSizeRowsMode = value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ReadOnly(true)]
        public new Color BackColor { get { return _base.BackColor; } set { _base.BackColor = value; } }
        [ReadOnly(true)]
        public Color BackgroundColor { get { return _base.BackgroundColor; } set { _base.BackgroundColor = value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Image BackgroundImage { get { return _base.BackgroundImage; } set { _base.BackgroundImage = value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new ImageLayout BackgroundImageLayout { get { return _base.BackgroundImageLayout; } set { _base.BackgroundImageLayout = value; } }
        [DefaultValue(BorderStyle.None)]
        public new BorderStyle BorderStyle { get { return _base.BorderStyle; } set { _base.BorderStyle = value; } }
        [Browsable(true)]
        [DefaultValue(DataGridViewCellBorderStyle.Single)]
        public DataGridViewCellBorderStyle CellBorderStyle { get { return _base.CellBorderStyle; } set { _base.CellBorderStyle = value; } }
        [Browsable(true)]
        [DefaultValue(DataGridViewClipboardCopyMode.EnableWithAutoHeaderText)]
        public DataGridViewClipboardCopyMode ClipboardCopyMode { get { return _base.ClipboardCopyMode; } set { _base.ClipboardCopyMode = value; } }
        [Browsable(false)]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int ColumnCount { get { return _base.ColumnCount; } set { _base.ColumnCount = value; } }
        [Browsable(true)]
        [DefaultValue(DataGridViewHeaderBorderStyle.Single)]
        public DataGridViewHeaderBorderStyle ColumnHeadersBorderStyle { get { return _base.ColumnHeadersBorderStyle; } set { _base.ColumnHeadersBorderStyle = value; } }
        [Localizable(true)]
        public int ColumnHeadersHeight { get { return _base.ColumnHeadersHeight; } set { _base.ColumnHeadersHeight = value; } }
        [DefaultValue(DataGridViewColumnHeadersHeightSizeMode.AutoSize)]
        [RefreshProperties(RefreshProperties.All)]
        public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode { get { return _base.ColumnHeadersHeightSizeMode; } set { _base.ColumnHeadersHeightSizeMode = value; } }
        [DefaultValue(true)]
        public bool ColumnHeadersVisible { get { return _base.ColumnHeadersVisible; } set { _base.ColumnHeadersVisible = value; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(ColumnEditor), typeof(UITypeEditor))]
        [MergableProperty(false)]
        public DataGridViewColumnCollection Columns { get { return _base.Columns; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell CurrentCell { get { return _base.CurrentCell; } set { _base.CurrentCell = value; } }
        [Browsable(false)]
        public Point CurrentCellAddress { get { return _base.CurrentCellAddress; } }
        [Browsable(false)]
        public DataGridViewRow CurrentRow { get { return _base.CurrentRow; } }
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string DataMember { get { return _base.DataMember; } set { _base.DataMember = value; } }
        [AttributeProvider(typeof(IListSource))]
        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public object DataSource { get { return _base.DataSource; } set { _base.DataSource = value; } }
        public new Rectangle DisplayRectangle { get { return _base.DisplayRectangle; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Control EditingControl { get { return _base.EditingControl; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Panel EditingPanel { get { return _base.EditingPanel; } }
        [DefaultValue(DataGridViewEditMode.EditOnKeystrokeOrF2)]
        public DataGridViewEditMode EditMode { get { return _base.EditMode; } set { _base.EditMode = value; } }
        [DefaultValue(false)]
        public bool EnableHeadersVisualStyles { get { return _base.EnableHeadersVisualStyles; } set { _base.EnableHeadersVisualStyles = value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell FirstDisplayedCell { get { return _base.FirstDisplayedCell; } set { _base.FirstDisplayedCell = value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int FirstDisplayedScrollingColumnHiddenWidth { get { return _base.FirstDisplayedScrollingColumnHiddenWidth; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FirstDisplayedScrollingColumnIndex { get { return _base.FirstDisplayedScrollingColumnIndex; } set { _base.FirstDisplayedScrollingColumnIndex = value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FirstDisplayedScrollingRowIndex { get { return _base.FirstDisplayedScrollingRowIndex; } set { _base.FirstDisplayedScrollingRowIndex = value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new Font Font { get { return _base.Font; } set { _base.Font = value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new Color ForeColor { get { return _base.ForeColor; } set { _base.ForeColor = value; } }
        [ReadOnly(true)]
        public Color GridColor { get { return _base.GridColor; } set { _base.GridColor = value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int HorizontalScrollingOffset { get { return _base.HorizontalScrollingOffset; } set { _base.HorizontalScrollingOffset = value; } }
        [Browsable(false)]
        public bool IsCurrentCellDirty { get { return _base.IsCurrentCellDirty; } }
        [Browsable(false)]
        public bool IsCurrentCellInEditMode { get { return _base.IsCurrentCellInEditMode; } }
        [Browsable(false)]
        public bool IsCurrentRowDirty { get { return _base.IsCurrentRowDirty; } }
        [DefaultValue(true)]
        public bool MultiSelect { get { return _base.MultiSelect; } set { _base.MultiSelect = value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int NewRowIndex { get { return _base.NewRowIndex; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding { get { return _base.Padding; } set { _base.Padding = value; } }
        [Browsable(true)]
        [DefaultValue(false)]
        public bool ReadOnly { get { return _base.ReadOnly; } set { _base.ReadOnly = value; } }
        [Browsable(false)]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public int RowCount { get { return _base.RowCount; } set { _base.RowCount = value; } }
        [Browsable(true)]
        [DefaultValue(DataGridViewHeaderBorderStyle.Single)]
        public DataGridViewHeaderBorderStyle RowHeadersBorderStyle { get { return _base.RowHeadersBorderStyle; } set { _base.RowHeadersBorderStyle = value; } }
        [DefaultValue(false)]
        public bool RowHeadersVisible { get { return _base.RowHeadersVisible; } set { _base.RowHeadersVisible = value; } }
        [Localizable(true)]
        public int RowHeadersWidth { get { return _base.RowHeadersWidth; } set { _base.RowHeadersWidth = value; } }
        [DefaultValue(DataGridViewRowHeadersWidthSizeMode.EnableResizing)]
        [RefreshProperties(RefreshProperties.All)]
        public DataGridViewRowHeadersWidthSizeMode RowHeadersWidthSizeMode { get { return _base.RowHeadersWidthSizeMode; } set { _base.RowHeadersWidthSizeMode = value; } }
        [Browsable(false)]
        public DataGridViewRowCollection Rows { get { return _base.Rows; } }
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DataGridViewRow RowTemplate { get { return _base.RowTemplate; } set { _base.RowTemplate = value; } }
        [Browsable(false)]
        public DataGridViewSelectedCellCollection SelectedCells { get { return _base.SelectedCells; } }
        [Browsable(false)]
        public DataGridViewSelectedColumnCollection SelectedColumns { get { return _base.SelectedColumns; } }
        [Browsable(false)]
        public DataGridViewSelectedRowCollection SelectedRows { get { return _base.SelectedRows; } }
        [Browsable(true)]
        [DefaultValue(DataGridViewSelectionMode.FullRowSelect)]
        public DataGridViewSelectionMode SelectionMode { get { return _base.SelectionMode; } set { _base.SelectionMode = value; } }
        [DefaultValue(true)]
        public bool ShowCellErrors { get { return _base.ShowCellErrors; } set { _base.ShowCellErrors = value; } }
        [DefaultValue(true)]
        public bool ShowCellToolTips { get { return _base.ShowCellToolTips; } set { _base.ShowCellToolTips = value; } }
        [DefaultValue(true)]
        public bool ShowEditingIcon { get { return _base.ShowEditingIcon; } set { _base.ShowEditingIcon = value; } }
        [DefaultValue(true)]
        public bool ShowRowErrors { get { return _base.ShowRowErrors; } set { _base.ShowRowErrors = value; } }
        [Browsable(false)]
        public DataGridViewColumn SortedColumn { get { return _base.SortedColumn; } }
        [Browsable(false)]
        public SortOrder SortOrder { get { return _base.SortOrder; } }
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool StandardTab { get { return _base.StandardTab; } set { _base.StandardTab = value; } }
        [Bindable(false)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string Text { get { return _base.Text; } set { _base.Text = value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewHeaderCell TopLeftHeaderCell { get { return _base.TopLeftHeaderCell; } set { _base.TopLeftHeaderCell = value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Cursor UserSetCursor { get { return _base.UserSetCursor; } }
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool VirtualMode { get { return _base.VirtualMode; } set { _base.VirtualMode = value; } }
        public event EventHandler AllowUserToAddRowsChanged { add { _base.AllowUserToAddRowsChanged += value; } remove { _base.AllowUserToAddRowsChanged -= value; } }
        public event EventHandler AllowUserToDeleteRowsChanged { add { _base.AllowUserToDeleteRowsChanged += value; } remove { _base.AllowUserToDeleteRowsChanged -= value; } }
        public event EventHandler AllowUserToOrderColumnsChanged { add { _base.AllowUserToOrderColumnsChanged += value; } remove { _base.AllowUserToOrderColumnsChanged -= value; } }
        public event EventHandler AllowUserToResizeColumnsChanged { add { _base.AllowUserToResizeColumnsChanged += value; } remove { _base.AllowUserToResizeColumnsChanged -= value; } }
        public event EventHandler AllowUserToResizeRowsChanged { add { _base.AllowUserToResizeRowsChanged += value; } remove { _base.AllowUserToResizeRowsChanged -= value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler AutoGenerateColumnsChanged { add { _base.AutoGenerateColumnsChanged += value; } remove { _base.AutoGenerateColumnsChanged -= value; } }
        public event DataGridViewAutoSizeColumnModeEventHandler AutoSizeColumnModeChanged { add { _base.AutoSizeColumnModeChanged += value; } remove { _base.AutoSizeColumnModeChanged -= value; } }
        public event DataGridViewAutoSizeColumnsModeEventHandler AutoSizeColumnsModeChanged { add { _base.AutoSizeColumnsModeChanged += value; } remove { _base.AutoSizeColumnsModeChanged -= value; } }
        public event DataGridViewAutoSizeModeEventHandler AutoSizeRowsModeChanged { add { _base.AutoSizeRowsModeChanged += value; } remove { _base.AutoSizeRowsModeChanged -= value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler BackColorChanged { add { _base.BackColorChanged += value; } remove { _base.BackColorChanged -= value; } }
        public event EventHandler BackgroundColorChanged { add { _base.BackgroundColorChanged += value; } remove { _base.BackgroundColorChanged -= value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler BackgroundImageChanged { add { _base.BackgroundImageChanged += value; } remove { _base.BackgroundImageChanged -= value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler BackgroundImageLayoutChanged { add { _base.BackgroundImageLayoutChanged += value; } remove { _base.BackgroundImageLayoutChanged -= value; } }
        public event EventHandler BorderStyleChanged { add { _base.BorderStyleChanged += value; } remove { _base.BorderStyleChanged -= value; } }
        public event QuestionEventHandler CancelRowEdit { add { _base.CancelRowEdit += value; } remove { _base.CancelRowEdit -= value; } }
        public event DataGridViewCellCancelEventHandler CellBeginEdit { add { _base.CellBeginEdit += value; } remove { _base.CellBeginEdit -= value; } }
        public event EventHandler CellBorderStyleChanged { add { _base.CellBorderStyleChanged += value; } remove { _base.CellBorderStyleChanged -= value; } }
        public event DataGridViewCellEventHandler CellClick { add { _base.CellClick += value; } remove { _base.CellClick -= value; } }
        public event DataGridViewCellEventHandler CellContentClick { add { _base.CellContentClick += value; } remove { _base.CellContentClick -= value; } }
        public event DataGridViewCellEventHandler CellContentDoubleClick { add { _base.CellContentDoubleClick += value; } remove { _base.CellContentDoubleClick -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellEventHandler CellContextMenuStripChanged { add { _base.CellContextMenuStripChanged += value; } remove { _base.CellContextMenuStripChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellContextMenuStripNeededEventHandler CellContextMenuStripNeeded { add { _base.CellContextMenuStripNeeded += value; } remove { _base.CellContextMenuStripNeeded -= value; } }
        public event DataGridViewCellEventHandler CellDoubleClick { add { _base.CellDoubleClick += value; } remove { _base.CellDoubleClick -= value; } }
        public event DataGridViewCellEventHandler CellEndEdit { add { _base.CellEndEdit += value; } remove { _base.CellEndEdit -= value; } }
        public event DataGridViewCellEventHandler CellEnter { add { _base.CellEnter += value; } remove { _base.CellEnter -= value; } }
        public event DataGridViewCellEventHandler CellErrorTextChanged { add { _base.CellErrorTextChanged += value; } remove { _base.CellErrorTextChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellErrorTextNeededEventHandler CellErrorTextNeeded { add { _base.CellErrorTextNeeded += value; } remove { _base.CellErrorTextNeeded -= value; } }
        public event DataGridViewCellFormattingEventHandler CellFormatting { add { _base.CellFormatting += value; } remove { _base.CellFormatting -= value; } }
        public event DataGridViewCellEventHandler CellLeave { add { _base.CellLeave += value; } remove { _base.CellLeave -= value; } }
        public event DataGridViewCellMouseEventHandler CellMouseClick { add { _base.CellMouseClick += value; } remove { _base.CellMouseClick -= value; } }
        public event DataGridViewCellMouseEventHandler CellMouseDoubleClick { add { _base.CellMouseDoubleClick += value; } remove { _base.CellMouseDoubleClick -= value; } }
        public event DataGridViewCellMouseEventHandler CellMouseDown { add { _base.CellMouseDown += value; } remove { _base.CellMouseDown -= value; } }
        public event DataGridViewCellEventHandler CellMouseEnter { add { _base.CellMouseEnter += value; } remove { _base.CellMouseEnter -= value; } }
        public event DataGridViewCellEventHandler CellMouseLeave { add { _base.CellMouseLeave += value; } remove { _base.CellMouseLeave -= value; } }
        public event DataGridViewCellMouseEventHandler CellMouseMove { add { _base.CellMouseMove += value; } remove { _base.CellMouseMove -= value; } }
        public event DataGridViewCellMouseEventHandler CellMouseUp { add { _base.CellMouseUp += value; } remove { _base.CellMouseUp -= value; } }
        public event DataGridViewCellPaintingEventHandler CellPainting { add { _base.CellPainting += value; } remove { _base.CellPainting -= value; } }
        public event DataGridViewCellParsingEventHandler CellParsing { add { _base.CellParsing += value; } remove { _base.CellParsing -= value; } }
        public event DataGridViewCellStateChangedEventHandler CellStateChanged { add { _base.CellStateChanged += value; } remove { _base.CellStateChanged -= value; } }
        public event DataGridViewCellEventHandler CellStyleChanged { add { _base.CellStyleChanged += value; } remove { _base.CellStyleChanged -= value; } }
        public event DataGridViewCellStyleContentChangedEventHandler CellStyleContentChanged { add { _base.CellStyleContentChanged += value; } remove { _base.CellStyleContentChanged -= value; } }
        public event DataGridViewCellEventHandler CellToolTipTextChanged { add { _base.CellToolTipTextChanged += value; } remove { _base.CellToolTipTextChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellToolTipTextNeededEventHandler CellToolTipTextNeeded { add { _base.CellToolTipTextNeeded += value; } remove { _base.CellToolTipTextNeeded -= value; } }
        public event DataGridViewCellEventHandler CellValidated { add { _base.CellValidated += value; } remove { _base.CellValidated -= value; } }
        public event DataGridViewCellValidatingEventHandler CellValidating { add { _base.CellValidating += value; } remove { _base.CellValidating -= value; } }
        public event DataGridViewCellEventHandler CellValueChanged { add { _base.CellValueChanged += value; } remove { _base.CellValueChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellValueEventHandler CellValueNeeded { add { _base.CellValueNeeded += value; } remove { _base.CellValueNeeded -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewCellValueEventHandler CellValuePushed { add { _base.CellValuePushed += value; } remove { _base.CellValuePushed -= value; } }
        public event DataGridViewColumnEventHandler ColumnAdded { add { _base.ColumnAdded += value; } remove { _base.ColumnAdded -= value; } }
        public event DataGridViewColumnEventHandler ColumnContextMenuStripChanged { add { _base.ColumnContextMenuStripChanged += value; } remove { _base.ColumnContextMenuStripChanged -= value; } }
        public event DataGridViewColumnEventHandler ColumnDataPropertyNameChanged { add { _base.ColumnDataPropertyNameChanged += value; } remove { _base.ColumnDataPropertyNameChanged -= value; } }
        public event DataGridViewColumnEventHandler ColumnDefaultCellStyleChanged { add { _base.ColumnDefaultCellStyleChanged += value; } remove { _base.ColumnDefaultCellStyleChanged -= value; } }
        public event DataGridViewColumnEventHandler ColumnDisplayIndexChanged { add { _base.ColumnDisplayIndexChanged += value; } remove { _base.ColumnDisplayIndexChanged -= value; } }
        public event DataGridViewColumnDividerDoubleClickEventHandler ColumnDividerDoubleClick { add { _base.ColumnDividerDoubleClick += value; } remove { _base.ColumnDividerDoubleClick -= value; } }
        public event DataGridViewColumnEventHandler ColumnDividerWidthChanged { add { _base.ColumnDividerWidthChanged += value; } remove { _base.ColumnDividerWidthChanged -= value; } }
        public event DataGridViewColumnEventHandler ColumnHeaderCellChanged { add { _base.ColumnHeaderCellChanged += value; } remove { _base.ColumnHeaderCellChanged -= value; } }
        public event DataGridViewCellMouseEventHandler ColumnHeaderMouseClick { add { _base.ColumnHeaderMouseClick += value; } remove { _base.ColumnHeaderMouseClick -= value; } }
        public event DataGridViewCellMouseEventHandler ColumnHeaderMouseDoubleClick { add { _base.ColumnHeaderMouseDoubleClick += value; } remove { _base.ColumnHeaderMouseDoubleClick -= value; } }
        public event EventHandler ColumnHeadersBorderStyleChanged { add { _base.ColumnHeadersBorderStyleChanged += value; } remove { _base.ColumnHeadersBorderStyleChanged -= value; } }
        public event EventHandler ColumnHeadersHeightChanged { add { _base.ColumnHeadersHeightChanged += value; } remove { _base.ColumnHeadersHeightChanged -= value; } }
        public event DataGridViewAutoSizeModeEventHandler ColumnHeadersHeightSizeModeChanged { add { _base.ColumnHeadersHeightSizeModeChanged += value; } remove { _base.ColumnHeadersHeightSizeModeChanged -= value; } }
        public event DataGridViewColumnEventHandler ColumnMinimumWidthChanged { add { _base.ColumnMinimumWidthChanged += value; } remove { _base.ColumnMinimumWidthChanged -= value; } }
        public event DataGridViewColumnEventHandler ColumnNameChanged { add { _base.ColumnNameChanged += value; } remove { _base.ColumnNameChanged -= value; } }
        public event DataGridViewColumnEventHandler ColumnRemoved { add { _base.ColumnRemoved += value; } remove { _base.ColumnRemoved -= value; } }
        public event DataGridViewColumnEventHandler ColumnSortModeChanged { add { _base.ColumnSortModeChanged += value; } remove { _base.ColumnSortModeChanged -= value; } }
        public event DataGridViewColumnStateChangedEventHandler ColumnStateChanged { add { _base.ColumnStateChanged += value; } remove { _base.ColumnStateChanged -= value; } }
        public event DataGridViewColumnEventHandler ColumnToolTipTextChanged { add { _base.ColumnToolTipTextChanged += value; } remove { _base.ColumnToolTipTextChanged -= value; } }
        public event DataGridViewColumnEventHandler ColumnWidthChanged { add { _base.ColumnWidthChanged += value; } remove { _base.ColumnWidthChanged -= value; } }
        public event EventHandler CurrentCellChanged { add { _base.CurrentCellChanged += value; } remove { _base.CurrentCellChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler CurrentCellDirtyStateChanged { add { _base.CurrentCellDirtyStateChanged += value; } remove { _base.CurrentCellDirtyStateChanged -= value; } }
        public event DataGridViewBindingCompleteEventHandler DataBindingComplete { add { _base.DataBindingComplete += value; } remove { _base.DataBindingComplete -= value; } }
        public event DataGridViewDataErrorEventHandler DataError { add { _base.DataError += value; } remove { _base.DataError -= value; } }
        public event EventHandler DataMemberChanged { add { _base.DataMemberChanged += value; } remove { _base.DataMemberChanged -= value; } }
        public event EventHandler DataSourceChanged { add { _base.DataSourceChanged += value; } remove { _base.DataSourceChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowEventHandler DefaultValuesNeeded { add { _base.DefaultValuesNeeded += value; } remove { _base.DefaultValuesNeeded -= value; } }
        public event DataGridViewEditingControlShowingEventHandler EditingControlShowing { add { _base.EditingControlShowing += value; } remove { _base.EditingControlShowing -= value; } }
        public event EventHandler EditModeChanged { add { _base.EditModeChanged += value; } remove { _base.EditModeChanged -= value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event EventHandler FontChanged { add { _base.FontChanged += value; } remove { _base.FontChanged -= value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public new event EventHandler ForeColorChanged { add { _base.ForeColorChanged += value; } remove { _base.ForeColorChanged -= value; } }
        public event EventHandler GridColorChanged { add { _base.GridColorChanged += value; } remove { _base.GridColorChanged -= value; } }
        public event EventHandler MultiSelectChanged { add { _base.MultiSelectChanged += value; } remove { _base.MultiSelectChanged -= value; } }
        public event DataGridViewRowEventHandler NewRowNeeded { add { _base.NewRowNeeded += value; } remove { _base.NewRowNeeded -= value; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler PaddingChanged { add { _base.PaddingChanged += value; } remove { _base.PaddingChanged -= value; } }
        public event EventHandler ReadOnlyChanged { add { _base.ReadOnlyChanged += value; } remove { _base.ReadOnlyChanged -= value; } }
        public event DataGridViewRowEventHandler RowContextMenuStripChanged { add { _base.RowContextMenuStripChanged += value; } remove { _base.RowContextMenuStripChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowContextMenuStripNeededEventHandler RowContextMenuStripNeeded { add { _base.RowContextMenuStripNeeded += value; } remove { _base.RowContextMenuStripNeeded -= value; } }
        public event DataGridViewRowEventHandler RowDefaultCellStyleChanged { add { _base.RowDefaultCellStyleChanged += value; } remove { _base.RowDefaultCellStyleChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event QuestionEventHandler RowDirtyStateNeeded { add { _base.RowDirtyStateNeeded += value; } remove { _base.RowDirtyStateNeeded -= value; } }
        public event DataGridViewRowDividerDoubleClickEventHandler RowDividerDoubleClick { add { _base.RowDividerDoubleClick += value; } remove { _base.RowDividerDoubleClick -= value; } }
        public event DataGridViewRowEventHandler RowDividerHeightChanged { add { _base.RowDividerHeightChanged += value; } remove { _base.RowDividerHeightChanged -= value; } }
        public event DataGridViewCellEventHandler RowEnter { add { _base.RowEnter += value; } remove { _base.RowEnter -= value; } }
        public event DataGridViewRowEventHandler RowErrorTextChanged { add { _base.RowErrorTextChanged += value; } remove { _base.RowErrorTextChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowErrorTextNeededEventHandler RowErrorTextNeeded { add { _base.RowErrorTextNeeded += value; } remove { _base.RowErrorTextNeeded -= value; } }
        public event DataGridViewRowEventHandler RowHeaderCellChanged { add { _base.RowHeaderCellChanged += value; } remove { _base.RowHeaderCellChanged -= value; } }
        public event DataGridViewCellMouseEventHandler RowHeaderMouseClick { add { _base.RowHeaderMouseClick += value; } remove { _base.RowHeaderMouseClick -= value; } }
        public event DataGridViewCellMouseEventHandler RowHeaderMouseDoubleClick { add { _base.RowHeaderMouseDoubleClick += value; } remove { _base.RowHeaderMouseDoubleClick -= value; } }
        public event EventHandler RowHeadersBorderStyleChanged { add { _base.RowHeadersBorderStyleChanged += value; } remove { _base.RowHeadersBorderStyleChanged -= value; } }
        public event EventHandler RowHeadersWidthChanged { add { _base.RowHeadersWidthChanged += value; } remove { _base.RowHeadersWidthChanged -= value; } }
        public event DataGridViewAutoSizeModeEventHandler RowHeadersWidthSizeModeChanged { add { _base.RowHeadersWidthSizeModeChanged += value; } remove { _base.RowHeadersWidthSizeModeChanged -= value; } }
        public event DataGridViewRowEventHandler RowHeightChanged { add { _base.RowHeightChanged += value; } remove { _base.RowHeightChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowHeightInfoNeededEventHandler RowHeightInfoNeeded { add { _base.RowHeightInfoNeeded += value; } remove { _base.RowHeightInfoNeeded -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowHeightInfoPushedEventHandler RowHeightInfoPushed { add { _base.RowHeightInfoPushed += value; } remove { _base.RowHeightInfoPushed -= value; } }
        public event DataGridViewCellEventHandler RowLeave { add { _base.RowLeave += value; } remove { _base.RowLeave -= value; } }
        public event DataGridViewRowEventHandler RowMinimumHeightChanged { add { _base.RowMinimumHeightChanged += value; } remove { _base.RowMinimumHeightChanged -= value; } }
        public event DataGridViewRowPostPaintEventHandler RowPostPaint { add { _base.RowPostPaint += value; } remove { _base.RowPostPaint -= value; } }
        public event DataGridViewRowPrePaintEventHandler RowPrePaint { add { _base.RowPrePaint += value; } remove { _base.RowPrePaint -= value; } }
        public event DataGridViewRowsAddedEventHandler RowsAdded { add { _base.RowsAdded += value; } remove { _base.RowsAdded -= value; } }
        public event EventHandler RowsDefaultCellStyleChanged { add { _base.RowsDefaultCellStyleChanged += value; } remove { _base.RowsDefaultCellStyleChanged -= value; } }
        public event DataGridViewRowsRemovedEventHandler RowsRemoved { add { _base.RowsRemoved += value; } remove { _base.RowsRemoved -= value; } }
        public event DataGridViewRowStateChangedEventHandler RowStateChanged { add { _base.RowStateChanged += value; } remove { _base.RowStateChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewRowEventHandler RowUnshared { add { _base.RowUnshared += value; } remove { _base.RowUnshared -= value; } }
        public event DataGridViewCellEventHandler RowValidated { add { _base.RowValidated += value; } remove { _base.RowValidated -= value; } }
        public event DataGridViewCellCancelEventHandler RowValidating { add { _base.RowValidating += value; } remove { _base.RowValidating -= value; } }
        public new event ScrollEventHandler Scroll { add { _base.Scroll += value; } remove { _base.Scroll -= value; } }
        public event EventHandler SelectionChanged { add { _base.SelectionChanged += value; } remove { _base.SelectionChanged -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event DataGridViewSortCompareEventHandler SortCompare { add { _base.SortCompare += value; } remove { _base.SortCompare -= value; } }
        public event EventHandler Sorted { add { _base.Sorted += value; } remove { _base.Sorted -= value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler StyleChanged { add { _base.StyleChanged += value; } remove { _base.StyleChanged -= value; } }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler TextChanged { add { _base.TextChanged += value; } remove { _base.TextChanged -= value; } }
        public event DataGridViewRowEventHandler UserAddedRow { add { _base.UserAddedRow += value; } remove { _base.UserAddedRow -= value; } }
        public event DataGridViewRowEventHandler UserDeletedRow { add { _base.UserDeletedRow += value; } remove { _base.UserDeletedRow -= value; } }
        public event DataGridViewRowCancelEventHandler UserDeletingRow { add { _base.UserDeletingRow += value; } remove { _base.UserDeletingRow -= value; } }
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public DataGridViewAdvancedBorderStyle AdjustColumnHeaderBorderStyle(DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput, DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder, bool isFirstDisplayedColumn, bool isLastVisibleColumn) { return _base.AdjustColumnHeaderBorderStyle(dataGridViewAdvancedBorderStyleInput, dataGridViewAdvancedBorderStylePlaceholder, isFirstDisplayedColumn, isLastVisibleColumn); }
        public bool AreAllCellsSelected(bool includeInvisibleCells) { return _base.AreAllCellsSelected(includeInvisibleCells); }
        public void AutoResizeColumn(int columnIndex) { _base.AutoResizeColumn(columnIndex); }
        public void AutoResizeColumn(int columnIndex, DataGridViewAutoSizeColumnMode autoSizeColumnMode) { _base.AutoResizeColumn(columnIndex, autoSizeColumnMode); }
        public void AutoResizeColumnHeadersHeight() { _base.AutoResizeColumnHeadersHeight(); }
        public void AutoResizeColumnHeadersHeight(int columnIndex) { _base.AutoResizeColumnHeadersHeight(columnIndex); }
        public void AutoResizeColumns() { _base.AutoResizeColumns(); }
        public void AutoResizeColumns(DataGridViewAutoSizeColumnsMode autoSizeColumnsMode) { _base.AutoResizeColumns(autoSizeColumnsMode); }
        public void AutoResizeRow(int rowIndex) { _base.AutoResizeRow(rowIndex); }
        public void AutoResizeRow(int rowIndex, DataGridViewAutoSizeRowMode autoSizeRowMode) { _base.AutoResizeRow(rowIndex, autoSizeRowMode); }
        public void AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode rowHeadersWidthSizeMode) { _base.AutoResizeRowHeadersWidth(rowHeadersWidthSizeMode); }
        public void AutoResizeRowHeadersWidth(int rowIndex, DataGridViewRowHeadersWidthSizeMode rowHeadersWidthSizeMode) { _base.AutoResizeRowHeadersWidth(rowIndex, rowHeadersWidthSizeMode); }
        public void AutoResizeRows() { _base.AutoResizeRows(); }
        public void AutoResizeRows(DataGridViewAutoSizeRowsMode autoSizeRowsMode) { _base.AutoResizeRows(autoSizeRowsMode); }
        public bool BeginEdit(bool selectAll) { return _base.BeginEdit(selectAll); }
        public bool CancelEdit() { return _base.CancelEdit(); }
        public void ClearSelection() { _base.ClearSelection(); }
        public bool CommitEdit(DataGridViewDataErrorContexts context) { return _base.CommitEdit(context); }
        public int DisplayedColumnCount(bool includePartialColumns) { return _base.DisplayedColumnCount(includePartialColumns); }
        public int DisplayedRowCount(bool includePartialRow) { return _base.DisplayedRowCount(includePartialRow); }
        public bool EndEdit() { return _base.EndEdit(); }
        public bool EndEdit(DataGridViewDataErrorContexts context) { return _base.EndEdit(context); }
        public int GetCellCount(DataGridViewElementStates includeFilter) { return _base.GetCellCount(includeFilter); }
        public Rectangle GetCellDisplayRectangle(int columnIndex, int rowIndex, bool cutOverflow) { return _base.GetCellDisplayRectangle(columnIndex, rowIndex, cutOverflow); }
        public DataObject GetClipboardContent() { return _base.GetClipboardContent(); }
        public Rectangle GetColumnDisplayRectangle(int columnIndex, bool cutOverflow) { return _base.GetColumnDisplayRectangle(columnIndex, cutOverflow); }
        public Rectangle GetRowDisplayRectangle(int rowIndex, bool cutOverflow) { return _base.GetRowDisplayRectangle(rowIndex, cutOverflow); }
        public HitTestInfo HitTest(int x, int y) { return _base.HitTest(x, y); }
        public HitTestInfo HitTest(Point p) { return _base.HitTest(p.X, p.Y); }
        public void InvalidateCell(DataGridViewCell dataGridViewCell) { _base.InvalidateCell(dataGridViewCell); }
        public void InvalidateCell(int columnIndex, int rowIndex) { _base.InvalidateCell(columnIndex, rowIndex); }
        public void InvalidateColumn(int columnIndex) { _base.InvalidateColumn(columnIndex); }
        public void InvalidateRow(int rowIndex) { _base.InvalidateRow(rowIndex); }
        public void NotifyCurrentCellDirty(bool dirty) { _base.NotifyCurrentCellDirty(dirty); }
        public bool RefreshEdit() { return _base.RefreshEdit(); }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new void ResetText() { _base.ResetText(); }
        public void SelectAll() { _base.SelectAll(); }
        public void Sort(IComparer comparer) { _base.Sort(comparer); }
        public void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction) { _base.Sort(dataGridViewColumn, direction); }
        public void UpdateCellErrorText(int columnIndex, int rowIndex) { _base.UpdateCellErrorText(columnIndex, rowIndex); }
        public void UpdateCellValue(int columnIndex, int rowIndex) { _base.UpdateCellValue(columnIndex, rowIndex); }
        public void UpdateRowErrorText(int rowIndex) { _base.UpdateRowErrorText(rowIndex); }
        public void UpdateRowErrorText(int rowIndexStart, int rowIndexEnd) { _base.UpdateRowErrorText(rowIndexStart, rowIndexEnd); }
        public void UpdateRowHeightInfo(int rowIndex, bool updateToEnd) { _base.UpdateRowHeightInfo(rowIndex, updateToEnd); }

        // Based on insanely helpful (any very hard to find!) online information about how to make the "Columns" property editable...
        // https://stackoverflow.com/questions/36787383/expose-columns-property-of-a-datagridview-in-usercontrol-and-make-it-editable-vi/36794920#36794920
        // https://web.archive.org/web/20100217122305/http://www.developersdex.com:80/vb/message.asp?p=1120&r=5501708
        // https://www.pcreview.co.uk/threads/user-control-instance-datagridview-columns-in-visual-studio-design.2893613/
        // https://stackoverflow.com/questions/35285198/exposing-datagridviews-columns-property-in-usercontrol-doesnt-work-properly
        private class ColumnEditor : UITypeEditor
        {
            private class TypeDescriptionContext : ITypeDescriptorContext
            {
                private Control editingObject;
                private PropertyDescriptor editingProperty;
                public TypeDescriptionContext(Control obj, PropertyDescriptor property)
                {
                    editingObject = obj;
                    editingProperty = property;
                }
                public IContainer Container
                {
                    get { return editingObject.Container; }
                }
                public object Instance
                {
                    get { return editingObject; }
                }
                public void OnComponentChanged()
                { }
                public bool OnComponentChanging()
                {
                    return true;
                }
                public PropertyDescriptor PropertyDescriptor
                {
                    get { return editingProperty; }
                }
                public object GetService(Type serviceType)
                {
                    return editingObject.Site.GetService(serviceType);
                }
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context,
                                             IServiceProvider provider, object value)
            {
                var field = context.Instance.GetType().GetField("_base", BindingFlags.NonPublic | BindingFlags.Instance);
                var _base = (DataGridView)field.GetValue(context.Instance);
                _base.Site = ((Control)context.Instance).Site;
                var columnsProperty = TypeDescriptor.GetProperties(_base)["Columns"];
                var tdc = new TypeDescriptionContext(_base, columnsProperty);
                var editor = (UITypeEditor)columnsProperty.GetEditor(typeof(UITypeEditor));
                var result = editor.EditValue(tdc, provider, value);
                _base.Site = null;
                return result;
            }
        }
    }

    public class DarkDataGridViewButtonCell : DataGridViewButtonCell
    {
        // Unfortunately we need access to a private data member
        private static readonly PropertyInfo _buttonState = typeof(DataGridViewButtonCell).GetProperty("ButtonState", BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly Padding _padding = new Padding(1, 1, 2, 2);
        private static readonly StringFormat _stringFormat = new StringFormat
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter
        };

        private int? _mouseCurserCell;
        private bool? _enabled;

        [Browsable(false)]
        public ButtonState ButtonState => (ButtonState)_buttonState.GetValue(this, null);

        [DefaultValue(false)]
        public bool Enabled
        {
            get
            {
                return _enabled ?? (OwningColumn as DarkDataGridViewButtonColumn)?.Enabled ?? true;
            }
            set
            {
                if (value != Enabled)
                    _enabled = value;
            }
        }


        protected override void OnMouseEnter(int rowIndex)
        {
            base.OnMouseEnter(rowIndex);

            int? previous_mousePositionCellCell = _mouseCurserCell;
            _mouseCurserCell = rowIndex;

            // Update
            if (previous_mousePositionCellCell.HasValue)
            {
                DataGridView.InvalidateCell(ColumnIndex, previous_mousePositionCellCell.Value);
                DataGridView.InvalidateCell(ColumnIndex, RowIndex);
            }
            DataGridView.InvalidateCell(ColumnIndex, RowIndex);
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);

            _mouseCurserCell = null;
            DataGridView.InvalidateCell(ColumnIndex, RowIndex);
        }

        protected override void OnKeyDown(KeyEventArgs e, int rowIndex)
        {
            if (Enabled)
                base.OnKeyDown(e, rowIndex);
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (Enabled)
                base.OnMouseDown(e);
        }


        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (FlatStyle != FlatStyle.Flat)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value,
                    formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                return;
            }

            // Paint background
            if (paintParts.HasFlag(DataGridViewPaintParts.Background))
            {
                Color backColor = elementState.HasFlag(DataGridViewElementStates.Selected) ?
                cellStyle.SelectionBackColor : cellStyle.BackColor;
                using (var brush = new SolidBrush(backColor))
                    graphics.FillRectangle(brush, cellBounds);
            }

            if (paintParts.HasFlag(DataGridViewPaintParts.Border))
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);

            // Choose button colors
            Color textColor = cellStyle.ForeColor;
            Color borderColor = Colors.GreySelection;
            Color fillColor = Colors.GreyBackground;

            if (DataGridView.Focused && (DataGridView.CurrentCellAddress == new Point(ColumnIndex, rowIndex)))
                borderColor = Colors.BlueHighlight; //Selection

            if (ButtonState.HasFlag(ButtonState.Inactive) || !Enabled)
            {
                fillColor = Colors.DarkGreySelection;
                textColor = Colors.DisabledText;
            }
            else if (ButtonState.HasFlag(ButtonState.Checked) || ButtonState.HasFlag(ButtonState.Pushed))
                fillColor = Colors.DarkBackground;
            else if (_mouseCurserCell == rowIndex) // Hover
                fillColor = Colors.LighterBackground;

            // Paint button
            Rectangle contentBounds = new Rectangle(cellBounds.X + _padding.Left, cellBounds.Y + _padding.Top,
                cellBounds.Width - _padding.Horizontal, cellBounds.Height - _padding.Vertical);

            if (paintParts.HasFlag(DataGridViewPaintParts.ContentBackground))
            {
                using (var brush = new SolidBrush(fillColor))
                    graphics.FillRectangle(brush, contentBounds);

                using (var pen = new Pen(borderColor, 1))
                    graphics.DrawRectangle(pen, contentBounds.Left, contentBounds.Top, contentBounds.Width - 1, contentBounds.Height - 1);
            }

            if (paintParts.HasFlag(DataGridViewPaintParts.ContentForeground))
                using (var brush = new SolidBrush(textColor))
                    graphics.DrawString(formattedValue?.ToString() ?? "", cellStyle.Font, brush, contentBounds, _stringFormat);

            // Paint error
            if (DataGridView.ShowCellErrors && paintParts.HasFlag(DataGridViewPaintParts.ErrorIcon))
                base.PaintErrorIcon(graphics, clipBounds, contentBounds, errorText);
        }
    }

    public class DarkDataGridViewButtonColumn : DataGridViewButtonColumn
    {
        public DarkDataGridViewButtonColumn()
        {
            base.CellTemplate = new DarkDataGridViewButtonCell();
            base.UseColumnTextForButtonValue = true;
            base.FlatStyle = FlatStyle.Flat;
        }

        [DefaultValue(FlatStyle.Flat)]
        public new FlatStyle FlatStyle
        {
            get { return FlatStyle.Flat; }
        }

        [DefaultValue(true)]
        public new bool UseColumnTextForButtonValue
        {
            get { return base.UseColumnTextForButtonValue; }
            set { base.UseColumnTextForButtonValue = value; }
        }

        [DefaultValue(true)]
        public bool Enabled { get; set; } = true;
    }

    public class DarkDataGridViewCheckBoxCell : DataGridViewCheckBoxCell
    {}

    public class DarkDataGridViewCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        public DarkDataGridViewCheckBoxColumn()
        {
            base.FlatStyle = FlatStyle.Flat;
        }

        [DefaultValue(FlatStyle.Flat)]
        public new FlatStyle FlatStyle
        {
            get { return FlatStyle.Flat; }
        }
    }
}

using System.Data;

namespace data_grid_view_colors_for_ok_no
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dataGridView.AllowUserToAddRows = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.DataSource = DataTable;
            dataGridView.Columns[nameof(Record.Description)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView.Columns[nameof(Record.ConditionA)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.Columns[nameof(Record.ConditionB)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Subscribe to the event inline using lambda.
            dataGridView.CellPainting += (sender, e) =>
            {
                if ((e.RowIndex != -1) && (e.ColumnIndex != -1))
                {
                    if (Enum.TryParse(e.Value?.ToString() ?? string.Empty, out ConditionState state))
                    {
                        switch (state)
                        {
                            case ConditionState.NO: dataGridView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightSalmon; break;
                            case ConditionState.OK: dataGridView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightGreen; break;
                        }
                    }
                }
            };
        }
        DataTable DataTable { get; } = new List<Record>
        {
            new Record{Description = "Assembly", ConditionA = ConditionState.OK, ConditionB = ConditionState.NO },
            new Record{Description = "Triangle", ConditionA = ConditionState.OK, ConditionB = ConditionState.NO },
            new Record{Description = "Wing", ConditionA = ConditionState.NO, ConditionB = ConditionState.OK },
            new Record{Description = "Radio", ConditionA = ConditionState.OK, ConditionB = ConditionState.OK },
            new Record{Description = "Marketing", ConditionA = ConditionState.NO, ConditionB = ConditionState.NO },
            new Record{Description = "Hospitality", ConditionA = ConditionState.OK, ConditionB = ConditionState.NO },
            new Record{Description = "Curriculum", ConditionA = ConditionState.NO, ConditionB = ConditionState.OK },
            new Record{Description = "Theme", ConditionA = ConditionState.OK, ConditionB = ConditionState.OK },
            new Record{Description = "Economy", ConditionA = ConditionState.NO, ConditionB = ConditionState.NO },
            new Record{Description = "Consumer", ConditionA = ConditionState.OK, ConditionB = ConditionState.NO }
        }.ToDataTable();
    }

    enum ConditionState{ NO, OK, }
    class Record
    {
        public string Description { get; set; } = string.Empty;
        public ConditionState ConditionA { get; set; }
        public ConditionState ConditionB { get; set; }
    }

    static partial class Extensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            DataTable table = new DataTable();
            Type type = typeof(T);

            string[] names =
                type
                .GetProperties().Where(_ => _.CanWrite || _.CanRead)
                .Select(_ => _.Name)
                .ToArray();

            if (names?.Any() == true)
            {
                foreach (var name in names)     // Add columns 
                {
                    table.Columns.Add(name, type.GetProperty(name).PropertyType);
                }
                foreach (var item in data)      // Add rows 
                {
                    object[] values =
                        names
                        .Select(name => type.GetProperty(name).GetValue(item))
                        .ToArray();
                    table.Rows.Add(values);
                }
            }
            return table;
        }
    }
}

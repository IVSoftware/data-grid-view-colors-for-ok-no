## DataGridView colors for OK and NO values

The correct event to use in this case is `DataGridView.CellPainting`.

[![screenshot with colored backgrounds][1]][1]

One concise way to both ensure that the event is really listening and handle it is to attach it inline using a lambda expression as shown in the `MainForm.Load` event handler.

```csharp
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
```
___
**Where**

```
enum ConditionState{ NO, OK, }
```

  [1]: https://i.stack.imgur.com/0BtyW.png
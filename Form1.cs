using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsCalculator;

public partial class Form1 : Form
{
    private TextBox displayTextBox = null!;
    private double resultValue = 0;
    private string operationPerformed = "";
    private bool isOperationPerformed = false;

    public Form1()
    {
        InitializeComponent();
        SetupCalculatorUI();
    }

    private void SetupCalculatorUI()
    {
        this.Text = "Calculator";
        this.Size = new Size(300, 420);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        displayTextBox = new TextBox();
        displayTextBox.Location = new Point(10, 10);
        displayTextBox.Size = new Size(260, 40);
        displayTextBox.Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
        displayTextBox.TextAlign = HorizontalAlignment.Right;
        displayTextBox.ReadOnly = true;
        displayTextBox.Text = "0";
        this.Controls.Add(displayTextBox);

        string[] buttonLabels = new string[]
        {
            "7", "8", "9", "/",
            "4", "5", "6", "*",
            "1", "2", "3", "-",
            "0", ".", "C", "+",
            "="
        };

        int startX = 10;
        int startY = 70;
        int btnWidth = 60;
        int btnHeight = 50;
        int padding = 5;

        int x = startX;
        int y = startY;

        for (int i = 0; i < buttonLabels.Length; i++)
        {
            string label = buttonLabels[i];
            Button btn = new Button();
            btn.Text = label;
            btn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);

            if (label == "=")
            {
                btn.Size = new Size(btnWidth * 4 + padding * 3, btnHeight);
                btn.Location = new Point(startX, y);
            }
            else
            {
                btn.Size = new Size(btnWidth, btnHeight);
                btn.Location = new Point(x, y);
            }

            btn.Click += Button_Click;
            this.Controls.Add(btn);

            if (label != "=")
            {
                x += btnWidth + padding;
                if ((i + 1) % 4 == 0)
                {
                    x = startX;
                    y += btnHeight + padding;
                }
            }
        }
    }

    private void Button_Click(object? sender, EventArgs e)
    {
        if (sender is not Button btn) return;
        string text = btn.Text;

        if (text == "C")
        {
            displayTextBox.Text = "0";
            resultValue = 0;
            operationPerformed = "";
            return;
        }

        if (text == "+" || text == "-" || text == "*" || text == "/")
        {
            if (operationPerformed != "")
            {
                CalculateResult();
            }
            else
            {
                if (double.TryParse(displayTextBox.Text, out double num))
                {
                    resultValue = num;
                }
            }
            operationPerformed = text;
            isOperationPerformed = true;
            return;
        }

        if (text == "=")
        {
            CalculateResult();
            operationPerformed = "";
            return;
        }

        // Numbers and decimal point
        if (displayTextBox.Text == "0" || isOperationPerformed)
        {
            displayTextBox.Text = "";
        }

        isOperationPerformed = false;

        if (text == ".")
        {
            if (!displayTextBox.Text.Contains("."))
            {
                if (displayTextBox.Text == "") displayTextBox.Text = "0";
                displayTextBox.Text += ".";
            }
        }
        else
        {
            displayTextBox.Text += text;
        }
    }

    private void CalculateResult()
    {
        if (!double.TryParse(displayTextBox.Text, out double currentValue)) return;

        switch (operationPerformed)
        {
            case "+":
                displayTextBox.Text = (resultValue + currentValue).ToString();
                break;
            case "-":
                displayTextBox.Text = (resultValue - currentValue).ToString();
                break;
            case "*":
                displayTextBox.Text = (resultValue * currentValue).ToString();
                break;
            case "/":
                if (currentValue != 0)
                    displayTextBox.Text = (resultValue / currentValue).ToString();
                else
                    displayTextBox.Text = "Error";
                break;
        }
        
        if (displayTextBox.Text != "Error" && double.TryParse(displayTextBox.Text, out double numResult))
        {
            resultValue = numResult;
        }
        else
        {
            resultValue = 0;
        }
            
        isOperationPerformed = true;
    }
}

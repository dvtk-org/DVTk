// ------------------------------------------------------
// DVTk - The Healthcare Validation Toolkit (www.dvtk.org)
// Copyright � 2009 DVTk
// ------------------------------------------------------
// This file is part of DVTk.
//
// DVTk is free software; you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License as published by the Free Software Foundation; either version 3.0
// of the License, or (at your option) any later version. 
// 
// DVTk is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even
// the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser
// General Public License for more details. 
// 
// You should have received a copy of the GNU Lesser General Public License along with this
// library; if not, see <http://www.gnu.org/licenses/>



using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;



namespace DvtkApplicationLayer.UserInterfaces
{
    /// <summary>
    /// InputForm is a Form which contains entries (like e.g. TextBox) that are added by using method calls
    /// of this class. The appearance of this Form is then determined dynamically.
    /// </summary>
    public class InputForm : System.Windows.Forms.Form
    {

        //
        // - Generated by Visual Studio -
        //

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // InputForm
            // 
            this.AutoScaleMode = AutoScaleMode.None;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(440, 398);
            this.KeyPreview = true;
            this.Name = "InputForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "InputForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputForm_KeyDown);
            this.Load += new System.EventHandler(this.InputForm_Load);
            this.VisibleChanged += new System.EventHandler(this.InputForm_VisibleChanged);

        }
        #endregion



        //
        // - Fields -
        //

        private int leftMargin = 16;
        private int rightMargin = 16;
        private int topMargin = 16;
        private int bottomMargin = 16;
        private int verticalSpaceBetweenControls = 8;
        private int horizontalSpaceBetweenControls = 16;
        private int verticalSizeEntryControls = 23;
        private ArrayList entries = new ArrayList();
        private int EntryDescriptionWidth = 150;
        private int EntryValueWidth = 450;
        private String description = "";
        private int descriptionWidth = 616;
        private String caption = "";
        private ArrayList images = new ArrayList();
        private StringCollection textOnButtons = new StringCollection();
        private ArrayList buttonControls = null;
        private String textOnButtonPressed = "";



        //
        // - Constructors -
        //

        /// <summary>
        /// Default constructor.
        /// </summary>
        public InputForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.textOnButtons.Add("OK");
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }



        //
        // - Properties -
        //

        /// <summary>
        /// Set the caption of the form.
        /// </summary>
        public String Caption
        {
            set
            {
                this.caption = value;
            }
        }

        /// <summary>
        /// UNDER DEVELOPMENT!
        /// </summary>
        public String Description
        {
            get
            {
                return (this.description);
            }
            set
            {
                this.description = value;
            }
        }



        //
        // - Methods -
        //

        /// <summary>
        /// Add an entry consisting of a desciption and a DropDownList.
        /// 
        /// The DropDownList will use the ToString method of the supplied values to display the
        /// items.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="values">The values.</param>
        /// <param name="defaultValue">The default value.</param>
        public void AddDropDownListEntry(String description, ArrayList values, Object defaultValue)
        {
            ArrayList valuesAsText = new ArrayList();

            foreach (Object theObject in values)
            {
                valuesAsText.Add(theObject.ToString());
            }

            AddDropDownListEntry(description, values, valuesAsText, defaultValue);
        }

        /// <summary>
        /// Add an entry consisting of a desciption and a DropDownList.
        /// 
        /// The DropDownList will use supplied textValues to display the items with.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="values">The values.</param>
        /// <param name="textValues">The text values.</param>
        /// <param name="defaultValue">The default value.</param>
        public void AddDropDownListEntry(String description, IList values, IList textValues, Object defaultValue)
        {
            if (values.Count != textValues.Count)
            {
                throw new Exception("Two supplied arraylists to method AddDropDownListEntry have different Counts.");
            }

            InputFormDropDownListEntry inputFormDropDownListEntry = new InputFormDropDownListEntry(description, values, textValues, defaultValue);
            entries.Add(inputFormDropDownListEntry);
        }

        /// <summary>
        /// Add an image to the form.
        /// 
        /// FOR NOW: only one image maximum will be displayed.
        /// </summary>
        /// <param name="image"></param>
        public void AddImage(Image image)
        {
            this.images.Add(image);
        }

        /// <summary>
        /// Add an entry consisting of a desciption and a TextBox.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="defaultValue"></param>
        /// <param name="readOnly"></param>
        public void AddTextBoxEntry(String description, String defaultValue, bool readOnly)
        {
            InputFormTextBoxEntry inputFormTextBoxEntry = new InputFormTextBoxEntry(description, defaultValue, readOnly);
            entries.Add(inputFormTextBoxEntry);
        }

        /// <summary>
        /// Called when the OK button is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            foreach (InputFormEntry inputFormEntry in this.entries)
            {
                if (inputFormEntry is InputFormTextBoxEntry)
                {
                    InputFormTextBoxEntry inputFormTextBoxEntry = inputFormEntry as InputFormTextBoxEntry;

                    inputFormTextBoxEntry.Value = inputFormTextBoxEntry.TextBox.Text;
                    inputFormTextBoxEntry.TextValue = inputFormTextBoxEntry.TextBox.Text;
                }
                else if (inputFormEntry is InputFormDropDownListEntry)
                {
                    InputFormDropDownListEntry inputFormDropDownListEntry = inputFormEntry as InputFormDropDownListEntry;

                    Object selectedItem = inputFormDropDownListEntry.ComboBox.SelectedItem;

                    if (selectedItem == null)
                    {
                        inputFormDropDownListEntry.Value = null;
                        inputFormDropDownListEntry.TextValue = "";
                    }
                    else
                    {
                        inputFormDropDownListEntry.Value = (selectedItem as InputFormValueAndRepresentation).Value;
                        inputFormDropDownListEntry.TextValue = (selectedItem as InputFormValueAndRepresentation).ValueRepresentation;
                    }
                }
                else
                {
                    // Do nothing.
                }
            }

            this.textOnButtonPressed = (sender as Control).Name;

            Close();
        }

        /// <summary>
        /// Get the text values of all entries for debugging purposes.
        /// </summary>
        /// <returns></returns>
        public String DumpTextValues()
        {
            String returnValues = "";

            foreach (InputFormEntry inputFormEntry in this.entries)
            {
                String theValue = "\"" + inputFormEntry.TextValue + "\"";

                returnValues += inputFormEntry.Description + ": " + theValue + "\r\n";
            }

            return (returnValues);
        }

        /// <summary>
        /// Makes that that when the return is pressed, the form is closed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void InputForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
            {
                (this.buttonControls[0] as Button).PerformClick();
            }
        }

        /// <summary>
        /// Put the controls on the Form.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        private void InputForm_Load(object sender, System.EventArgs e)
        {
            int nextYPosition = this.topMargin;
            int newClientWidth = this.leftMargin + this.EntryDescriptionWidth + this.horizontalSpaceBetweenControls + this.EntryValueWidth + this.rightMargin;

            Text = this.caption;

            // Add the description if it is set.
            if (this.description.Length > 0)
            {
                TextBox descriptionTextBox = new TextBox();
                this.description = this.description.Replace("\n\r", "\n");
                this.description = this.description.Replace("\r\n", "\n");
                this.description = this.description.Replace("\r", "\n");
                String[] lines = this.description.Split(new Char[] { '\n' });
                descriptionTextBox.Lines = lines;
                descriptionTextBox.Multiline = true;
                descriptionTextBox.ReadOnly = true;

                int numberOfLinesDisplayed = Math.Min(4, lines.Length) + 1;
                int descriptionHeight = (numberOfLinesDisplayed * (descriptionTextBox.Font.Height + 2));
                descriptionTextBox.ClientSize = new Size(this.descriptionWidth, descriptionHeight);
                descriptionTextBox.Location = new Point(this.leftMargin, this.topMargin);

                if (lines.Length > numberOfLinesDisplayed)
                {
                    descriptionTextBox.ScrollBars = ScrollBars.Both;
                }
                else
                {
                    descriptionTextBox.ScrollBars = ScrollBars.Horizontal;
                }

                nextYPosition += descriptionHeight + this.verticalSpaceBetweenControls + verticalSpaceBetweenControls;

                descriptionTextBox.TabIndex = 2;
                Controls.Add(descriptionTextBox);
            }

            // Add the image if it is added.
            // ONLY SUPPORT ONE IMAGE FOR NOW.
            if (this.images.Count > 0)
            {
                nextYPosition += this.verticalSpaceBetweenControls;
                Image image = this.images[0] as Image;

                PictureBox pictureBox = new PictureBox();
                pictureBox.Location = new Point((newClientWidth - image.Size.Width) / 2, nextYPosition);
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Image = image;
                pictureBox.BorderStyle = BorderStyle.Fixed3D;
                Controls.Add(pictureBox);

                nextYPosition += pictureBox.Size.Height + (this.verticalSpaceBetweenControls * 2);
            }

            // Add all the entries.
            foreach (InputFormEntry inputFormEntry in this.entries)
            {
                // Add the description label of the entry.
                Label label = new Label();
                label.Text = inputFormEntry.Description;
                label.Size = new Size(EntryDescriptionWidth, verticalSizeEntryControls);
                label.Location = new Point(this.leftMargin, nextYPosition);
                this.Controls.Add(label);

                // Add the value control of the entry.
                Size sizeValueControl = new Size(EntryValueWidth, this.verticalSizeEntryControls);
                Point locationValueControl = new Point(this.leftMargin + this.EntryDescriptionWidth + this.horizontalSpaceBetweenControls, nextYPosition);

                if (inputFormEntry is InputFormTextBoxEntry)
                {
                    // Add the textbox.
                    InputFormTextBoxEntry inputFormTextBoxEntry = inputFormEntry as InputFormTextBoxEntry;

                    TextBox textBox = new TextBox();
                    textBox.Text = inputFormTextBoxEntry.TextValue;
                    textBox.ReadOnly = inputFormTextBoxEntry.ReadOnly;
                    textBox.Size = sizeValueControl;
                    textBox.Location = locationValueControl;
                    inputFormTextBoxEntry.TextBox = textBox;
                    this.Controls.Add(textBox);
                }
                else if (inputFormEntry is InputFormDropDownListEntry)
                {
                    // Add the combobox with style DropDownList.
                    InputFormDropDownListEntry inputFormDropDownListEntry = inputFormEntry as InputFormDropDownListEntry;

                    ArrayList theDropDownListItems = new ArrayList();
                    InputFormValueAndRepresentation itemToSelect = null;

                    for (int index = 0; index < inputFormDropDownListEntry.Values.Count; index++)
                    {
                        InputFormValueAndRepresentation newItem = new InputFormValueAndRepresentation(inputFormDropDownListEntry.Values[index], inputFormDropDownListEntry.TextValues[index] as String);

                        if (newItem.Value == inputFormDropDownListEntry.Value)
                        {
                            itemToSelect = newItem;
                        }

                        theDropDownListItems.Add(newItem);
                    }

                    ComboBox comboBox = new ComboBox();

                    comboBox.Items.AddRange(theDropDownListItems.ToArray());

                    if (itemToSelect != null)
                    {
                        comboBox.SelectedItem = itemToSelect;
                    }
                    comboBox.Size = sizeValueControl;
                    comboBox.Location = locationValueControl;
                    comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                    inputFormDropDownListEntry.ComboBox = comboBox;
                    this.Controls.Add(comboBox);
                }
                else
                {
                    // Do nothing.
                }

                nextYPosition += this.verticalSizeEntryControls + this.verticalSpaceBetweenControls;
            }


            // Add the buttons
            this.buttonControls = new ArrayList();

            Graphics graphics = CreateGraphics();

            int rightSideButtonXPosition = this.leftMargin + this.EntryDescriptionWidth + this.horizontalSpaceBetweenControls + this.EntryValueWidth;

            for (int index = 0; index < this.textOnButtons.Count; index++)
            {
                Button button = new Button();

                String textOnButton = this.textOnButtons[index];

                int minimumWidthNeededForButton = graphics.MeasureString(textOnButton, button.Font).ToSize().Width;

                int buttonWidth = Math.Max(75, minimumWidthNeededForButton + 16);

                button.Location = new Point(rightSideButtonXPosition - buttonWidth, nextYPosition);
                rightSideButtonXPosition -= (buttonWidth + this.horizontalSpaceBetweenControls);
                button.Name = textOnButton;
                button.ClientSize = new System.Drawing.Size(buttonWidth, 22);
                button.Text = textOnButton;
                button.Click += new System.EventHandler(this.buttonOK_Click);
                button.TabIndex = index + 1;
                this.Controls.Add(button);
                this.buttonControls.Add(button);

                if (index == 0)
                {
                    button.Focus();
                }
            }

            graphics.Dispose();


            // Adjust the minimum and maximum size of the form.
            int newClientHeight = nextYPosition + 22 + this.bottomMargin;
            ClientSize = new Size(newClientWidth, newClientHeight);
            this.MinimumSize = Size;
            this.MaximumSize = Size;

        }

        /// <summary>
        /// Get the value of an entry as Object.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The Object.</returns>
        public Object GetValue(System.String description)
        {
            InputFormEntry inputFormEntryToFind = null;

            foreach (InputFormEntry inputFormEntry in this.entries)
            {
                if (inputFormEntry.Description == description)
                {
                    inputFormEntryToFind = inputFormEntry;
                    break;
                }
            }

            if (inputFormEntryToFind == null)
            {
                return (null);
            }
            else
            {
                return (inputFormEntryToFind.Value);
            }
        }

        /// <summary>
        /// Get the text value of an entry.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The text value.</returns>
        public String GetTextValue(System.String description)
        {
            InputFormEntry inputFormEntryToFind = null;

            foreach (InputFormEntry inputFormEntry in this.entries)
            {
                if (inputFormEntry.Description == description)
                {
                    inputFormEntryToFind = inputFormEntry;
                    break;
                }
            }

            if (inputFormEntryToFind == null)
            {
                return ("");
            }
            else
            {
                return (inputFormEntryToFind.TextValue);
            }
        }

        private void InputForm_VisibleChanged(object sender, System.EventArgs e)
        {
            if (Visible)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttons"></param>
        public void SetButtons(params String[] buttons)
        {
            if (buttons.Length > 0)
            {
                this.textOnButtons = new StringCollection();
                this.textOnButtons.AddRange(buttons);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new String ShowDialog()
        {
            base.ShowDialog();

            return (this.textOnButtonPressed);
        }
    }
}
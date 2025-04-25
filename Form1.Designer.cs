using System.Windows.Forms;

namespace VRCMidiTransfarSystem
{
    partial class VRChatMidiTransferSystem
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label2 = new Label();
            DeviceComboBox = new ComboBox();
            ReSearchButton = new Button();
            LogView = new ListView();
            label3 = new Label();
            OSCOptCheck = new CheckBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            txtdisplayCheck = new CheckBox();
            textBox1 = new TextBox();
            VRCConnection = new Button();
            IP1 = new TextBox();
            IP2 = new TextBox();
            label8 = new Label();
            IP3 = new TextBox();
            IP4 = new TextBox();
            label9 = new Label();
            Port = new TextBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            menuStrip1 = new MenuStrip();
            Menu = new ToolStripMenuItem();
            Language = new ToolStripMenuItem();
            Japanese = new ToolStripMenuItem();
            English = new ToolStripMenuItem();
            Korean = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            Credit = new ToolStripMenuItem();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 26);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 1;
            label2.Text = "MIDIデバイス";
            // 
            // DeviceComboBox
            // 
            DeviceComboBox.FormattingEnabled = true;
            DeviceComboBox.Location = new Point(11, 44);
            DeviceComboBox.Name = "DeviceComboBox";
            DeviceComboBox.Size = new Size(212, 23);
            DeviceComboBox.TabIndex = 2;
            DeviceComboBox.SelectedIndexChanged += DeviceComboBox_SelectedIndexChanged;
            // 
            // ReSearchButton
            // 
            ReSearchButton.Location = new Point(226, 44);
            ReSearchButton.Name = "ReSearchButton";
            ReSearchButton.Size = new Size(30, 23);
            ReSearchButton.TabIndex = 3;
            ReSearchButton.Text = "↺";
            ReSearchButton.UseVisualStyleBackColor = true;
            ReSearchButton.Click += ReSearchButton_Click;
            // 
            // LogView
            // 
            LogView.Location = new Point(11, 93);
            LogView.Name = "LogView";
            LogView.Scrollable = false;
            LogView.Size = new Size(248, 197);
            LogView.TabIndex = 6;
            LogView.UseCompatibleStateImageBehavior = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 75);
            label3.Name = "label3";
            label3.Size = new Size(78, 15);
            label3.TabIndex = 5;
            label3.Text = "MIDI受信ログ";
            // 
            // OSCOptCheck
            // 
            OSCOptCheck.AutoSize = true;
            OSCOptCheck.Location = new Point(12, 26);
            OSCOptCheck.Name = "OSCOptCheck";
            OSCOptCheck.Size = new Size(118, 19);
            OSCOptCheck.TabIndex = 8;
            OSCOptCheck.Text = "OSCオプション設定";
            OSCOptCheck.UseVisualStyleBackColor = true;
            OSCOptCheck.CheckedChanged += OSCOptCheck_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 52);
            label5.Name = "label5";
            label5.Size = new Size(55, 15);
            label5.TabIndex = 9;
            label5.Text = "IPアドレス";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 108);
            label6.Name = "label6";
            label6.Size = new Size(57, 15);
            label6.TabIndex = 17;
            label6.Text = "ポート番号";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(72, 80);
            label7.Name = "label7";
            label7.Size = new Size(10, 15);
            label7.TabIndex = 11;
            label7.Text = ".";
            // 
            // txtdisplayCheck
            // 
            txtdisplayCheck.AutoSize = true;
            txtdisplayCheck.Location = new Point(12, 169);
            txtdisplayCheck.Name = "txtdisplayCheck";
            txtdisplayCheck.Size = new Size(242, 19);
            txtdisplayCheck.TabIndex = 19;
            txtdisplayCheck.Text = "接続成功時にVRChatにメッセージを表示する";
            txtdisplayCheck.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 206);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(252, 23);
            textBox1.TabIndex = 20;
            textBox1.Text = "接続成功。VMTS Ver1.0";
            // 
            // VRCConnection
            // 
            VRCConnection.Enabled = false;
            VRCConnection.Location = new Point(12, 253);
            VRCConnection.Name = "VRCConnection";
            VRCConnection.Size = new Size(252, 30);
            VRCConnection.TabIndex = 21;
            VRCConnection.Text = "接続";
            VRCConnection.UseVisualStyleBackColor = true;
            VRCConnection.Click += VRCConnection_Click;
            // 
            // IP1
            // 
            IP1.Enabled = false;
            IP1.ImeMode = ImeMode.Disable;
            IP1.Location = new Point(35, 72);
            IP1.MaxLength = 3;
            IP1.Name = "IP1";
            IP1.ShortcutsEnabled = false;
            IP1.Size = new Size(30, 23);
            IP1.TabIndex = 10;
            IP1.TextAlign = HorizontalAlignment.Right;
            IP1.KeyPress += IP1_KeyPress;
            // 
            // IP2
            // 
            IP2.Enabled = false;
            IP2.ImeMode = ImeMode.Disable;
            IP2.Location = new Point(88, 72);
            IP2.MaxLength = 3;
            IP2.Name = "IP2";
            IP2.ShortcutsEnabled = false;
            IP2.Size = new Size(30, 23);
            IP2.TabIndex = 12;
            IP2.KeyPress += IP2_KeyPress;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(124, 80);
            label8.Name = "label8";
            label8.Size = new Size(10, 15);
            label8.TabIndex = 13;
            label8.Text = ".";
            // 
            // IP3
            // 
            IP3.Enabled = false;
            IP3.ImeMode = ImeMode.Disable;
            IP3.Location = new Point(140, 72);
            IP3.MaxLength = 3;
            IP3.Name = "IP3";
            IP3.ShortcutsEnabled = false;
            IP3.Size = new Size(30, 23);
            IP3.TabIndex = 14;
            IP3.KeyPress += IP3_KeyPress;
            // 
            // IP4
            // 
            IP4.Enabled = false;
            IP4.ImeMode = ImeMode.Disable;
            IP4.Location = new Point(192, 72);
            IP4.MaxLength = 3;
            IP4.Name = "IP4";
            IP4.ShortcutsEnabled = false;
            IP4.Size = new Size(30, 23);
            IP4.TabIndex = 16;
            IP4.KeyPress += IP4_KeyPress;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(176, 80);
            label9.Name = "label9";
            label9.Size = new Size(10, 15);
            label9.TabIndex = 15;
            label9.Text = ".";
            // 
            // Port
            // 
            Port.Enabled = false;
            Port.ImeMode = ImeMode.Disable;
            Port.Location = new Point(35, 131);
            Port.MaxLength = 4;
            Port.Name = "Port";
            Port.ShortcutsEnabled = false;
            Port.Size = new Size(40, 23);
            Port.TabIndex = 18;
            Port.KeyPress += Port_KeyPress;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(DeviceComboBox);
            groupBox1.Controls.Add(ReSearchButton);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(LogView);
            groupBox1.Location = new Point(12, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(270, 300);
            groupBox1.TabIndex = 23;
            groupBox1.TabStop = false;
            groupBox1.Text = "MIDI設定";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(OSCOptCheck);
            groupBox2.Controls.Add(Port);
            groupBox2.Controls.Add(IP1);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(IP4);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(IP3);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(txtdisplayCheck);
            groupBox2.Controls.Add(IP2);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Controls.Add(VRCConnection);
            groupBox2.Location = new Point(291, 27);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(270, 300);
            groupBox2.TabIndex = 24;
            groupBox2.TabStop = false;
            groupBox2.Text = "VRChat OSC設定";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { Menu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(571, 24);
            menuStrip1.TabIndex = 25;
            menuStrip1.Text = "menuStrip1";
            // 
            // Menu
            // 
            Menu.BackColor = SystemColors.ControlLight;
            Menu.DropDownItems.AddRange(new ToolStripItem[] { Language, toolStripSeparator1, Credit });
            Menu.Name = "Menu";
            Menu.Size = new Size(50, 20);
            Menu.Text = "Menu";
            // 
            // Language
            // 
            Language.DropDownItems.AddRange(new ToolStripItem[] { Japanese, English, Korean });
            Language.Name = "Language";
            Language.Size = new Size(126, 22);
            Language.Text = "Language";
            // 
            // Japanese
            // 
            Japanese.Checked = true;
            Japanese.CheckState = CheckState.Checked;
            Japanese.Name = "Japanese";
            Japanese.Size = new Size(112, 22);
            Japanese.Text = "日本語";
            Japanese.Click += Japanease_Click;
            // 
            // English
            // 
            English.Name = "English";
            English.Size = new Size(112, 22);
            English.Text = "English";
            English.Click += English_Click;
            // 
            // Korean
            // 
            Korean.Name = "Korean";
            Korean.Size = new Size(112, 22);
            Korean.Text = "한국어";
            Korean.Click += Korean_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(123, 6);
            // 
            // Credit
            // 
            Credit.Name = "Credit";
            Credit.Size = new Size(126, 22);
            Credit.Text = "クレジット";
            Credit.Click += Credit_Click;
            // 
            // VRChatMidiTransferSystem
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Control;
            ClientSize = new Size(571, 331);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(menuStrip1);
            Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            MaximumSize = new Size(587, 370);
            MinimumSize = new Size(587, 370);
            Name = "VRChatMidiTransferSystem";
            Text = "VRCMidiTransferSystem　Ver1.0";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private ComboBox DeviceComboBox;
        private Button ReSearchButton;
        private ListView LogView;
        private Label label3;
        private CheckBox OSCOptCheck;
        private Label label5;
        private Label label6;
        private Label label7;
        private CheckBox txtdisplayCheck;
        private TextBox textBox1;
        private Button VRCConnection;
        private TextBox IP1;
        private TextBox IP2;
        private Label label8;
        private TextBox IP3;
        private TextBox IP4;
        private Label label9;
        private TextBox Port;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem Menu;
        private ToolStripMenuItem Language;
        private ToolStripMenuItem Japanese;
        private ToolStripMenuItem English;
        private ToolStripMenuItem Korean;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem Credit;
    }
}

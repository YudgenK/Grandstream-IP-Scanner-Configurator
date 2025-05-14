namespace FormsSipPhone
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtStartIp = new System.Windows.Forms.TextBox();
            this.txtEndIp = new System.Windows.Forms.TextBox();
            this.chkConfigureSyslog = new System.Windows.Forms.CheckBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameSip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSaveToYaml = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SysLogHostIp = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SysLogLevelDegug = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.configNat = new System.Windows.Forms.CheckBox();
            this.ConfigAstrisk = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.EditPass = new System.Windows.Forms.CheckBox();
            this.textboxPass = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TextBoxSipServerIp = new System.Windows.Forms.TextBox();
            this.EditSipServerChek = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.EditPhoneBook = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TextPBXml = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStartIp
            // 
            this.txtStartIp.Location = new System.Drawing.Point(79, 19);
            this.txtStartIp.Name = "txtStartIp";
            this.txtStartIp.Size = new System.Drawing.Size(100, 20);
            this.txtStartIp.TabIndex = 0;
            this.txtStartIp.Text = "192.168.0.0";
            // 
            // txtEndIp
            // 
            this.txtEndIp.Location = new System.Drawing.Point(79, 45);
            this.txtEndIp.Name = "txtEndIp";
            this.txtEndIp.Size = new System.Drawing.Size(100, 20);
            this.txtEndIp.TabIndex = 1;
            this.txtEndIp.Text = "192.168.0.254";
            // 
            // chkConfigureSyslog
            // 
            this.chkConfigureSyslog.AutoSize = true;
            this.chkConfigureSyslog.Location = new System.Drawing.Point(6, 19);
            this.chkConfigureSyslog.Name = "chkConfigureSyslog";
            this.chkConfigureSyslog.Size = new System.Drawing.Size(226, 17);
            this.chkConfigureSyslog.TabIndex = 2;
            this.chkConfigureSyslog.Text = "Изменять настройки телефона(SysLog)";
            this.chkConfigureSyslog.UseVisualStyleBackColor = true;
            this.chkConfigureSyslog.CheckStateChanged += new System.EventHandler(this.chkConfigureSyslog_CheckStateChanged);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(620, 541);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(135, 23);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "Начать сканирование";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.NameSip});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(499, 389);
            this.dataGridView1.TabIndex = 4;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "IpAddress";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "SipNumber";
            this.Column2.Name = "Column2";
            // 
            // NameSip
            // 
            this.NameSip.HeaderText = "Имя пользователя";
            this.NameSip.Name = "NameSip";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Начальний IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Конечний IP:";
            // 
            // btnSaveToYaml
            // 
            this.btnSaveToYaml.Location = new System.Drawing.Point(12, 407);
            this.btnSaveToYaml.Name = "btnSaveToYaml";
            this.btnSaveToYaml.Size = new System.Drawing.Size(168, 37);
            this.btnSaveToYaml.TabIndex = 7;
            this.btnSaveToYaml.Text = "Сохранить в файл импорта Zabbix";
            this.btnSaveToYaml.UseVisualStyleBackColor = true;
            this.btnSaveToYaml.Click += new System.EventHandler(this.btnSaveToYaml_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(239, 407);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(272, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // SysLogHostIp
            // 
            this.SysLogHostIp.Enabled = false;
            this.SysLogHostIp.Location = new System.Drawing.Point(132, 42);
            this.SysLogHostIp.Name = "SysLogHostIp";
            this.SysLogHostIp.Size = new System.Drawing.Size(100, 20);
            this.SysLogHostIp.TabIndex = 10;
            this.SysLogHostIp.Text = "192.168.0.170";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.SysLogLevelDegug);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.SysLogHostIp);
            this.groupBox1.Controls.Add(this.chkConfigureSyslog);
            this.groupBox1.Location = new System.Drawing.Point(518, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 100);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройка параметров SysLog";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Уровень логирования:";
            // 
            // SysLogLevelDegug
            // 
            this.SysLogLevelDegug.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.SysLogLevelDegug.Enabled = false;
            this.SysLogLevelDegug.FormattingEnabled = true;
            this.SysLogLevelDegug.Items.AddRange(new object[] {
            "NONE",
            "DEBUG",
            "INFO",
            "WARNING",
            "ERROR"});
            this.SysLogLevelDegug.Location = new System.Drawing.Point(132, 69);
            this.SysLogLevelDegug.Name = "SysLogLevelDegug";
            this.SysLogLevelDegug.Size = new System.Drawing.Size(100, 21);
            this.SysLogLevelDegug.TabIndex = 12;
            this.SysLogLevelDegug.Text = "NONE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "IP сервера SysLog:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FormsSipPhone.Properties.Resources.@__Iphone_spinner_1;
            this.pictureBox1.Location = new System.Drawing.Point(210, 407);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtStartIp);
            this.groupBox2.Controls.Add(this.txtEndIp);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(518, 465);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 70);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройка сканирования";
            // 
            // configNat
            // 
            this.configNat.AutoSize = true;
            this.configNat.Location = new System.Drawing.Point(6, 19);
            this.configNat.Name = "configNat";
            this.configNat.Size = new System.Drawing.Size(106, 17);
            this.configNat.TabIndex = 14;
            this.configNat.Text = "Отключить NAT";
            this.configNat.UseVisualStyleBackColor = true;
            // 
            // ConfigAstrisk
            // 
            this.ConfigAstrisk.Location = new System.Drawing.Point(12, 444);
            this.ConfigAstrisk.Name = "ConfigAstrisk";
            this.ConfigAstrisk.Size = new System.Drawing.Size(168, 41);
            this.ConfigAstrisk.TabIndex = 15;
            this.ConfigAstrisk.Text = "Создать конфигурацию для Astrisk";
            this.ConfigAstrisk.UseVisualStyleBackColor = true;
            this.ConfigAstrisk.Click += new System.EventHandler(this.ConfigAstrisk_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.configNat);
            this.groupBox3.Location = new System.Drawing.Point(518, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(246, 45);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Сеть";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.EditPass);
            this.groupBox4.Controls.Add(this.textboxPass);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.TextBoxSipServerIp);
            this.groupBox4.Controls.Add(this.EditSipServerChek);
            this.groupBox4.Location = new System.Drawing.Point(518, 179);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(246, 104);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Настройки аккаунта";
            // 
            // EditPass
            // 
            this.EditPass.AutoSize = true;
            this.EditPass.Location = new System.Drawing.Point(9, 57);
            this.EditPass.Name = "EditPass";
            this.EditPass.Size = new System.Drawing.Size(116, 17);
            this.EditPass.TabIndex = 5;
            this.EditPass.Text = "Изменить пароль";
            this.EditPass.UseVisualStyleBackColor = true;
            this.EditPass.CheckedChanged += new System.EventHandler(this.EditPass_CheckedChanged);
            // 
            // textboxPass
            // 
            this.textboxPass.Enabled = false;
            this.textboxPass.Location = new System.Drawing.Point(140, 74);
            this.textboxPass.Name = "textboxPass";
            this.textboxPass.Size = new System.Drawing.Size(100, 20);
            this.textboxPass.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoEllipsis = true;
            this.label6.Location = new System.Drawing.Point(11, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Новый пароль(WEB):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Ip Sip server:";
            // 
            // TextBoxSipServerIp
            // 
            this.TextBoxSipServerIp.Enabled = false;
            this.TextBoxSipServerIp.Location = new System.Drawing.Point(140, 35);
            this.TextBoxSipServerIp.Name = "TextBoxSipServerIp";
            this.TextBoxSipServerIp.Size = new System.Drawing.Size(100, 20);
            this.TextBoxSipServerIp.TabIndex = 1;
            // 
            // EditSipServerChek
            // 
            this.EditSipServerChek.AutoSize = true;
            this.EditSipServerChek.Location = new System.Drawing.Point(9, 20);
            this.EditSipServerChek.Name = "EditSipServerChek";
            this.EditSipServerChek.Size = new System.Drawing.Size(127, 17);
            this.EditSipServerChek.TabIndex = 0;
            this.EditSipServerChek.Text = "Изменить Sip server";
            this.EditSipServerChek.UseVisualStyleBackColor = true;
            this.EditSipServerChek.CheckedChanged += new System.EventHandler(this.EditSipServerChek_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.TextPBXml);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.EditPhoneBook);
            this.groupBox5.Location = new System.Drawing.Point(518, 290);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(246, 100);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Телефонная книга";
            // 
            // EditPhoneBook
            // 
            this.EditPhoneBook.AutoSize = true;
            this.EditPhoneBook.Location = new System.Drawing.Point(9, 20);
            this.EditPhoneBook.Name = "EditPhoneBook";
            this.EditPhoneBook.Size = new System.Drawing.Size(165, 17);
            this.EditPhoneBook.TabIndex = 0;
            this.EditPhoneBook.Text = "Изменить настройки книги";
            this.EditPhoneBook.UseVisualStyleBackColor = true;
            this.EditPhoneBook.CheckedChanged += new System.EventHandler(this.EditPhoneBook_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "XML Адрес:";
            // 
            // TextPBXml
            // 
            this.TextPBXml.Enabled = false;
            this.TextPBXml.Location = new System.Drawing.Point(140, 44);
            this.TextPBXml.Name = "TextPBXml";
            this.TextPBXml.Size = new System.Drawing.Size(100, 20);
            this.TextPBXml.TabIndex = 2;
       
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 612);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.ConfigAstrisk);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnSaveToYaml);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Настройка Sip телефонов";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtStartIp;
        private System.Windows.Forms.TextBox txtEndIp;
        private System.Windows.Forms.CheckBox chkConfigureSyslog;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSaveToYaml;
        private System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.TextBox SysLogHostIp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox SysLogLevelDegug;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.CheckBox configNat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameSip;
        private System.Windows.Forms.Button ConfigAstrisk;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TextBoxSipServerIp;
        private System.Windows.Forms.CheckBox EditSipServerChek;
        private System.Windows.Forms.TextBox textboxPass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox EditPass;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox TextPBXml;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox EditPhoneBook;
    }
}


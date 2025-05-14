using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using YamlDotNet.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static PhoneScanner;



namespace FormsSipPhone
{
    public partial class Form1 : Form
    {
        private PhoneScanner scanner;
        private List<PhoneInfo> phones;
        private ScanOptions GetScanOptions()
        {
            return new ScanOptions
            {
                StartIp = txtStartIp.Text, // Поле с IP начального диапазона
                EndIp = txtEndIp.Text,     // Поле с IP конечного диапазона
                ConfigureSyslog = chkConfigureSyslog.Checked, // Чекбокс с логами
                Progress = new Progress<int>(value => progressBar1.Value = value), // Для обновления ProgressBar
                ProgressBar = progressBar1,  // Сам ProgressBar
                ConfigNatValue = configNat.Checked, // Чекбокс NAT
                EditSipServerChek = EditSipServerChek.Checked, // Чекбокс Ip sip server
                SipServerIp = TextBoxSipServerIp.Text, // Айпи Sip Server
                PassEdit = EditPass.Checked, //Замена пароля
                stringPass = textboxPass.Text,
                PhoneBookXML =  TextPBXml.Text,
                CheckPhoneBook = EditPhoneBook.Checked
            };
        }
        public Form1()
        {
            InitializeComponent();
            scanner = new PhoneScanner("config.txt", 5);
            phones = new List<PhoneInfo>();
        }

        private async void btnScan_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            
            progressBar1.Value = 0; // Сброс прогресс-бара
            string startIp = txtStartIp.Text;
            string endIp = txtEndIp.Text;
            bool configureSyslog = chkConfigureSyslog.Checked;
            btnScan.Enabled = false;
            phones.Clear();
            dataGridView1.Rows.Clear();
            var options = GetScanOptions();
            var progress = new Progress<int>(value => progressBar1.Value = value);
            // Запуск сканирования
            phones = await scanner.ScanIpRangeAsync(options);

            
            // Заполнение таблицы результатами
            foreach (var phone in phones)
            {
                dataGridView1.Rows.Add(phone.IpAddress, phone.SipNumber, phone.PhoneName);
            }
            progressBar1.Value = 0;
            pictureBox1 .Visible = false;
            btnScan.Enabled = true;
        }

        private void btnSaveToYaml_Click(object sender, EventArgs e)
        {
            try
            {
                scanner.GenerateYamlImportFile(phones);
                MessageBox.Show("YAML файл импорта успешно создан и сохранен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Файл импорта не создан ошибка: " + ex.Message);
            }
        }

        private void chkConfigureSyslog_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkConfigureSyslog.Checked)
            {
                SysLogHostIp.Enabled = true;
                SysLogLevelDegug.Enabled = true;
            }
            else
            {
                SysLogHostIp.Enabled = false; 
                SysLogLevelDegug.Enabled = false;
            }
        }

        public bool GetCheckBoxValue()
        {
           
            if (this.InvokeRequired)
            {
                return (bool)this.Invoke((Func<bool>)(() => configNat.Checked));
            }
            else
            {
                return configNat.Checked;
            }
        }

        private void ConfigAstrisk_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog
            {
                Title = "Сохранить SIP конфигурации",
                Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*",
                FileName = "sip_configs.csv"
            })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string csvContent = CsvGenerator.GenerateAsteriskCsv(phones);
                    CsvGenerator.SaveCsvToFile(csvContent, saveFileDialog.FileName);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void EditSipServerChek_CheckedChanged(object sender, EventArgs e)
        {
            if (EditSipServerChek.Checked)
            {
                TextBoxSipServerIp.Enabled = true;
            }
            else
            {
                TextBoxSipServerIp.Enabled = false;
            }
        }

        private void EditPass_CheckedChanged(object sender, EventArgs e)
        {
            if (EditPass.Checked)
            {
                textboxPass.Enabled = true;
            }
            else
            {
                textboxPass.Enabled = false;
            }
        }
        private void EditPhoneBook_CheckedChanged(object sender, EventArgs e)
        {
            if (EditPhoneBook.Checked)
            {
                TextPBXml.Enabled = true;
            }
            else
            {
                TextPBXml.Enabled = false;
            }
        }
    }
}

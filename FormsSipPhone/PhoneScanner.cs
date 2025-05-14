using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using FormsSipPhone;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using YamlDotNet.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


public class PhoneInfo
{
    public string IpAddress { get; set; }
    public string SipNumber { get; set; }

    public string PhoneName { get; set; }
}

public class PhoneScanner
{
    private List<(string Username, string Password)> credentials;
    private SemaphoreSlim semaphore;
    private List<string> failedConfigurations = new List<string>();


    public PhoneScanner(string credentialsFilePath, int maxConcurrentTasks)
    {
        this.credentials = LoadCredentials(credentialsFilePath);
        this.semaphore = new SemaphoreSlim(maxConcurrentTasks);

    }
    /// <summary>
    /// Свойства Sip телефона
    /// </summary>
    public class ScanOptions
    {
        public string StartIp { get; set; }
        public string EndIp { get; set; }
        public bool ConfigureSyslog { get; set; }
        public IProgress<int> Progress { get; set; }
        public System.Windows.Forms.ProgressBar ProgressBar { get; set; }
        public bool ConfigNatValue { get; set; }
        public string SipServerIp { get; set; }
        public bool EditSipServerChek { get; set; }
        public bool PassEdit { get; set; }
        public string stringPass { get; set; }

        public string PhoneBookXML { get; set; }

        public bool CheckPhoneBook { get; set; }
    }

    /// <summary>
    /// Сканирует сеть на доступные айпи
    /// </summary>
    /// <param name="options">Свойства телефона</param>
    /// <returns></returns>
    public async Task<List<PhoneInfo>> ScanIpRangeAsync(ScanOptions options)
    {
        string startIp = options.StartIp;
        string endIp = options.EndIp;
        bool configureSyslog = options.ConfigureSyslog;
        IProgress<int> progress = options.Progress;
        System.Windows.Forms.ProgressBar progressBar = options.ProgressBar;
        bool configNatValue = options.ConfigNatValue;


        var phoneList = new List<PhoneInfo>();
        var tasks = new List<Task<PhoneInfo>>();
        var ipRange = GetIpRange(options).ToList();
        progressBar.Maximum = 0;
        var availableIps = new List<string>();

        Parallel.ForEach(GetIpRange(options), ipAddress =>
        {
            if (PingHost(ipAddress))
            {
                availableIps.Add(ipAddress);
            }

        });
        progressBar.Maximum = availableIps.Count;
        foreach (var ipAddress in availableIps)
        {

            tasks.Add(ScanIpAsync(ipAddress, options));
        }
        await Task.WhenAll(tasks);

        phoneList.AddRange(tasks.Where(t => t.Result != null).Select(t => t.Result));
        return phoneList;
    }
    /// <summary>
    /// Начинает проверять доступные адреса айпи и запускает настройку телефона если он найден
    /// </summary>
    /// <param name="ipAddress"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    private async Task<PhoneInfo> ScanIpAsync(string ipAddress, ScanOptions options)
    {
        var progressBar = options.ProgressBar;
        var collectedPhones = new List<PhoneInfo>();

        await semaphore.WaitAsync();
        try
        {
            int countTry = 0;
            foreach (var (username, password) in credentials)
            {

                var sipData = await Task.Run(() => FindPhoneAndGetSipData(ipAddress, username, password, options));
                if (sipData != null)
                {
                    countTry++;
                    foreach (var data in sipData)
                    {

                        if (!string.IsNullOrEmpty(data.Sip))
                        {
                            return new PhoneInfo
                            {
                                IpAddress = ipAddress,
                                SipNumber = data.Sip,
                                PhoneName = data.Value
                            };
                        }
                        if(!string.IsNullOrEmpty(data.Value) && !collectedPhones.Any(phone => phone.IpAddress == ipAddress) && countTry >= credentials.Count)
                        {
                            return new PhoneInfo
                            {
                                IpAddress = ipAddress,
                                SipNumber = data.Sip,
                                PhoneName = data.Value
                            };
                        }
                       
                    }
                }
                
            }
            return null;
        }
        finally
        {
            progressBar.Invoke((Action)(() =>
            {
                progressBar.Value += 1;
            }));
            semaphore.Release();
        }
    }

    private List<(string Sip, string Value)> FindPhoneAndGetSipData(string ipAddress, string username, string password, ScanOptions options)
    {
        var configurePass = options.PassEdit;
        var configureSyslog = options.ConfigureSyslog;
        var configNatValue = options.ConfigNatValue;
        var optionsChrome = new ChromeOptions();
        var checkPhoneBook = options.CheckPhoneBook;
        optionsChrome.AddArgument("--headless");
        optionsChrome.AddArgument("--no-sandbox");
        optionsChrome.AddArgument("--disable-dev-shm-usage");
        optionsChrome.AddArgument("--window-size=1920,1080");

        // Настройка ChromeDriverService для скрытия консольного окна
        var service = ChromeDriverService.CreateDefaultService();
        service.HideCommandPromptWindow = true;

        using (var driver = new ChromeDriver(service, optionsChrome))
        {
            try
            {
                driver.Navigate().GoToUrl($"http://{ipAddress}/");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                Thread.Sleep(4000);

                var phoneElement = driver.FindElements(By.ClassName("gwt-HTML"))
                    .FirstOrDefault(el => el.Text.Contains("Copyright © Grandstream Networks, Inc. 2025. All Rights Reserved."));
                var errorLoad = driver.FindElements(By.XPath("//span[@jsselect='heading' and text()='Не удается получить доступ к сайту']"))
                         .FirstOrDefault();
                if (phoneElement == null || errorLoad != null)
                {
                    // Закрываем браузер и освобождаем ресурсы, если элемент не найден
                    driver.Quit();
                    return null;
                }

                // Выполняем вход
                try
                {
                    driver.FindElement(By.ClassName("gwt-TextBox")).SendKeys(username);
                    driver.FindElement(By.ClassName("gwt-PasswordTextBox")).SendKeys(password);
                    driver.FindElement(By.ClassName("gwt-Button")).Click();
                }
                catch
                {
                    driver.FindElement(By.ClassName("gwt-PasswordTextBox")).SendKeys(password);
                    driver.FindElement(By.ClassName("gwt-Button")).Click();
                }

                Thread.Sleep(3000);
                bool isAuthorized = false;
                try
                {
                    var parentElement = wait.Until(drv => drv.FindElement(By.ClassName("table-row")));

                    var sipElements = parentElement.FindElements(By.ClassName("gwt-HTML"))
                   .Where(el => !el.Text.Contains("Account"))
                   .Select(el => el.Text.Trim())
                   .ToList();

                    var additionalSipElements = parentElement.FindElements(By.ClassName("column-accounts"))
                        .Select(el => el.Text.Trim())
                        .ToList();

                    sipElements.AddRange(additionalSipElements);

                    var accountsType = wait.Until(drv => drv.FindElement(By.XPath("//td[@class='gwt-MenuItem' and text()='Accounts']")));
                    accountsType.Click();
                    isAuthorized = true;


                    // Получаем все элементы с именем 'P270' и 'Pxxx' (или другим SIP-атрибутом)
                    string className = "gwt-TextBox";
                    string elementName = "P270";
                    Thread.Sleep(3000);
                    // Выполнение JavaScript для получения всех значений элементов input(Тут Получается имя пользователя)
                    var inputValues = ((IList<object>)((IJavaScriptExecutor)driver).ExecuteScript(
                        "var elements = document.getElementsByClassName(arguments[0]); " +
                        "var values = []; " +
                        "for (var i = 0; i < elements.length; i++) { " +
                        "    if (elements[i].getAttribute('name') === arguments[1]) { " +
                        "        values.push(elements[i].value); " +
                        "    } " +
                        "} " +
                        "return values;",
                        className, elementName
                    )).Select(value => value?.ToString() ?? string.Empty).ToList();

                    //Изменения Ip Sip Server
                    if (options.EditSipServerChek)
                    {
                        var divElement = driver.FindElement(By.CssSelector("div.cell.contents"));

                        // Поиск <input> внутри найденного <div>
                        var inputElement = driver.FindElement(By.XPath("//input[@name='P47' and @class='gwt-TextBox']"));

                        // Очистка поля ввода и запись нового значения
                        inputElement.Clear();
                        inputElement.SendKeys(options.SipServerIp);
                        var saveButton = wait.Until(drv => drv.FindElement(By.XPath("//button[@class='gwt-Button' and text()='Save and Apply']")));
                        saveButton.Click();
                    }


                    // Собираем SIP и Value в пары
                    var sipData = new List<(string Sip, string Value)>();

                    for (int i = 0; i < Math.Min(sipElements.Count, inputValues.Count); i++)
                    {
                        string sipValue = sipElements[i];
                        string inputValue = inputValues[i];

                        // Добавляем в список пару SIP и Value
                        sipData.Add((sipValue, inputValue));
                    }

                    // Вывод результата
                    foreach (var item in sipData)
                    {
                        Console.WriteLine($"SIP: {item.Sip}, Value: {item.Value}");
                    }
                    // Выполняем настройки, если нужно
                    if (configureSyslog)
                    {
                        ConfigureSyslogAsync(driver, "192.168.7.170", "ERROR", ipAddress).Wait();
                    }

                    if (configNatValue)
                    {
                        ConfigereNat(driver, ipAddress);
                    }
                    if (configurePass)//Должен быть последним так как после приминения выбрасывает 
                    {
                        ConfigerePass(driver, ipAddress, options);
                    }
                    if (checkPhoneBook)
                    {
                        PhoneBookConfigure(driver, ipAddress, options);
                    }

                    return sipData;
                }
                catch
                {
                    isAuthorized = false;
                }


                if (!isAuthorized)
                {
                    return new List<(string Sip, string Value)>
                {
                    (null, "Не удалось авторизоваться")
                };
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
    private async Task ConfigerePass(IWebDriver driver, string ipAddress, ScanOptions options)
    {
        try
        {
            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            // Переход к вкладке Maintenance
            var maintenanceTab = wait.Until(drv => drv.FindElement(By.XPath("//td[@class='gwt-MenuItem' and text()='Maintenance']")));
            maintenanceTab.Click();

            var passNew = wait.Until(drv => drv.FindElement(By.XPath("//input[@class='gwt-PasswordTextBox' and @name='P196']")));
            passNew.Clear();
            passNew.SendKeys(options.stringPass);

            var passConfirm = wait.Until(drv => drv.FindElement(By.XPath("//input[@class='gwt-PasswordTextBox' and @name=':confirmUserPwd']")));
            passConfirm.Clear();
            passConfirm.SendKeys(options.stringPass);

            var passNewAdmin = wait.Until(drv => drv.FindElement(By.XPath("//input[@class='gwt-PasswordTextBox' and @name='P2']")));
            passNewAdmin.Clear();
            passNewAdmin.SendKeys(options.stringPass);

            var passConfirmAdmin = wait.Until(drv => drv.FindElement(By.XPath("//input[@class='gwt-PasswordTextBox' and @name=':confirmAdminPwd']")));
            passConfirmAdmin.Clear();
            passConfirmAdmin.SendKeys(options.stringPass);

            var saveButton = wait.Until(drv => drv.FindElement(By.XPath("//button[@class='gwt-Button' and text()='Save and Apply']")));
            saveButton.Click();
        }
        catch
        {
            failedConfigurations.Add(ipAddress);
        }
    }

    private async Task PhoneBookConfigure(IWebDriver driver, string ipAddress, ScanOptions options)
    {
        try
        {

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            // Переход к вкладке Phonebook
            var maintenanceTab = wait.Until(drv => drv.FindElement(By.XPath("//td[@class='gwt-MenuItem' and text()='Phonebook']")));
            maintenanceTab.Click();
            //Подменю Phonebook Management
            var syslogSection = wait.Until(drv => drv.FindElement(By.XPath("//div[@class='verticalMenu level1']//div[@class='label' and text()='Phonebook Management']")));
            syslogSection.Click();

            var syslogLevelDropdown = wait.Until(drv => drv.FindElement(By.XPath("//select[@class='gwt-ListBox last' and @name='P330']")));
            var selectElementsyslog = new SelectElement(syslogLevelDropdown);
            selectElementsyslog.SelectByText("Enabled, use HTTP");


            var passConfirm = wait.Until(drv => drv.FindElement(By.XPath("//input[@class='gwt-TextBox last' and @name='P331']")));
            passConfirm.Clear();
            passConfirm.SendKeys(options.PhoneBookXML);

            try
            {
                var DownLoadInterval = wait.Until(drv => drv.FindElement(By.XPath("//input[@class='gwt-TextBox last' and @name='P332']")));
                DownLoadInterval.Clear();
                DownLoadInterval.SendKeys("5");
            }
            catch
            {

            }
            try
            {
                var KeyFunction = wait.Until(drv => drv.FindElement(By.XPath("//select[@class='gwt-ListBox last' and @name='P330']")));
                var selectKeyFunction = new SelectElement(KeyFunction);
                selectKeyFunction.SelectByText("Local Phonebook");
            }
            catch
            {

            }

            var saveButton = wait.Until(drv => drv.FindElement(By.XPath("//button[@class='gwt-Button' and text()='Save and Apply']")));
            saveButton.Click();

        }
        catch
        {
            failedConfigurations.Add(ipAddress);
        }
    }
    private async Task ConfigereNat(IWebDriver driver, string ipAddress)//Настройка NAT
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            // Переход к вкладке Maintenance
            var maintenanceTab = wait.Until(drv => drv.FindElement(By.XPath("//td[@class='gwt-MenuItem gwt-MenuItem-selected' and text()='Accounts']")));
            maintenanceTab.Click();


            // Переход к разделу Syslog
            var syslogSection = wait.Until(drv => drv.FindElement(By.XPath("//div[@class='verticalMenu level2']//div[@class='label' and text()='Network Settings']")));
            syslogSection.Click();

            var primeryIP = wait.Until(drv => drv.FindElement(By.XPath("//input[@class='gwt-TextBox' and @name='P2308']")));
           
            primeryIP.Clear();

            var syslogLevelDropdown = wait.Until(drv => drv.FindElement(By.XPath("//select[@class='gwt-ListBox last' and @name='P52']")));
            var selectElementsyslog = new SelectElement(syslogLevelDropdown);
            selectElementsyslog.SelectByText("Auto");

            var saveButton = wait.Until(drv => drv.FindElement(By.XPath("//button[@class='gwt-Button' and text()='Save and Apply']")));
            saveButton.Click();
        }
        catch
        {
            failedConfigurations.Add(ipAddress);
        }
    }

    private async Task ConfigureSyslogAsync(IWebDriver driver, string syslogIp, string syslogLevel, string ipAddress)//Настройка Syslog
    {
        try
        {
            FormsSipPhone.Form1 form1 = new FormsSipPhone.Form1();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            // Переход к вкладке Maintenance
            var maintenanceTab = wait.Until(drv => drv.FindElement(By.XPath("//td[@class='gwt-MenuItem' and text()='Maintenance']")));
            maintenanceTab.Click();

            // Переход к разделу Syslog
            var syslogSection = wait.Until(drv => drv.FindElement(By.XPath("//div[@class='verticalMenu level1']//div[@class='label' and text()='Syslog']")));
            syslogSection.Click();

            // Установка значения IP для Syslog
            var syslogIpField = wait.Until(drv => drv.FindElement(By.XPath("//input[@class='gwt-TextBox last' and @name='P207']")));
            syslogIpField.Clear();
            syslogIpField.SendKeys(form1.SysLogHostIp.Text);

            // Установка уровня логирования Syslog
            var syslogLevelDropdown = wait.Until(drv => drv.FindElement(By.XPath("//select[@class='gwt-ListBox last' and @name='P208']")));
            var selectElement = new SelectElement(syslogLevelDropdown);
            selectElement.SelectByText(form1.SysLogLevelDegug.SelectedItem.ToString());

            //Установка Радиокнопки в положение yes
            var yesRadioButton = wait.Until(drv => drv.FindElement(By.XPath("//span[@class='gwt-RadioButton last']//input[@type='radio' and @value='1']")));
            yesRadioButton.Click();

            // Нажатие на кнопку Сохранить
            var saveButton = wait.Until(drv => drv.FindElement(By.XPath("//button[@class='gwt-Button' and text()='Save and Apply']")));
            saveButton.Click();
        }
        catch (Exception ex)
        {
            failedConfigurations.Add(ipAddress);
        }
    }
    /// <summary>
    /// Отправка Ping 
    /// </summary>
    /// <param name="ipAddress">Назначение пинга</param>
    /// <returns></returns>
    private bool PingHost(string ipAddress, int port = 80, int timeout = 1000)
    {
        try
        {
            using (var client = new TcpClient())
            {
                var result = client.BeginConnect(ipAddress, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(timeout);
                if (success)
                {
                    client.EndConnect(result);
                    return true; // Порт открыт
                }
            }
        }
        catch
        {
            // Игнорируем исключения, порт закрыт
        }
        return false; // Порт закрыт или недоступен
    }

    private static string GetMacAddress(string ipAddress)
    {
        try
        {
            // Выполняем ARP для получения MAC-адреса
            var arpProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "arp",
                    Arguments = "-a",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            arpProcess.Start();
            string output = arpProcess.StandardOutput.ReadToEnd();
            arpProcess.WaitForExit();

            // Разбиваем строку вывода на строки
            string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                // Проверяем, содержит ли строка искомый IP-адрес
                if (line.Contains(ipAddress))
                {
                    string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        return parts[1]; // MAC-адрес во втором элементе
                    }
                }
            }

            return null; // Если MAC-адрес не найден
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении MAC-адреса: {ex.Message}");
            return null;
        }
    }
    /// <summary>
    /// Построение списка айпи адресов 
    /// </summary>
    /// <param name="options">Парамер который дожен содержать StartIp и EndIp</param>
    /// <returns></returns>
    private IEnumerable<string> GetIpRange(ScanOptions options)
    {
        var startIp = options.StartIp;
        var endIp = options.EndIp;

        var start = System.Net.IPAddress.Parse(startIp).GetAddressBytes();
        var end = System.Net.IPAddress.Parse(endIp).GetAddressBytes();

        for (byte b1 = start[0]; b1 <= end[0]; b1++)
        {
            for (byte b2 = start[1]; b2 <= end[1]; b2++)
            {
                for (byte b3 = start[2]; b3 <= end[2]; b3++)
                {
                    for (byte b4 = start[3]; b4 <= end[3]; b4++)
                    {
                        yield return $"{b1}.{b2}.{b3}.{b4}";
                    }
                }
            }
        }
    }
    /// <summary>
    /// Гененерация файла импорта в Zabbix
    /// </summary>
    /// <param name="phones">Лист телефонов с которых будет состоять файл</param>
    public void GenerateYamlImportFile(List<PhoneInfo> phones)
    {
        if (failedConfigurations.Count > 0)
        {
            Console.WriteLine("\nНе удалось обновить настройки для следующих IP-адресов:");
            foreach (var failedIp in failedConfigurations)
            {
                Console.WriteLine(failedIp);
            }
        }
        else
        {
            Console.WriteLine("\nВсе настройки обновлены успешно.");
        }
        //Чистим список перед добавление в файл импорта
        for (int i = phones.Count - 1; i >= 0; i--)
        {
            if (phones[i].SipNumber == "Не удалось авторизоваться")
            {
                phones.RemoveAt(i);
            }
        }
        var hostNamesByIpPattern = new Dictionary<string, string>//Добавление к каждой подсети имени региона
        {
            { "192.168.0.", "Office-GP" },
            { "192.168.1.", "KREMENCHUG" },
            { "192.168.2.", "GP-SKLAD" },
            { "192.168.4.", "KIROVOGRAD" },
            { "192.168.5.", "KRIVOY-ROG" },
            { "192.168.6.", "Belaya-Tserkov" },
            { "192.168.7.", "Kiev-Servernaya" },
            { "192.168.8.", "Poltava" },
            { "192.168.9.", "Lubni" },
            { "192.168.10.", "Pavlograd" },
            { "192.168.11.", "Dnepr" },
            { "192.168.12.", "Lubni-Garazh" },
            { "192.168.15.", "Kiev" }

        };
        if (phones != null)
        {

            var hostName = "UNKNOW-REGION";
            var hostGroupUuid = Guid.NewGuid().ToString("N");
            var yamlExport = new
            {
                zabbix_export = new
                {
                    version = "7.0",
                    host_groups = new[]
                    {
                    new { uuid = hostGroupUuid, name = "IP phone" }
                },
                    hosts = phones.Select(phone =>
                    {
                        // Определение имени хоста на основе шаблона IP-адреса
                        var currentHostName = hostName;
                        foreach (var pattern in hostNamesByIpPattern.Keys)
                        {
                            if (phone.IpAddress.StartsWith(pattern))
                            {
                                currentHostName = hostNamesByIpPattern[pattern];
                                break;
                            }
                        }

                        return new
                        {
                            host = $"{currentHostName}-SIP Phone {phone.SipNumber}".TrimEnd(),
                            name = $"{currentHostName}-SIP Phone {phone.SipNumber}".TrimEnd(),
                            templates = new[]
                            {
                            new { name = "Template Module ICMP Ping" }
                        },
                            groups = new[]
                            {
                            new { name = "IP phone" }
                        },
                            interfaces = new[]
                            {
                            new { ip = phone.IpAddress, interface_ref = "if1" }
                        },
                            inventory_mode = "DISABLED"
                        };
                    }).ToArray()
                }
            };

            var serializer = new SerializerBuilder().Build();

            var yamlContent = serializer.Serialize(yamlExport);
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "YAML files (*.yaml)|*.yaml|All files (*.*)|*.*",
                Title = "Сохранить YAML файл",
                FileName = "Zabbix.yaml"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = saveFileDialog.FileName;

                // Запись в выбранный файл
                File.WriteAllText(filePath, yamlContent);

                // Чтение строк в List
                var lines = File.ReadAllLines(filePath).ToList();

                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Contains("version:"))
                    {
                        lines[i] = "  version: '7.0'";
                    }
                }
                File.WriteAllLines(filePath, lines);
            }

        }
    }
    /// <summary>
    /// Считывает все логины и пароли с файла txt
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private List<(string Username, string Password)> LoadCredentials(string filePath)
    {
        var credentials = new List<(string Username, string Password)>();
        var lines = File.ReadAllLines(filePath);
        string username = null;

        foreach (var line in lines)
        {
            if (line.StartsWith("Login:"))
                username = line.Substring("Login:".Length).Trim();
            else if (line.StartsWith("Pass:") && username != null)
            {
                var password = line.Substring("Pass:".Length).Trim();
                credentials.Add((username, password));
                username = null;
            }
        }
        return credentials;
    }
}

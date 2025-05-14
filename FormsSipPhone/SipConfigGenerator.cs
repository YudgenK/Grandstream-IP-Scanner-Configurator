using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium;

namespace FormsSipPhone
{
    internal class SipConfigGenerator
    {
        public static string GenerateSipConfig(string extension, string name, string context = "from-internal")
        {
            return $@"
[{extension}]
type=endpoint
aors={extension}
auth={extension}-auth
tos_audio=ef
tos_video=af41
cos_audio=5
cos_video=4
allow=ulaw,alaw,gsm,g726,g722
context={context}
callerid={name} <{extension}>

dtmf_mode=rfc4733
direct_media=no
mailboxes={extension}@device

mwi_subscribe_replaces_unsolicited=yes
aggregate_mwi=no
use_avpf=no
rtcp_mux=no
max_audio_streams=1
max_video_streams=1
bundle=no
ice_support=no
media_use_received_transport=no
trust_id_inbound=yes
user_eq_phone=no
media_encryption=no
timers=yes
timers_min_se=90
media_encryption_optimistic=no
refer_blind_progress=yes
rtp_timeout=30
rtp_timeout_hold=300
rtp_keepalive=30
send_pai=yes
rtp_symmetric=yes
rewrite_contact=yes
force_rport=yes
language=ru
".Trim();
        }

        public static List<string> GenerateSipConfigsFromElements(List<PhoneInfo> phones)
        {
            if (phones == null || !phones.Any())
            {
                Console.WriteLine("Список телефонов пуст.");
                return new List<string>();
            }

            var configs = new List<string>();

            foreach (var phone in phones)
            {
                string sipNumber = phone.SipNumber;
                string name = string.IsNullOrEmpty(phone.PhoneName) ? $"User {sipNumber}" : phone.PhoneName;
                

                // Генерация конфигурации для каждого телефона
                configs.Add(GenerateSipConfig(sipNumber, name));
            }

            return configs;
        }

        public static void SaveConfigsToFile(List<string> configs, string outputPath)
        {
            var result = string.Join(Environment.NewLine + Environment.NewLine, configs);
            File.WriteAllText(outputPath, result);
            Console.WriteLine($"SIP конфигурации успешно созданы и записаны в файл: {outputPath}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsSipPhone
{
    internal class CsvGenerator
    {
        public static string GenerateAsteriskCsv(List<PhoneInfo> phones)
        {
            if (phones == null || !phones.Any())
            {
                Console.WriteLine("Список телефонов пуст.");
                return string.Empty;
            }

            // Заголовок CSV
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("extension,password,name,voicemail,ringtimer,noanswer,recording,outboundcid,sipname,noanswer_cid,busy_cid,chanunavail_cid,noanswer_dest,busy_dest,chanunavail_dest,mohclass,id,tech,dial,devicetype,user,description,emergency_cid,hint_override,cwtone,recording_in_external,recording_out_external,recording_in_internal,recording_out_internal,recording_ondemand,recording_priority,answermode,intercom,cid_masquerade,concurrency_limit,devicedata,accountcode,aggregate_mwi,allow,avpf,bundle,callerid,context,defaultuser,device_state_busy_at,direct_media,disallow,dtmfmode,force_callerid,force_rport,icesupport,mailbox,match,max_audio_streams,max_contacts,max_video_streams,maximum_expiration,md5_cred,media_address,media_encryption,media_encryption_optimistic,media_use_received_transport,message_context,minimum_expiration,mwi_subscription,namedcallgroup,namedpickupgroup,outbound_auth,outbound_proxy,qualifyfreq,refer_blind_progress,remove_existing,rewrite_contact,rtcp_mux,rtp_symmetric,rtp_timeout,rtp_timeout_hold,secret,send_connected_line,sendrpid,sipdriver,timers,timers_min_se,transport,trustrpid,user_eq_phone,vmexten,webrtc,callwaiting_enable,voicemail_enable,voicemail_vmpwd,voicemail_email,voicemail_pager,voicemail_options,voicemail_same_exten,disable_star_voicemail,vmx_unavail_enabled,vmx_busy_enabled,vmx_temp_enabled,vmx_play_instructions,vmx_option_0_number,vmx_option_1_number,vmx_option_2_number");

            // Заполнение данных
            foreach (var phone in phones)
            {
                string extension = phone.SipNumber ?? string.Empty;
                string password =  string.Empty;
                string name = phone.PhoneName ?? string.Empty;
                string voicemail =  "novm"; // Предположим, voicemail по умолчанию отключен

                // Строка данных
                csvBuilder.AppendLine($"{extension},{password},{name},{voicemail},0,,,,,,,,,,,default,{extension},pjsip,PJSIP/{extension},fixed,{extension},{name},,,disabled,dontcare,dontcare,dontcare,dontcare,disabled,10,disabled,enabled,{extension},3,{extension},,no,,no,no,\"{name} <{extension}>\",from-internal,,0,no,,rfc2833,,yes,no,{extension}@device,,1,1,1,7200,,,no,no,no,,60,auto,,,no,,60,yes,yes,yes,no,yes,0,0,frex{extension},yes,pai,chan_pjsip,yes,90,\"udp,tcp,tls\",yes,no,,no,ENABLED,,,,,,,,,,,,,,");
            }

            return csvBuilder.ToString();
        }

        public static void SaveCsvToFile(string csvContent, string outputPath)
        {
            try
            {
                File.WriteAllText(outputPath, csvContent);
                Console.WriteLine($"CSV файл успешно сохранен по пути: {outputPath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка записи файла: {ex.Message}");
            }
        }
    }
}


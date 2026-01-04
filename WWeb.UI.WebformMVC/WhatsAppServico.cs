using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using WhatsAppApi;
using WhatsAppApi.Account;
using WhatsAppApi.Helper;
using WhatsAppApi.Register;
using WhatsAppApi.Response;


namespace WWeb.ServicoWhatsApp
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 11/12/2015
    /// </summary>
    public static class WhatsAppServico
    {
        public static byte[] Foto { get; set; }

        static bool foto_gravada = false;
        static string nickname = ConfigurationManager.AppSettings["Application.Nickname"];
        static string sender_num = ConfigurationManager.AppSettings["Application.Phone"];
        static string password = ConfigurationManager.AppSettings["Application.KeyWhatsapp"];
        static WhatsApp wa;

        private static void ConectarWhatsApp()
        {
            wa = new WhatsApp(sender_num, password, nickname, true);

            wa.OnLoginSuccess += wa_OnLoginSuccess;
            wa.OnLoginFailed += wa_OnLoginFailed;
            wa.OnGetMessage += wa_OnGetMessage;
            wa.OnGetMessageReadedClient += wa_OnGetMessageReadedClient;
            wa.OnGetMessageReceivedClient += wa_OnGetMessageReceivedClient;
            wa.OnGetMessageReceivedServer += wa_OnGetMessageReceivedServer;
            wa.OnNotificationPicture += wa_OnNotificationPicture;
            wa.OnGetPresence += wa_OnGetPresence;
            wa.OnGetGroupParticipants += wa_OnGetGroupParticipants;
            wa.OnGetLastSeen += wa_OnGetLastSeen;
            wa.OnGetTyping += wa_OnGetTyping;
            wa.OnGetPaused += wa_OnGetPaused;
            wa.OnGetMessageImage += wa_OnGetMessageImage;
            wa.OnGetMessageAudio += wa_OnGetMessageAudio;
            wa.OnGetMessageVideo += wa_OnGetMessageVideo;
            wa.OnGetMessageLocation += wa_OnGetMessageLocation;
            wa.OnGetMessageVcard += wa_OnGetMessageVcard;
            wa.OnGetPhoto += wa_OnGetPhoto;
            wa.OnGetPhotoPreview += wa_OnGetPhotoPreview;
            wa.OnGetGroups += wa_OnGetGroups;
            wa.OnGetSyncResult += wa_OnGetSyncResult;
            wa.OnGetStatus += wa_OnGetStatus;
            wa.OnGetPrivacySettings += wa_OnGetPrivacySettings;
            DebugAdapter.Instance.OnPrintDebug += Instance_OnPrintDebug;

            wa.Connect();

            string datFile = getDatFileName(sender_num);
            byte[] nextChallenge = null;
            if (File.Exists(datFile))
            {
                try
                {
                    string foo = File.ReadAllText(datFile);
                    nextChallenge = Convert.FromBase64String(foo);
                }
                catch (Exception) { };
            }

            wa.Login(nextChallenge);
            wa.SendGetServerProperties();
        }

        private static void DesconectarWhatsApp()
        {
            if (wa != null)
                wa.Disconnect();
        }

        public static void EnviarMensagemTexto(string to, string msg)
        {
            try
            {
                ConectarWhatsApp();
                wa.SendMessage(to, msg);
                System.Threading.Thread.Sleep(1500);
                wa.Disconnect();
            }
            catch { }
        }

        public static void ObterFotoPerfil(string cliente, string path_imagem)
        {
            foto_gravada = false;
            try
            {
                ConectarWhatsApp();
                wa.SendGetPhoto("55" + cliente, "55" + cliente, true);

                while (!foto_gravada)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() => wa.PollMessages());
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch
            {
                Foto = File.ReadAllBytes(path_imagem);
            }
            finally
            {
                if (wa != null)
                {
                    if (wa.ConnectionStatus != ApiBase.CONNECTION_STATUS.DISCONNECTED)
                        wa.Disconnect();
                }
            }
        }

        private static void ConfigurarEvendtos()
        {
            wa.OnLoginSuccess += wa_OnLoginSuccess;
            wa.OnLoginFailed += wa_OnLoginFailed;
            wa.OnGetMessage += wa_OnGetMessage;
            wa.OnGetMessageReadedClient += wa_OnGetMessageReadedClient;
            wa.OnGetMessageReceivedClient += wa_OnGetMessageReceivedClient;
            wa.OnGetMessageReceivedServer += wa_OnGetMessageReceivedServer;
            wa.OnNotificationPicture += wa_OnNotificationPicture;
            wa.OnGetPresence += wa_OnGetPresence;
            wa.OnGetGroupParticipants += wa_OnGetGroupParticipants;
            wa.OnGetLastSeen += wa_OnGetLastSeen;
            wa.OnGetTyping += wa_OnGetTyping;
            wa.OnGetPaused += wa_OnGetPaused;
            wa.OnGetMessageImage += wa_OnGetMessageImage;
            wa.OnGetMessageAudio += wa_OnGetMessageAudio;
            wa.OnGetMessageVideo += wa_OnGetMessageVideo;
            wa.OnGetMessageLocation += wa_OnGetMessageLocation;
            wa.OnGetMessageVcard += wa_OnGetMessageVcard;
            wa.OnGetPhoto += wa_OnGetPhoto;
            wa.OnGetPhotoPreview += wa_OnGetPhotoPreview;
            wa.OnGetGroups += wa_OnGetGroups;
            wa.OnGetSyncResult += wa_OnGetSyncResult;
            wa.OnGetStatus += wa_OnGetStatus;
            wa.OnGetPrivacySettings += wa_OnGetPrivacySettings;

            DebugAdapter.Instance.OnPrintDebug += Instance_OnPrintDebug;
        }

        #region WhatsappAPI
        static void Instance_OnPrintDebug(object value)
        {
            Console.WriteLine(value);
        }

        static void wa_OnGetPrivacySettings(Dictionary<ApiBase.VisibilityCategory, ApiBase.VisibilitySetting> settings)
        {
            throw new NotImplementedException();
        }

        static void wa_OnGetStatus(string from, string type, string name, string status)
        {
            Console.WriteLine(String.Format("Got status from {0}: {1}", from, status));
        }

        static string getDatFileName(string pn)
        {
            string filename = string.Format("{0}.next.dat", pn);
            return Path.Combine(Directory.GetCurrentDirectory(), filename);
        }

        static void wa_OnGetSyncResult(int index, string sid, Dictionary<string, string> existingUsers, string[] failedNumbers)
        {
            Console.WriteLine("Sync result for {0}:", sid);
            foreach (KeyValuePair<string, string> item in existingUsers)
            {
                Console.WriteLine("Existing: {0} (username {1})", item.Key, item.Value);
            }
            foreach (string item in failedNumbers)
            {
                Console.WriteLine("Non-Existing: {0}", item);
            }
        }

        static void wa_OnGetGroups(WaGroupInfo[] groups)
        {
            Console.WriteLine("Got groups:");
            foreach (WaGroupInfo info in groups)
            {
                Console.WriteLine("\t{0} {1}", info.subject, info.id);
            }
        }

        static void wa_OnGetPhotoPreview(string from, string id, byte[] data)
        {
            //Console.WriteLine("Got preview photo for {0}", from);
            //File.WriteAllBytes(string.Format("preview_{0}.jpg", from), data);
            Foto = data;
            foto_gravada = true;
        }

        static void wa_OnGetPhoto(string from, string id, byte[] data)
        {
            //Console.WriteLine("Got full photo for {0}", from);
            //File.WriteAllBytes(string.Format("{0}.jpg", from), data);
            Foto = data;
            foto_gravada = true;
        }

        static void wa_OnGetMessageVcard(ProtocolTreeNode vcardNode, string from, string id, string name, byte[] data)
        {
            Console.WriteLine("Got vcard \"{0}\" from {1}", name, from);
            File.WriteAllBytes(string.Format("{0}.vcf", name), data);
        }

        static void wa_OnGetMessageLocation(ProtocolTreeNode locationNode, string from, string id, double lon, double lat, string url, string name, byte[] preview, string userName)
        {
            Console.WriteLine("Got location from {0} username {1} ({1}, {2})", from, userName, lat, lon);
            if (!string.IsNullOrEmpty(name))
            {
                Console.WriteLine("\t{0}", name);
            }
            File.WriteAllBytes(string.Format("{0}{1}.jpg", lat, lon), preview);
        }

        static void wa_OnGetMessageVideo(ProtocolTreeNode mediaNode, string from, string id, string fileName, int fileSize, string url, byte[] preview, string name)
        {
            Console.WriteLine("Got video from {0} name{1}", from, name, fileName);
            OnGetMedia(fileName, url, preview);
        }

        static void OnGetMedia(string file, string url, byte[] data)
        {
            //save preview
            File.WriteAllBytes(string.Format("preview_{0}.jpg", file), data);
            //download
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFileAsync(new Uri(url), file, null);
            }
        }

        static void wa_OnGetMessageAudio(ProtocolTreeNode mediaNode, string from, string id, string fileName, int fileSize, string url, byte[] preview, string name)
        {
            Console.WriteLine("Got audio from {0} name{1}", from, name, fileName);
            OnGetMedia(fileName, url, preview);
        }

        static void wa_OnGetMessageImage(ProtocolTreeNode mediaNode, string from, string id, string fileName, int size, string url, byte[] preview, string name)
        {
            Console.WriteLine("Got image from {0} name{1}", from, name, fileName);
            OnGetMedia(fileName, url, preview);
        }

        static void wa_OnGetPaused(string from)
        {
            Console.WriteLine("{0} stopped typing", from);
        }

        static void wa_OnGetTyping(string from)
        {
            Console.WriteLine("{0} is typing...", from);
        }

        static void wa_OnGetLastSeen(string from, DateTime lastSeen)
        {
            Console.WriteLine("{0} last seen on {1}", from, lastSeen.ToString());
        }

        static void wa_OnGetMessageReadedClient(string from, string id)
        {
            Console.WriteLine("Message {0} to {1} readed by client", id, from);
        }

        static void wa_OnGetMessageReceivedServer(string from, string id)
        {
            Console.WriteLine("Message {0} to {1} received by server", id, from);
        }

        static void wa_OnGetMessageReceivedClient(string from, string id)
        {
            Console.WriteLine("Message {0} to {1} received by client", id, from);
        }

        static void wa_OnGetGroupParticipants(string gjid, string[] jids)
        {
            Console.WriteLine("Got participants from {0}:", gjid);
            foreach (string jid in jids)
            {
                Console.WriteLine("\t{0}", jid);
            }
        }

        static void wa_OnGetPresence(string from, string type)
        {
            Console.WriteLine("Presence from {0}: {1}", from, type);
        }

        static void wa_OnNotificationPicture(string type, string jid, string id)
        {
            //TODO
            //throw new NotImplementedException();
        }

        static void wa_OnGetMessage(ProtocolTreeNode node, string from, string id, string name, string message, bool receipt_sent)
        {
            //wa.SendMessage("5511961877317", message);
            //Console.WriteLine("Message from {0} {1}: {2}", name, from, message);
        }

        static private void wa_OnLoginFailed(string data)
        {
            Console.WriteLine("Login failed. Reason: {0}", data);
        }

        static private void wa_OnLoginSuccess(string phoneNumber, byte[] data)
        {
            //Console.WriteLine("Login success. Next password:");
            string sdata = Convert.ToBase64String(data);
            //Console.WriteLine(sdata);
            try
            {
                File.WriteAllText(getDatFileName(phoneNumber), sdata);
            }
            catch (Exception) { }
        }

        static private void ProcessChat(WhatsApp wa, string dst)
        {
            var thRecv = new Thread(t =>
            {
                try
                {
                    while (wa != null)
                    {
                        wa.PollMessages();
                        Thread.Sleep(100);
                        continue;
                    }

                }
                catch (ThreadAbortException)
                {
                }
            })
            { IsBackground = true };
            thRecv.Start();

            WhatsUserManager usrMan = new WhatsUserManager();
            var tmpUser = usrMan.CreateUser(dst, "User");

            while (true)
            {
                string line = "";
                if (line == null && line.Length == 0)
                    continue;

                string command = line.Trim();
                switch (command)
                {
                    case "/query":
                        //var dst = dst//trim(strstr($line, ' ', FALSE));
                        Console.WriteLine("[] Interactive conversation with {0}:", tmpUser);
                        break;
                    case "/accountinfo":
                        Console.WriteLine("[] Account Info: {0}", wa.GetAccountInfo().ToString());
                        break;
                    case "/lastseen":
                        Console.WriteLine("[] Request last seen {0}", tmpUser);
                        wa.SendQueryLastOnline(tmpUser.GetFullJid());
                        break;
                    case "/exit":
                        wa = null;
                        thRecv.Abort();
                        return;
                    case "/start":
                        wa.SendComposing(tmpUser.GetFullJid());
                        break;
                    case "/pause":
                        wa.SendPaused(tmpUser.GetFullJid());
                        break;
                    default:
                        Console.WriteLine("[] Send message to {0}: {1}", tmpUser, line);
                        wa.SendMessage(tmpUser.GetFullJid(), line);
                        break;
                }
            }
        }

        #endregion
    }
}

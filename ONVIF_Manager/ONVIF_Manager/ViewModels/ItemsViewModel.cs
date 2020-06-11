using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using ONVIF_Manager.Models;
using ONVIF_Manager.Views;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Reflection;

namespace ONVIF_Manager.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<ConnectionInfo> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<ConnectionInfo>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, ConnectionInfo>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as ConnectionInfo;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                /*
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "ONVIF_Manager.ws-request-net.xml";




                Socket udpSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                 

                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                        using (StreamReader reader = new StreamReader(stream))
                        {

                            UdpClient client = new UdpClient(port: 3702, family: AddressFamily.InterNetwork);

                            var destIp = System.Net.IPAddress.Parse("239.255.255.250");
                            client.JoinMulticastGroup(destIp);
                            string result = reader.ReadToEnd();

                           
                            
                            udpSock.Bind(new IPEndPoint(ip, 0));//your machine ip address

                            udpSock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(destIp));
                            udpSock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
                            IPEndPoint ipep = new IPEndPoint(destIp, 3702);


                            var bytes = Encoding.ASCII.GetBytes(result);
                            udpSock.SendTo(bytes, ipep);
                            break;
                        }
                    }
                }

                byte[] data = new byte[8000];
                int len = 0;
                EndPoint iep = new IPEndPoint(System.Net.IPAddress.Any, 3702);
                while ((len = udpSock.ReceiveFrom(data, ref iep)) > 0)
                {
                     Console.WriteLine("Received {0} bytes from {1}", len, iep.ToString());
                }
                */



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        static void ProbeMatchOpReceived(object sender, WSDiscovery.ProbeMatchOpReceivedEventArgs e)
        {

        }
    }
}
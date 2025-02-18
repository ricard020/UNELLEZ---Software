using System.Net.Sockets;
using System.Text;
using UTILITIES.CryptographyDataUtility;
using System.Net.NetworkInformation;
using Application = Microsoft.Maui.Controls.Application;
using System.Threading;
using SpinningTrainerTV.ViewTV;
using System.Net;
using ENTITYS;
using System.Text.Json;
using System.ComponentModel.Design;
using SpinningTrainerTV.Resources.Charts;

#if ANDROID
using Android.Content;
using Android.App;
using Android.Net.Wifi;
#endif

namespace SpinningTrainerTV.Socket
{
    public class DeviceResponder
    {
        private static string _ipConnected;
        private static UdpClient _udpClient = new UdpClient(5000);
        private CancellationTokenSource cts;

        private readonly ICryptographyDataUtility _cryptographyDataUtility;
         
        public DeviceResponder(ICryptographyDataUtility cryptographyDataUtility)
        {
            _cryptographyDataUtility = cryptographyDataUtility;
            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(15));
        }

        public static string GetWiFiIPAddress()
        {
            #if ANDROID
            try
            {
                using var wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
                if (wifiManager != null && wifiManager.ConnectionInfo != null)
                {
                    // Obtén la IP en formato entero.
                    int ip = wifiManager.ConnectionInfo.IpAddress;

                    // Convierte la IP de entero a formato legible.
                    return $"{(ip & 0xFF)}.{(ip >> 8 & 0xFF)}.{(ip >> 16 & 0xFF)}.{(ip >> 24 & 0xFF)}";
                }
                else
                {
                    return "No Wi-Fi Connected.";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
            #else
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Filtramos adaptadores WiFi con estado Activo
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                    networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    // Obtenemos las direcciones IP asociadas al adaptador
                    var ipProperties = networkInterface.GetIPProperties();
                    foreach (var ipAddress in ipProperties.UnicastAddresses)
                    {
                        if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ipAddress.Address.ToString(); // Retornamos la IP IPv4
                        }
                    }
                }
            }

            throw new Exception("No se pudo encontrar una dirección IP asociada al adaptador WiFi.");
            #endif
        }

        public async Task<(int, bool, IPEndPoint)> ControlSession(string userId)
        {
            Console.WriteLine("Esperando mensajes de descubrimiento...");
            
            int? actionReceived = null;
            IPEndPoint iPEndPoint = null;

            while (actionReceived == null)
            {
                var receiveTask = _udpClient.ReceiveAsync();
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(300));

                var completedTask = await Task.WhenAny(receiveTask, timeoutTask);

                if (completedTask == receiveTask)
                {
                    var result = await receiveTask;
                    var message = _cryptographyDataUtility.Decrypt(Encoding.UTF8.GetString(result.Buffer));
                    var requestArray = message.Split('|');

                    if (requestArray[0] == "CONTROL_SESSION")
                    {
                        if (requestArray[1] == userId)
                        {
                            try
                            {
                                actionReceived = 0;
                                var response = _cryptographyDataUtility.Encrypt($"{1}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                            catch (Exception)
                            {
                                var response = _cryptographyDataUtility.Encrypt($"{0}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                        }
                    }
                    else if (requestArray[0] == "NEXT_EXECISE")
                    {
                        if (requestArray[1] == userId)
                        {
                            try
                            {
                                actionReceived = 1;
                                var response = _cryptographyDataUtility.Encrypt($"{1}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                            catch (Exception)
                            {
                                var response = _cryptographyDataUtility.Encrypt($"{0}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                        }
                    }
                    else if (requestArray[0] == "PREVIOUS_EXECISE")
                    {
                        if (requestArray[1] == userId)
                        {
                            try
                            {
                                actionReceived = 2;
                                var response = _cryptographyDataUtility.Encrypt($"{1}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                            catch (Exception)
                            {
                                var response = _cryptographyDataUtility.Encrypt($"{0}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                        }
                    }
                    else if (requestArray[0] == "RESTING_EXECISE")
                    {
                        if (requestArray[1] == userId)
                        {
                            try
                            {
                                actionReceived = 3;
                                var response = _cryptographyDataUtility.Encrypt($"{1}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                            catch (Exception)
                            {
                                var response = _cryptographyDataUtility.Encrypt($"{0}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                        }
                    }
                    else if (requestArray[0] == "INCREASE_RESISTANCE")
                    {
                        if (requestArray[1] == userId)
                        {
                            try
                            {
                                actionReceived = 4;
                                var response = _cryptographyDataUtility.Encrypt($"{1}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                            catch (Exception)
                            {
                                var response = _cryptographyDataUtility.Encrypt($"{0}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                        }
                    }
                    else if (requestArray[0] == "REDUCE_RESISTANCE")
                    {
                        if (requestArray[1] == userId)
                        {
                            try
                            {
                                actionReceived = 5;
                                var response = _cryptographyDataUtility.Encrypt($"{1}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                            catch (Exception)
                            {
                                var response = _cryptographyDataUtility.Encrypt($"{0}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                        }
                    }
                    else if (requestArray[0] == "HEARTBEAT")
                    {
                        if (requestArray[1] != userId)
                        {
                            await Application.Current.MainPage.DisplayAlert("Conexión Inconsistente", "Parece ser que se recibió una señal de otro usuario.", "Entendido");
                        }
                    }
                    else if (requestArray[0] == "DISCOVER_TV")
                    {
                        if (requestArray[1] == userId)
                        {
                            string response;

                            if (requestArray[3] == _ipConnected)
                            {
                                response = _cryptographyDataUtility.Encrypt($"3|{GetWiFiIPAddress()}|{userId}");

                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);
                                
                                iPEndPoint = result.RemoteEndPoint;
                                actionReceived = 1035;
                            }
                        }
                    }
                }
                else
                {
                    bool userWantsCancelConnection = await Application.Current.MainPage.DisplayAlert("Conexión Perdida", "Parece ser que el dispositivo móvil a dejado de responder. ¿Desea finalizar la conexión?", "Sí", "No");

                    if (userWantsCancelConnection)
                        actionReceived = 1034;
                }
            }

            if(actionReceived == 1034)
                return (1034, true, null);
            else
                return (actionReceived ?? 2000, false, iPEndPoint) /*2000: Número colocado al azar ya que el compilador necesita una opción B en caso de nulos*/;
        }

        public async Task<bool> SendCurrentSessioninPlayData(SessionEntity session,
                                                             List<SessionExercisesEntity> exerciseList,
                                                             int currentExerciseIndex,
                                                             int currentDataPointIndex,
                                                             List<DataPoint> datapoints,
                                                             double graphProgress,
                                                             double currentRPMValue,
                                                             double currentResistanceValue,
                                                             double currentTargetRPM,
                                                             DateTime startTime,
                                                             TimeSpan elapsedExerciseTime,
                                                             TimeSpan elapsedTime,
                                                             TimeSpan changeExerciseCountdown,
                                                             int userId,
                                                             IPEndPoint iPEndPoint)
        {
            try
            {
                string sessionJson = JsonSerializer.Serialize(session);
                string exerciseListJson = JsonSerializer.Serialize(exerciseList);
                string datapointsJson = JsonSerializer.Serialize(datapoints);
                string startTimeJson = JsonSerializer.Serialize(startTime);
                string elapsedExerciseTimeJson = JsonSerializer.Serialize(elapsedExerciseTime);
                string elapsedTimeJson = JsonSerializer.Serialize(elapsedTime);
                string changeExerciseCountdownJson = JsonSerializer.Serialize(changeExerciseCountdown);

                var message = _cryptographyDataUtility.Encrypt($"{sessionJson}|{exerciseListJson}|{currentExerciseIndex.ToString()}|{currentDataPointIndex.ToString()}|{datapointsJson}|{graphProgress}|{currentRPMValue.ToString()}|{currentResistanceValue.ToString()}|{currentTargetRPM}|{startTimeJson}|{elapsedExerciseTimeJson}|{elapsedTimeJson}|{changeExerciseCountdownJson}|{GetWiFiIPAddress()}");
                var messageData = Encoding.UTF8.GetBytes(message);
                await _udpClient.SendAsync(messageData, messageData.Length, iPEndPoint);

                var result = await _udpClient.ReceiveAsync();
                var response = _cryptographyDataUtility.Decrypt(Encoding.UTF8.GetString(result.Buffer));
                var responseSep = response.Split('|');

                if (responseSep[0] == _ipConnected)
                {
                    if (responseSep[1] == userId.ToString())
                    {
                        if (responseSep[2] == "1")
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<(int, IPEndPoint, bool)> WaitingForTheSessionToPlay(string userId)
        {
            int sessionID = 0;
            bool sessionIDReceived = false;
            IPEndPoint ipToResponse = null;

            while (!sessionIDReceived)
            {
                var receiveTask = _udpClient.ReceiveAsync();
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(60));

                var completedTask = await Task.WhenAny(receiveTask, timeoutTask);

                if (completedTask == receiveTask)
                {
                    var result = await receiveTask;
                    var message = _cryptographyDataUtility.Decrypt(Encoding.UTF8.GetString(result.Buffer));
                    var requestArray = message.Split('|');

                    if (requestArray[0] == "PLAY_SESSION")
                    {
                        if (requestArray[1] == userId)
                        {
                            sessionIDReceived = true;
                            sessionID = int.Parse(requestArray[2]);
                            ipToResponse = result.RemoteEndPoint;
                        }
                    }
                    else if (requestArray[0] == "HEARTBEAT")
                    {
                        if (requestArray[1] != userId)
                        {
                            await Application.Current.MainPage.DisplayAlert("Conexión Inconsistente", "Parece ser que se recibió una señal de otro usuario.", "Entendido");
                        }
                    }
                    else if (requestArray[0] == "DISCOVER_TV")
                    {
                        if (requestArray[1] == userId)
                        {
                            string response;

                            if (requestArray[3] == _ipConnected)
                            {
                                response = _cryptographyDataUtility.Encrypt($"2|{GetWiFiIPAddress()}|{userId}");
                                
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                        }
                    }
                }
                else
                {
                    bool userWantsCancelConnection = await Application.Current.MainPage.DisplayAlert("Conexión Perdida", "Parece ser que el dispositivo móvil a dejado de responder. ¿Desea finalizar la conexión?", "Sí", "No");

                    if (userWantsCancelConnection)
                    {
                        return (0, null, true);
                    }
                }
            }

            return (sessionID, ipToResponse, false);
        }

        public async Task<bool> SendSession(string userId, IPEndPoint ipToSend, SessionEntity session)
        {
            try
            {
                // Serializa el objeto a JSON
                string jsonString = JsonSerializer.Serialize(session);

                var response = _cryptographyDataUtility.Encrypt($"{jsonString}|{userId}");
                var responseData = Encoding.UTF8.GetBytes(response);
                await _udpClient.SendAsync(responseData, responseData.Length, ipToSend);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> WaitForTheConnectionAsync(string userId)
        {
            string ipAddress = GetWiFiIPAddress();

            Console.WriteLine("Esperando mensajes de descubrimiento...");

            bool connecting = true;

            while (connecting)
            {
                var result = await _udpClient.ReceiveAsync();
                var message = _cryptographyDataUtility.Decrypt(Encoding.UTF8.GetString(result.Buffer));
                var requestArray = message.Split('|');

                if (requestArray[0] == "DISCOVER_TV")
                {
                    if (requestArray[1] == userId)
                    {
                        bool confirm = await Application.Current.MainPage.DisplayAlert("Conexión desde un dispositivo", $"El dispositivo con nombre: {requestArray[2]} esta solicitando establecer conexión. ¿Desea permitir la conexión?", "Sí", "No");

                        string response;
                        if (confirm)
                        {
                            _ipConnected = requestArray[3];
                            response = _cryptographyDataUtility.Encrypt($"1|{ipAddress}|{userId}");
                            connecting = false;
                        }
                        else
                        {
                            response = _cryptographyDataUtility.Encrypt($"0|0.0.0.0|0");
                        }

                        var responseData = Encoding.UTF8.GetBytes(response);
                        await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                        Console.WriteLine($"Respondido a descubrimiento: {response}");
                    }
                }
            }

            return true;
        }

        public async Task<int> WaitingForTheFinishedSessionMessage(string userId)
        {
            Console.WriteLine("Esperando mensajes de descubrimiento...");

            int? actionReceived = null;

            while (actionReceived == null)
            {
                var receiveTask = _udpClient.ReceiveAsync();
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(300));

                var completedTask = await Task.WhenAny(receiveTask, timeoutTask);

                if (completedTask == receiveTask)
                {
                    var result = await receiveTask;
                    var message = _cryptographyDataUtility.Decrypt(Encoding.UTF8.GetString(result.Buffer));
                    var requestArray = message.Split('|');

                    if (requestArray[0] == "FINISH_SESSION")
                    {
                        if (requestArray[1] == userId)
                        {
                            try
                            {
                                actionReceived = 0;
                                var response = _cryptographyDataUtility.Encrypt($"{1}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                            catch (Exception)
                            {
                                var response = _cryptographyDataUtility.Encrypt($"{0}|{userId}");
                                var responseData = Encoding.UTF8.GetBytes(response);
                                await _udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                                Console.WriteLine($"Respondido a descubrimiento: {response}");
                            }
                        }
                    }
                }
            }

            return actionReceived ?? 2000;
        }

        public void StopResponder()
        {
            _udpClient.Close();
            Console.WriteLine("Receptor detenido.");
        }
    }
}
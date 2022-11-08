using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Net.NetworkInformation;
using WUApiLib;
using Microsoft.Win32;
using System.Diagnostics;

/// <summary>
/// 
/// </summary>
/// 

namespace Dll_Utilidades
{
    public class Especificaciones
    {
        //-------------------------------- Primer Punto ------------------------------------//

        public class Serial
        {
            public static string Serial_HDD()
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
                    foreach (ManagementObject queryObj in searcher.Get()) { return queryObj["SerialNumber"].ToString(); }
                }
                catch { return ""; }
                return "";
            }

            public static string Serial_CD()
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");
                    foreach (ManagementObject queryObj in searcher.Get()) { return queryObj["SerialCD"].ToString(); }
                }
                catch { return "No hay instancias disponibles."; }
                return "No hay instancias disponibles.";
            }
        }

        //-------------------------------- Segundo Punto ------------------------------------//

        public static string unidadDisco()
        {
            DriveInfo[] Discos = DriveInfo.GetDrives();
            string discos = "";
            foreach (DriveInfo disk in Discos)
            {
                discos += ($"Drive {disk.Name} \n\r" +
                    $"Drive type: {disk.DriveType} \n\r"
                    );
                if (disk.IsReady == true)
                {
                    discos += ($"Etiqueta de volumen: {disk.VolumeLabel} \n\r" +
                        $"Sistema de archivos: {disk.DriveFormat} \n\r" +
                        $"Espacio disponible para el usuario actual: {Convert.ToDecimal(disk.AvailableFreeSpace) / 1073741824} GB \n\r" +
                        $"Espacio total disponible: {Convert.ToDecimal(disk.TotalFreeSpace) / 1073741824} GB \n\r" +
                        $"Tamaño total de la unidad: {Convert.ToDecimal(disk.TotalSize) / 1073741824} GB \n\r"
                        );
                }
            }
            return discos;
        }

        //-------------------------------- Tercer Punto ------------------------------------//

        public class Procesador
        {
            public string ProcesadorNombre()
            {
                ManagementClass BuscarProcesador = new ManagementClass("win32_processor");
                ManagementObjectCollection Procesador = BuscarProcesador.GetInstances();
                string ProcesadorEncontrado = string.Empty;

                foreach (ManagementObject CPU in Procesador)
                {
                    ProcesadorEncontrado = (string)CPU["Name"] + ", " + (string)CPU["Caption"];
                }
                return ProcesadorEncontrado;
            }
            public string ProcesadorID()
            {
                ManagementClass BuscarIdProcesador = new ManagementClass("win32_processor");
                ManagementObjectCollection IDProcesador = BuscarIdProcesador.GetInstances();
                String Id = String.Empty;
                foreach (ManagementObject CPU in IDProcesador)
                {

                    Id = CPU.Properties["processorID"].Value.ToString();
                    break;
                }
                return Id;
            }
        }

        public class RAM
        {
            public string CantidadRAM()
            {
                long Tamaño = 0;

                ManagementScope RAM = new ManagementScope();
                ObjectQuery BuscarTamaño = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
                ManagementObjectSearcher Busqueda = new ManagementObjectSearcher(RAM, BuscarTamaño);
                ManagementObjectCollection Coleccion = Busqueda.Get();

                foreach (ManagementObject obj in Coleccion)
                {
                    Tamaño += Convert.ToInt64(obj["Capacity"]);
                }
                Tamaño = (Tamaño / 1024) / 1024;

                return Tamaño.ToString() + " MB";
            }
            public string SlotRAM()
            {

                int Slots = 0;
                ManagementScope SRam = new ManagementScope();
                ObjectQuery BuscarSlots = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");
                ManagementObjectSearcher Busqueda = new ManagementObjectSearcher(SRam, BuscarSlots);
                ManagementObjectCollection Coleccion = Busqueda.Get();

                foreach (ManagementObject obj in Coleccion)
                {
                    Slots = Convert.ToInt32(obj["MemoryDevices"]);
                }
                return Slots.ToString();
            }

        }

        public static string NIC()
        {
            string nombres = "";
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters) { nombres += adapter.Description + "\n\n"; }
            return nombres;
        }

        //-------------------------------- Cuarto Punto ------------------------------------//


        public string MACAddress()
        {
            ManagementClass BuscarMACAddress = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection MACAdress = BuscarMACAddress.GetInstances();
            string MacA = string.Empty;
            foreach (ManagementObject MAC in MACAdress)
            {
                if (MacA == string.Empty)
                {
                    if ((bool)MAC["IPEnabled"] == true) MacA = MAC["MacAddress"].ToString();
                }
                MAC.Dispose();
            }
            return MacA;
        }
        //-------------------------------- Quinto Punto ------------------------------------//

        public static void Main()
        {
            int opc = Menu();
            if (opc == 1)
            {
                Console.WriteLine("Ingrese la ruta del registro a leer. ");
                string source = Console.ReadLine();
                Console.WriteLine("Ingresa la llave del registro a leer");
                string key = Console.ReadLine();
                Console.WriteLine($"El valor que se encontro en la ruta{source}: es: {ReadRegistryValue(source, key)}");
            }
            else if (opc == 2)
            {
                Console.WriteLine("Ingrese la ruta del registro a leer. Nota:debes de agregarle un '\' al final");
                string source = Console.ReadLine();
                Console.WriteLine("Ingrese la llave del registro a editar");
                string keyname = Console.ReadLine();
                Console.WriteLine($"Ingrese el valor de la llave: {keyname}");
                string Value = Console.ReadLine();
                SetRegistryKeyValue(source, keyname, Value);
            }
        }

        private static void SetRegistryKeyValue(string source, string keyname, string Value)
        {
            Registry.SetValue(source, keyname, Value);
        }
        private static string ReadRegistryValue(string source, string key)
        {
            return Registry.GetValue(source, key, "No found").ToString();
        }

        public static int Menu()
        {
            Console.WriteLine("Bienvenido al editor de registro del sistema.\n\n");
            Console.WriteLine("Seleccione una opcion:\n");
            Console.WriteLine("\t1.-Leer un valor en el Registro del sistema");
            Console.WriteLine("\t2.-Editar una llave en el Registro del sistema");
            int opc = int.Parse(Console.ReadLine());
            return opc;


        }

        //-------------------------------- Sexto Punto ------------------------------------//

        public static List<string> Procesos()
        {
            List<string> Proceso = new List<string>();
            Process[] listaProcesos = Process.GetProcesses();
            foreach (Process procesos in listaProcesos) { Proceso.Add(procesos.ProcessName + "\n\r"); }
            return Proceso;
        }
        public static List<decimal> ProcesosMB()
        {
            List<decimal> MB = new List<decimal>();
            Process[] listaProcesos = Process.GetProcesses();
            foreach (Process procesos in listaProcesos)
            {
                MB.Add(procesos.PeakPagedMemorySize64 / 1048576);
            }
            return MB;
        }

        public static void ProcesosKill(string Nombre = "")
        {
            try
            {
                Process[] procesos = Process.GetProcessesByName(Nombre);
                foreach (Process proceso in procesos) { proceso.Kill(); }
            }
            catch { }
        }
    }
}


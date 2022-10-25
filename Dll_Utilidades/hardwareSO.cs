using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

/// <summary>
/// 
/// </summary>
/// 

namespace Dll_Utilidades
{
    public class hardwareSO
    {
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
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TelemetryDataNFSU2
{
    using static Nfsu2Adresses;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Runtime.CompilerServices;

    public class ProcessMemory : IDisposable
    {
        private const uint PROCESS_VM_READ = 0x0010;
        private const uint PROCESS_QUERY_INFORMATION = 0x0400;

        private nint _processHandle;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern nint OpenProcess(
            uint dwDesiredAccess,
            bool bInheritHandle,
            int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            nint hProcess,
            nint lpBaseAddress,
            byte[] lpBuffer,
            nint nSize,
            out nint lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(nint hObject);

        public ProcessMemory(int processId)
        {
            _processHandle = OpenProcess(
                PROCESS_VM_READ | PROCESS_QUERY_INFORMATION,
                false,
                processId);

            if (_processHandle == 0)
            {
                throw new InvalidOperationException(
                    "Failed to open process.");
            }
        }

        public byte[] ReadBytes(nint address, int size)
        {
            byte[] buffer = new byte[size];

            bool success = ReadProcessMemory(
                _processHandle,
                address,
                buffer,
                size,
                out nint bytesRead);

            if (!success || bytesRead != size)
            {
                throw new InvalidOperationException(
                    $"Failed to read memory at 0x{address:X}.");
            }

            return buffer;
        }

        public int ReadInt32(nint address)
        {
            byte[] bytes = ReadBytes(address, sizeof(int));
            return BitConverter.ToInt32(bytes);
        }

        public float ReadFloat(nint address)
        {
            byte[] bytes = ReadBytes(address, sizeof(float));
            return BitConverter.ToSingle(bytes);
        }

        public string ReadString(nint address, int length)
        {
            byte[] bytes = ReadBytes(address, length);
            return Encoding.ASCII.GetString(bytes).TrimEnd('\0');
        }

        public void Dispose()
        {
            if (_processHandle != 0)
            {
                CloseHandle(_processHandle);
                _processHandle = 0;
            }
        }
    }
}

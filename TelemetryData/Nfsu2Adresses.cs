using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryDataNFSU2
{
    public class Nfsu2Adresses
    {
        //All address consts value types MUST be with nint

        public const nint SpeedMph = 0x007F09E8;

        public const nint PositionX = 0x0082BE80;
        public const nint PositionY = 0x0082BE88;
        public const nint PositionZ = 0x0082BE84;

        public const nint Money = 0x00861E74;
        public const nint CarId = 0x00891CF8;
        public const nint PlayerName = 0x0083A9E0;
    }
}

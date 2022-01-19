using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind.enums
{
    public class Messages
    {
        public static string SetCode { get { return "Ułóż kod:"; } }
        public static string BreakCode { get { return "Spróbuj odgadnąć sekwencję:"; } }
        public static string Decoded { get { return "Sekwencja została odgadnięta. Ułóż kod:"; } }
        public static string NotDecoded { get { return "Sekwencja nie została odgadnięta. Ułóż kod:"; } }
        public static string NextAttempt { get { return "Spróbuj ponownie:"; } }
        public static string DecodedSingle { get { return "Sekwencja została odgadnięta. Spróbuj odgadnąć kolejną:"; } }
        public static string NotDecodedSingle { get { return "Sekwencja nie została odgadnięta. Spróbuj odgadnąć kolejną:"; } }
    }
}

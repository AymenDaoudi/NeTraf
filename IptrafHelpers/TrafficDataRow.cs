using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    public class TrafficDataRow
    {
        #region Fields
            const string unsignedIntegerPattern = @"\d+";
        #endregion
        #region Properties
            public PortType PortType { get; set; }
            public uint PortNumber { get; set; }
            public TrafficData TotalTrafficData { get; set; }
            public TrafficData IncomingTrafficData { get; set; }
            public TrafficData OutgoingTrafficData { get; set; }
        #endregion

        public TrafficDataRow(string loggedRow)
        {
            var splittedRow = loggedRow.Split(':',';');
            SetPort(splittedRow);   
            SetTrafficData(splittedRow);
        }

        private void SetPort(string[] splittedRow)
        {
            var portInfo = splittedRow[0].Split('/');
            switch (portInfo[0])
            {
                case "TCP": PortType = PortType.TCP;
                break;
                case "UDP": PortType = PortType.UDP;
                break;
                default: PortType = PortType.Other;
                break;
            }
            uint portNumber;
            uint.TryParse(portInfo[1], out portNumber);
            PortNumber = portNumber;
        }

        public void SetTrafficData(string[] splittedRow)
        {
            TotalTrafficData = ParseTrafficData(splittedRow,TrafficDataType.TotalData);
            IncomingTrafficData = ParseTrafficData(splittedRow,TrafficDataType.IncomingData);
            OutgoingTrafficData = ParseTrafficData(splittedRow,TrafficDataType.OutgoingData);
        }

        public TrafficData ParseTrafficData(string[] splittedRow,TrafficDataType TrafficDataType)
        {
            var integerRegex = new Regex(@"\d+");

            var data = splittedRow[(int)TrafficDataType + 1].Split(',');
            double packets;
            try
            {
                double.TryParse(integerRegex.Match(data[0]).Groups[0].Value, out packets);
            }
            catch (System.IndexOutOfRangeException)
            {
                packets = 0;
            }
            
            double bytes;
            try
            {
                double.TryParse(integerRegex.Match(data[1]).Groups[0].Value, out bytes);
            }
            catch (System.IndexOutOfRangeException)
            {
                bytes = 0;
            }
            
            return new TrafficData(){Packets = packets, Bytes = bytes};
        }
    }
}
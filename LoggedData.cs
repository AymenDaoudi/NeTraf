using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    public class LoggedData
    {
        #region Properties
            public PortType PortType { get; set; }
            public uint PortNumber { get; set; }
            public CollectedData TotalCollectedData { get; set; }
            public CollectedData IncomingCollectedData { get; set; }
            public CollectedData OutgoingCollectedData { get; set; }
        #endregion

        public LoggedData(string loggedData)
        {
            var splittedData = loggedData.Split(':',';');
            SetPort(splittedData);   
            SetCollectedData(splittedData);
        }

        private void SetPort(string[] splittedData)
        {
            var portInfo = splittedData[0].Split('/');
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

        public void SetCollectedData(string[] splittedData)
        {
            TotalCollectedData = ParseTrafficData(splittedData,CollectedDataType.TotalData);
            IncomingCollectedData = ParseTrafficData(splittedData,CollectedDataType.IncomingData);
            OutgoingCollectedData = ParseTrafficData(splittedData,CollectedDataType.OutgoingData);
        }

        public CollectedData ParseTrafficData(string[] splittedData,CollectedDataType collectedDataType)
        {
            var integerRegex = new Regex(@"\d+");

            var data = splittedData[(int)collectedDataType + 1].Split(',');
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
            
            return new CollectedData(){Packets = packets, Bytes = bytes};
        }
    }
}